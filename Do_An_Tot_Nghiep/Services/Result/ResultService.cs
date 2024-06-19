using AutoMapper;
using Do_An_Tot_Nghiep.Dto.Result;
using System.Linq;
using System.Net;
using System.Security.Claims;
using Do_An_Tot_Nghiep.Enums.Question;
using Microsoft.EntityFrameworkCore;
using NewProject.Services.Common;
using Newtonsoft.Json;


namespace Do_An_Tot_Nghiep.Services.Result;

public class ResultService : IResultService
{
    private readonly IDbServices _dbService;
    private PublicContext context = new PublicContext();
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ResultService(IDbServices dbService, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _dbService = dbService;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<object> Submit(SubmitQuestionDto input)
    {
        try
        {
            var submittedQuestionIds = input.ResultOfUser.Select(s => s.IdQuestion).ToList();
            var submittedAnswerIds = input.ResultOfUser.Select(s => s.IdAnswer).ToList();

            int listeningCorrect = 0;
            int readingCorrect = 0;
            int listeningWrong = 0;
            int readingWrong = 0;
            var query = from question in context.QuestionToeics
                where submittedQuestionIds.Contains(question.Id)
                join answers in context.AnswerToeics on question.Id equals answers.IdQuestion into answersGroup
                // from selectedAnswer in answersGroup.DefaultIfEmpty()
                // where submittedAnswerIds.Contains(selectedAnswer.Id)
                join groupQuestion in context.GroupQuestions on question.IdGroupQuestion equals groupQuestion.Id into
                    groupQuestionGroup
                from groups in groupQuestionGroup.DefaultIfEmpty()
                select new
                {
                    Id = question.Id,
                    Content = question.Content,
                    PartId = question.PartId,
                    ImageUrl = question.ImageUrl,
                    AudioUrl = question.AudioUrl,
                    Transcription = question.Transcription,
                    NumberSTT = question.NumberSTT,
                    Type = question.Type,
                    IdGroupQuestion = question.IdGroupQuestion,
                    CreationTime = question.CreationTime,
                    Group = groups != null
                        ? new
                        {
                            ImageUrl = groups.ImageUrl,
                            AudioUrl = groups.AudioUrl,
                            Content = groups.Content,
                            Transcription = groups.Transcription,
                        }
                        : null,
                    Answer = context.AnswerToeics.Where(a => a.IdQuestion == question.Id)
                        .Where(a => submittedAnswerIds.Contains(a.Id))
                        .Select(answerChoose => new
                        {
                            Id = answerChoose.Id,
                            Content = answerChoose.Content,
                            IsBoolean = answerChoose.IsBoolean,
                            Transcription = answerChoose.Transcription,
                        }).FirstOrDefault(),
                    AnswersQuestion = context.AnswerToeics
                        .Where(a => a.IdQuestion == question.Id)
                        .Select(a => new { a.Id, a.IsBoolean }).ToList()
                };
            var results = query.ToList();
            var sortedResults = results.OrderBy(r => submittedQuestionIds.IndexOf(r.Id)).ToList();

            foreach (var item in sortedResults)
            {
                if (item.PartId >= (PART_TOEIC?)1 && item.PartId <= (PART_TOEIC?)4)
                {
                    // Listening part
                    if (item.Answer != null && item.Answer.IsBoolean)
                        listeningCorrect++;
                    else
                        listeningWrong++;
                }
                else if (item.PartId >= (PART_TOEIC?)5 && item.PartId <= (PART_TOEIC?)7)
                {
                    if (item.Answer != null && item.Answer.IsBoolean)
                        readingCorrect++;
                    else
                        readingWrong++;
                }
            }

            var jsonResult = JsonConvert.SerializeObject(sortedResults);
            var resultEntry = new Models.Result()
            {
                Data = jsonResult,
                UserId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("Id")),
                TimeStart = input.TimeStart,
                TimeEnd = DateTime.Now,
                NumberCorrect = readingCorrect + listeningCorrect,
            };
            if (input.IdExam.HasValue)
            {
                resultEntry.IdExam = input.IdExam;
            }

            await context.Results.AddAsync(resultEntry);
            await context.SaveChangesAsync();

            return new
            {
                ListeningCorrect = listeningCorrect,
                ReadingCorrect = readingCorrect,
                TotalCorrect = listeningCorrect + readingCorrect,
                TotalWrong = listeningWrong + readingWrong,
                Details = resultEntry.Data
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<object> GetById(int id)
    {
        try
        {
            var result = await context.Results
                .Where(r => r.Id == id)
                .Select(r => new
                {
                    Data = r.Data,
                    Id = r.Id,
                    TimeStart = r.TimeStart,
                    TimeEnd = r.TimeEnd,
                    ExamName = r.IdExam.HasValue
                        ? context.ExamToeics
                            .Where(e => e.Id == r.IdExam.Value)
                            .Select(e => e.NameExam)
                            .FirstOrDefault()
                        : null
                })
                .FirstOrDefaultAsync();

            if (result == null)
            {
                throw new Exception("Bad Request");
            }

            return DataResult.ResultSuccess(result, "Thành công");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<object> HistoryForUser(GetHistoryResultDto parameters)
    {
        try
        {
            var userId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("Id"));
            var query = context.Results
                .Where(r => r.UserId == userId)
                .Select(r => new
                {
                    Data = r.Data,
                    Id = r.Id,
                    TimeStart = r.TimeStart,
                    TimeEnd = r.TimeEnd,
                    ExamId = r.IdExam,
                    ExamName = r.IdExam.HasValue
                        ? context.ExamToeics
                            .Where(e => e.Id == r.IdExam.Value)
                            .Select(e => e.NameExam)
                            .FirstOrDefault()
                        : null
                });
            if (query == null)
            {
                return DataResult.ResultSuccess(query, "Không có dữ liệu");
            }

            if (parameters.ExamId.HasValue)
            {
                query = query.Where(x => x.ExamId == parameters.ExamId);
            }

            var result = query.Skip(parameters.SkipCount).Take(parameters.MaxResultCount).ToList();
            return DataResult.ResultSuccess(result, "", query.Count());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
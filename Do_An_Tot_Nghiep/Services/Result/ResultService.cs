using AutoMapper;
using Do_An_Tot_Nghiep.Dto.Result;
using System.Linq;
using System.Security.Claims;
using Do_An_Tot_Nghiep.Enums.Question;
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
            var submittedQuestionIds = input.resultOfUser.Select(s => s.IdQuestion).ToList();
            var submittedAnswerIds = input.resultOfUser.Select(s => s.IdAnswer).ToList();
            int listeningCorrect = 0;
            int readingCorrect = 0;
            int listeningWrong = 0;
            int readingWrong = 0;
            var query = from question in context.QuestionToeics
                where submittedQuestionIds.Contains(question.Id)
                join answers in context.AnswerToeics on question.Id equals answers.IdQuestion into answersGroup
                from selectedAnswer in answersGroup
                where submittedAnswerIds.Contains(selectedAnswer.Id)
                join groupQuestion in context.GroupQuestions on question.IdGroupQuestion equals groupQuestion.Id into groupQuestionGroup
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
                    Group = groups != null ? new
                    {
                        ImageUrl = groups.ImageUrl,
                        AudioUrl = groups.AudioUrl,
                        Content = groups.Content,
                        Transcription = groups.Transcription,
                    } : null,
                    Answer = new
                    {
                        Id = selectedAnswer.Id,
                        Content = selectedAnswer.Content,
                        IsBoolean = selectedAnswer.IsBoolean, 
                        Transcription = selectedAnswer.Transcription
                    }
                };
            var results =  query.ToList();
            
            foreach (var item in results)
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
            
            var jsonResult = JsonConvert.SerializeObject(results);
            var resultEntry = new Models.Result()
            {
                
                Data = jsonResult,
                UserId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("Id")),
                TimeStart = input.TimeStart,
                TimeEnd = input.TimeEnd
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
                Details = results.Select(r => new {
                    r.Id,
                    r.Content,
                    r.Type,
                    r.PartId,
                    Correct = (r.Answer != null && submittedAnswerIds.Contains(r.Answer.Id) && r.Answer.IsBoolean),
                    AnswerId = r.Answer?.Id
                }).ToList()
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
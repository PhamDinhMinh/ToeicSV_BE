using System.Net;
using System.Text.RegularExpressions;
using AutoMapper;
using Do_An_Tot_Nghiep.Dto.Question;
using Do_An_Tot_Nghiep.Enums.Question;
using Do_An_Tot_Nghiep.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using NewProject.Services.Common;
using OfficeOpenXml;

namespace Do_An_Tot_Nghiep.Services.Question;

public class QuestionService : IQuestionService
{
    private readonly IDbServices _dbService;
    private PublicContext context = new PublicContext();
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public QuestionService(IDbServices dbService, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _dbService = dbService;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }


    public async Task<object> CreateQuestionSingle(CreateQuestionSingleDto parameters)
    {
        var question = _mapper.Map<QuestionToeic>(parameters);
        question.CreationTime = DateTime.Now;

        await context.QuestionToeics.AddAsync(question);
        await context.SaveChangesAsync();

        foreach (var answerDto in parameters.Answers)
        {
            var answer = new AnswerToeic
            {
                IdQuestion = question.Id,
                IsBoolean = answerDto.IsBoolean,
                Content = answerDto.Content,
                STTAnswer = answerDto.STTAnswer,
                Transcription = answerDto.Transcription
            };

            await context.AnswerToeics.AddAsync(answer);
        }

        await context.SaveChangesAsync();

        return DataResult.ResultSuccess("Tạo câu hỏi đơn thành công");
    }

    public async Task<object> CreateQuestionGroup(CreateQuestionGroupDto parameters)
    {
        var groupQuestion = _mapper.Map<GroupQuestion>(parameters);
        groupQuestion.CreationTime = DateTime.Now;
        await context.GroupQuestions.AddAsync(groupQuestion);
        await context.SaveChangesAsync();

        foreach (var questionDto in parameters.Questions)
        {
            var question = new QuestionToeic
            {
                Content = questionDto.Content,
                NumberSTT = questionDto.NumberSTT,
                Index = questionDto.Index,
                Transcription = questionDto.Transcription,
                Type = questionDto.Type,
                PartId = parameters.PartId,
                IdGroupQuestion = groupQuestion.Id,
                CreationTime = DateTime.Now
            };
            await context.QuestionToeics.AddAsync(question);
            await context.SaveChangesAsync();

            foreach (var answerDto in questionDto.Answers)
            {
                var answer = new AnswerToeic
                {
                    IdQuestion = question.Id,
                    IsBoolean = answerDto.IsBoolean,
                    Content = answerDto.Content,
                    STTAnswer = answerDto.STTAnswer,
                    Transcription = answerDto.Transcription
                };

                await context.AnswerToeics.AddAsync(answer);
            }
        }

        await context.SaveChangesAsync();

        return DataResult.ResultSuccess("Tạo nhóm câu hỏi đơn thành công");
    }

    public async Task<object> GetListQuestionSingle(GetListQuestionSingleDto parameters)
    {
        try
        {
            var query = from question in context.QuestionToeics
                join answers in context.AnswerToeics on question.Id equals answers.IdQuestion into answersGroup
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
                    Answers = answersGroup.Select(a => new
                    {
                        Id = a.Id,
                        Content = a.Content,
                        IsBoolean = a.IsBoolean,
                        Transcription = a.Transcription
                    }).ToList()
                };
            query = query.Where(x => x.IdGroupQuestion == null);
            if (parameters.OrderBy.HasValue && parameters.OrderBy.Value)
            {
                query = query.OrderByDescending(x => x.CreationTime);
            }

            if (parameters.Type.HasValue)
            {
                query = query.Where(x => x.Type.Contains(parameters.Type.Value));
            }

            if (parameters.PartId.HasValue)
            {
                query = query.Where(x => x.PartId == parameters.PartId);
            }

            if (!string.IsNullOrEmpty(parameters.Keyword))
            {
                query = query.Where(x => x.Content.Contains(parameters.Keyword));
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

    public async Task<object> GetListQuestionGroup(GetListQuestionGroupDto parameters)
    {
        try
        {
            var query = from groupQuestion in context.GroupQuestions
                join question in context.QuestionToeics on groupQuestion.Id equals question.IdGroupQuestion into
                    manyQuestion
                select new
                {
                    Id = groupQuestion.Id,
                    Content = groupQuestion.Content,
                    PartId = groupQuestion.PartId,
                    ImageUrl = groupQuestion.ImageUrl,
                    AudioUrl = groupQuestion.AudioUrl,
                    IdExam = groupQuestion.IdExam,
                    CreationTime = groupQuestion.CreationTime,
                    Questions = manyQuestion.Select(q => new
                    {
                        Id = q.Id,
                        NumberSTT = q.NumberSTT,
                        Content = q.Content,
                        Type = q.Type,
                        Transcription = q.Transcription,
                        Answers = context.AnswerToeics.Where(a => a.IdQuestion == q.Id).ToList()
                    }).ToList()
                };
            if (parameters.PartId.HasValue)
            {
                query = query.Where(x => x.PartId == parameters.PartId);
            }

            if (!string.IsNullOrEmpty(parameters.Keyword))
            {
                query = query.Where(x => x.Content.Contains(parameters.Keyword));
            }

            if (parameters.OrderBy.HasValue && parameters.OrderBy.Value)
            {
                query = query.OrderByDescending(x => x.CreationTime != null)
                    .ThenByDescending(x => x.CreationTime);
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

    public Task<object> GetListQuestion(GetListQuestionDto parameters)
    {
        throw new NotImplementedException();
    }

    public async Task<object> ImportExcelQuestionSingle(ImportExcelDto input)
    {
        try
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            IFormFile file = input.File;
            string fileName = file.FileName;
            string fileExt = Path.GetExtension(fileName);
            if (fileExt != ".xlsx" && fileExt != ".xls")
            {
                return DataResult.ResultError("File not supported", "Error");
            }

            string filePath = Path.GetRandomFileName() + fileExt;
            using (FileStream stream = File.Create(filePath))
            {
                await file.CopyToAsync(stream);
                stream.Close();
            }

            var package = new ExcelPackage(new FileInfo(filePath));
            var worksheet = package.Workbook.Worksheets.First();
            int rowCount = worksheet.Dimension.End.Row;
            const int QUESTION_NUMBERSTT_INDEX = 1;
            const int QUESTION_CONTENT_INDEX = 2;
            const int QUESTION_PARTID_INDEX = 3;
            const int QUESTION_TYPE_INDEX = 4;
            const int QUESTION_IMAGEURL_INDEX = 5;
            const int QUESTION_AUDIOURL_INDEX = 6;
            const int QUESTION_TRANSCRIPTION_INDEX = 7;
            const int ANSWERS_CONTENT1_INDEX = 8;
            const int ANSWERS_ISBOOLEAN1_INDEX = 9;
            const int ANSWERS_CONTENT2_INDEX = 10;
            const int ANSWERS_ISBOOLEAN2_INDEX = 11;
            const int ANSWERS_CONTENT3_INDEX = 12;
            const int ANSWERS_ISBOOLEAN3_INDEX = 13;
            const int ANSWERS_CONTENT4_INDEX = 14;
            const int ANSWERS_ISBOOLEAN4_INDEX = 15;

            var questions = new List<CreateQuestionSingleDto>();

            for (var row = 2; row <= rowCount; row++)
            {
                object partIdValue = worksheet.Cells[row, QUESTION_PARTID_INDEX].Value;
                int? partIdInt = partIdValue != null ? Convert.ToInt32(partIdValue) : (int?)null;
                Do_An_Tot_Nghiep.Enums.Question.PART_TOEIC? PartId = partIdInt.HasValue
                    ? (Do_An_Tot_Nghiep.Enums.Question.PART_TOEIC?)Enum.Parse(
                        typeof(Do_An_Tot_Nghiep.Enums.Question.PART_TOEIC), partIdInt.Value.ToString())
                    : (Do_An_Tot_Nghiep.Enums.Question.PART_TOEIC?)null;
                string TypeString = worksheet.Cells[row, QUESTION_TYPE_INDEX].Text.Trim();
                List<int> TypeList = TypeString.Split(',')
                    .Select(s => int.Parse(s.Trim()))
                    .ToList();
                var content = worksheet.Cells[row, QUESTION_CONTENT_INDEX].Text.Trim();

                if (!PartId.HasValue || string.IsNullOrWhiteSpace(TypeString))
                {
                    continue;
                }

                bool questionExists =
                    await context.QuestionToeics.AnyAsync(q => q.Content == content && q.PartId == PartId);
                if (questionExists)
                {
                    continue;
                }

                var question = new QuestionToeic
                {
                    NumberSTT = Convert.ToInt32(worksheet.Cells[row, QUESTION_NUMBERSTT_INDEX].Value),
                    Content = content,
                    PartId = (Do_An_Tot_Nghiep.Enums.Question.PART_TOEIC?)Enum.Parse(
                        typeof(Do_An_Tot_Nghiep.Enums.Question.PART_TOEIC),
                        worksheet.Cells[row, QUESTION_PARTID_INDEX].Value.ToString()),
                    Type = TypeList,
                    ImageUrl = !string.IsNullOrWhiteSpace(worksheet.Cells[row, QUESTION_IMAGEURL_INDEX].Text?.Trim())
                        ? new[] { worksheet.Cells[row, QUESTION_IMAGEURL_INDEX].Text.Trim() }
                        : null,
                    AudioUrl = !string.IsNullOrWhiteSpace(worksheet.Cells[row, QUESTION_AUDIOURL_INDEX].Text?.Trim())
                        ? worksheet.Cells[row, QUESTION_AUDIOURL_INDEX].Text.Trim()
                        : null,
                    Transcription = worksheet.Cells[row, QUESTION_TRANSCRIPTION_INDEX].Text.Trim(),
                    CreationTime = DateTime.Now
                };

                await context.QuestionToeics.AddAsync(question);
                await context.SaveChangesAsync();

                for (int i = 0; i < 4; i++)
                {
                    var answerContentIndex = ANSWERS_CONTENT1_INDEX + i * 2;
                    var answerIsBooleanIndex = ANSWERS_ISBOOLEAN1_INDEX + i * 2;

                    var answerContent = worksheet.Cells[row, answerContentIndex].Text.Trim();
                    var answerIsBoolean = worksheet.Cells[row, answerIsBooleanIndex].Value;

                    if (!string.IsNullOrWhiteSpace(answerContent) || answerIsBoolean != null)
                    {
                        var answer = new AnswerToeic
                        {
                            Content = answerContent,
                            IsBoolean = answerIsBoolean != null && Convert.ToBoolean(answerIsBoolean),
                            IdQuestion = question.Id,
                        };
                        await context.AnswerToeics.AddAsync(answer);
                    }
                }

                await context.SaveChangesAsync();
            }

            File.Delete(filePath);

            return DataResult.ResultSuccess("Tạo câu hỏi đơn thành công");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<object> UpdateQuestionSingle(UpdateQuestionSingleDto input)
    {
        using (var transaction = context.Database.BeginTransaction())
        {
            try
            {
                var question = await context.QuestionToeics.FindAsync(input.Id);
                if (question == null)
                {
                    throw new Exception("Bad Request");
                }

                _mapper.Map(input, question);
                context.QuestionToeics.Update(question);
                foreach (var answerDto in input.Answers)
                {
                    var answers = await context.AnswerToeics.FindAsync(answerDto.Id);
                    answers.IsBoolean = answerDto.IsBoolean;
                    answers.Content = answerDto.Content;
                    context.AnswerToeics.Update(answers);
                }
                await context.SaveChangesAsync();
                transaction.Commit();
                return DataResult.ResultSuccess("Chỉnh sửa thành công");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }

    public async Task<object> DeleteQuestionSingle(int id)
    {
        using (var transaction = context.Database.BeginTransaction())
        {
            try
            {
                var question = await context.QuestionToeics.FindAsync(id);
                if (question == null)
                {
                    return DataResult.ResultFail("Không tìm thấy câu hỏi", (int)HttpStatusCode.NotFound);
                }

                context.QuestionToeics.Remove(question);
                var answers = context.AnswerToeics.Where(a => a.IdQuestion == id);
                context.AnswerToeics.RemoveRange(answers);
                await context.SaveChangesAsync();

                transaction.Commit();
                return DataResult.ResultSuccess(true, "");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }

    public async Task<object> GetQuestionUser(GetQuestionUserDto parameters)
    {
        try
        {
            if (parameters.PartId == PART_TOEIC.Part1 || parameters.PartId == PART_TOEIC.Part2 ||
                parameters.PartId == PART_TOEIC.Part5)
            {
                var query = from question in context.QuestionToeics
                    join answers in context.AnswerToeics on question.Id equals answers.IdQuestion into answersGroup
                    select new
                    {
                        Id = question.Id,
                        Content = question.Content,
                        PartId = question.PartId,
                        ImageUrl = question.ImageUrl,
                        AudioUrl = question.AudioUrl,
                        NumberSTT = question.NumberSTT,
                        Type = question.Type,
                        IdGroupQuestion = question.IdGroupQuestion,
                        Transcription = question.Transcription,
                        Answers = answersGroup.Select(a => new
                        {
                            Id = a.Id,
                            Content = a.Content,
                            IsBoolean = a.IsBoolean,
                            Transcription = a.Transcription
                        }).ToList(),
                    };

                if (parameters.Type.HasValue)
                {
                    query = query.Where(x => x.Type.Contains(parameters.Type.Value));
                }

                if (parameters.PartId.HasValue)
                {
                    query = query.Where(x => x.PartId == parameters.PartId);
                }

                if (!string.IsNullOrEmpty(parameters.Keyword))
                {
                    query = query.Where(x => x.Content.Contains(parameters.Keyword));
                }

                query = query.OrderBy(x => x.IdGroupQuestion).ThenBy((x) => Guid.NewGuid());
                var result = query.Take(parameters.MaxResultCount).ToList();
                return DataResult.ResultSuccess(result, "", result.Count);
            }

            if (parameters.PartId == PART_TOEIC.Part3 || parameters.PartId == PART_TOEIC.Part4 ||
                parameters.PartId == PART_TOEIC.Part6)
            {
                var queryGroup = from groupQ in context.GroupQuestions
                    select new
                    {
                        Id = groupQ.Id,
                        AudioUrl = groupQ.AudioUrl,
                        ImageUrl = groupQ.ImageUrl,
                        Content = groupQ.Content,
                        PartId = groupQ.PartId,
                        Transcription = groupQ.Transcription,
                        Questions = context.QuestionToeics.Where(q => q.IdGroupQuestion == groupQ.Id).Select(q => new
                        {
                            Id = q.Id,
                            Content = q.Content,
                            NumberSTT = q.NumberSTT,
                            Type = q.Type,
                            Transcription = q.Transcription,
                            Answers = context.AnswerToeics.Where(a => a.IdQuestion == q.Id).Select(a => new
                            {
                                Id = a.Id,
                                Content = a.Content,
                                IsBoolean = a.IsBoolean,
                                Transcription = a.Transcription
                            }).ToList()
                        }).ToList()
                    };

                if (parameters.PartId.HasValue)
                {
                    queryGroup = queryGroup.Where(x => x.PartId == parameters.PartId);
                }

                if (!string.IsNullOrEmpty(parameters.Keyword))
                {
                    queryGroup = queryGroup.Where(x => x.Content.Contains(parameters.Keyword));
                }

                queryGroup = queryGroup.OrderBy((x) => Guid.NewGuid());

                if (parameters.PartId == PART_TOEIC.Part3 || parameters.PartId == PART_TOEIC.Part4)
                {
                    var result = queryGroup.Take(parameters.MaxResultCount / 3).ToList();
                    return DataResult.ResultSuccess(result, "", result.Count);
                }

                if (parameters.PartId == PART_TOEIC.Part6)
                {
                    var result = queryGroup.Skip(parameters.SkipCount).Take(parameters.MaxResultCount / 4).ToList();
                    return DataResult.ResultSuccess(result, "", result.Count);
                }
            }

            if (parameters.PartId == PART_TOEIC.Part7)
            {
                var queryGroup = from groupQ in context.GroupQuestions
                    where groupQ.PartId == PART_TOEIC.Part7
                    select new
                    {
                        Id = groupQ.Id,
                        AudioUrl = groupQ.AudioUrl,
                        ImageUrl = groupQ.ImageUrl,
                        Content = groupQ.Content,
                        PartId = groupQ.PartId,
                        Transcription = groupQ.Transcription,
                        Questions = context.QuestionToeics
                            .Where(q => q.IdGroupQuestion == groupQ.Id)
                            .Select(q => new
                            {
                                Id = q.Id,
                                Content = q.Content,
                                NumberSTT = q.NumberSTT,
                                Type = q.Type,
                                Transcription = q.Transcription,
                                Answers = context.AnswerToeics
                                    .Where(a => a.IdQuestion == q.Id)
                                    .Select(a => new
                                    {
                                        Id = a.Id,
                                        Content = a.Content,
                                        IsBoolean = a.IsBoolean,
                                        Transcription = a.Transcription
                                    }).ToList()
                            }).ToList()
                    };

                // Áp dụng các điều kiện lọc nếu có
                if (!string.IsNullOrEmpty(parameters.Keyword))
                {
                    queryGroup = queryGroup.Where(x => x.Content.Contains(parameters.Keyword));
                }

                // Phân loại nhóm câu hỏi theo số lượng câu hỏi
                var groupsWith2Questions = queryGroup.Where(g => g.Questions.Count == 2).OrderBy(x => Guid.NewGuid())
                    .Take(4).ToList();
                var groupsWith3Questions = queryGroup.Where(g => g.Questions.Count == 3).OrderBy(x => Guid.NewGuid())
                    .Take(3).ToList();
                var groupsWith4Questions = queryGroup.Where(g => g.Questions.Count == 4).OrderBy(x => Guid.NewGuid())
                    .Take(3).ToList();
                var groupsWith5Questions = queryGroup.Where(g => g.Questions.Count == 5).OrderBy(x => Guid.NewGuid())
                    .Take(5).ToList();

                // Kết hợp tất cả các nhóm đã chọn
                var result = groupsWith2Questions.Concat(groupsWith3Questions)
                    .Concat(groupsWith4Questions)
                    .Concat(groupsWith5Questions)
                    .ToList();

                return DataResult.ResultSuccess(result, "", result.Count);
            }

            return DataResult.ResultSuccess("Thành công");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
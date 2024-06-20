using System.Security.Claims;
using System.Text.Json;
using Do_An_Tot_Nghiep.Dto.Result;
using Do_An_Tot_Nghiep.Dto.Statistics;
using Do_An_Tot_Nghiep.Enums.Question;
using Microsoft.EntityFrameworkCore;
using NewProject.Services.Common;
using Newtonsoft.Json;

namespace Do_An_Tot_Nghiep.Services.Upload;

public class StatisticsService : IStatisticsService
{
    private readonly IDbServices _dbService;
    private PublicContext context = new PublicContext();
    private readonly IHttpContextAccessor _httpContextAccessor;

    public StatisticsService(IDbServices dbService, IHttpContextAccessor httpContextAccessor)
    {
        _dbService = dbService;
        _httpContextAccessor = httpContextAccessor;
    }

    private static readonly string[] VietnameseMonths = new string[]
    {
        "Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4", "Tháng 5", "Tháng 6",
        "Tháng 7", "Tháng 8", "Tháng 9", "Tháng 10", "Tháng 11", "Tháng 12"
    };

    public async Task<object> StatisticsUser(int NumberRange)
    {
        return await GatherStatistics(context.Users, NumberRange);
    }

    public async Task<object> StatisticsPost(int NumberRange)
    {
        return await GatherStatistics(context.Posts, NumberRange);
    }

    public async Task<object> StatisticsQuestion(int NumberRange)
    {
        return await GatherStatistics(context.QuestionToeics, NumberRange);
    }

    public async Task<object> StatisticsCorrectQuestion()
    {
        try
        {
            var results = await context.Results.ToListAsync();
            int totalQuestions = 0;
            int correctQuestion = 0;
            int questionListening = 0;
            int questionReading = 0;

            foreach (var result in results)
            {
                if (!string.IsNullOrEmpty(result.Data))
                {
                    try
                    {
                        var resultData = JsonConvert.DeserializeObject<List<DataResultDto>>(result.Data);
                        if (resultData != null)
                        {
                            foreach (var question in resultData)
                            {
                                totalQuestions++;
                                if (question.PartId < (PART_TOEIC?)5)
                                {
                                    questionListening++;
                                    if (question.Answer != null)
                                    {
                                        if (question.Answer.IsBoolean)
                                        {
                                            correctQuestion++;
                                        }
                                    }
                                }
                                else
                                {
                                    questionReading++;
                                    if (question.Answer != null)
                                    {
                                        if (question.Answer.IsBoolean)
                                        {
                                            correctQuestion++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                }
            }

            var dataReturn = new
            {
                TotalQuestions = totalQuestions,
                CorrectAnswers = correctQuestion,
                QuestionListening = questionListening,
                QuestionReading = questionReading
            };
            return DataResult.ResultSuccess(dataReturn, "Thành công!");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<object> StatisticsOrdinal(GetOrdinalDto parameters)
    {
        try
        {
            var results = await context.Results
                .Where(r => r.UserId.HasValue && r.NumberCorrect.HasValue && !string.IsNullOrEmpty(r.Data))
                .Join(context.Users, 
                    result => result.UserId, 
                    user => user.Id, 
                    (result, user) => new { Result = result, User = user })
                .ToListAsync();

            var userStats = results
                .GroupBy(r => new { r.Result.UserId, r.User.Name, r.User.ImageUrl })  
                .Select(g => new
                {
                    UserId = g.Key.UserId,
                    UserName = g.Key.Name, 
                    Avatar = g.Key.ImageUrl,
                    TotalQuestionsAttempted = g.Sum(r => GetTotalQuestions(r.Result.Data)),
                    TotalCorrectAnswers = g.Sum(r => r.Result.NumberCorrect),
                    AccuracyRate = g.Sum(r => r.Result.NumberCorrect) * 1.0 / g.Sum(r => GetTotalQuestions(r.Result.Data))
                })
                .OrderByDescending(x => x.TotalCorrectAnswers)  
                .ThenByDescending(x => x.AccuracyRate)  
                .ToList();

            var result = userStats.Select((x, index) => new
            {
                Rank = index + 1,
                UserId = x.UserId,
                UserName = x.UserName,
                Avatar = x.Avatar,
                TotalQuestionsAttempted = x.TotalQuestionsAttempted,
                TotalCorrectAnswers = x.TotalCorrectAnswers,
                AccuracyRate = x.AccuracyRate
            });
            
            if (!string.IsNullOrEmpty(parameters.UserName))
            {
                result = result.Where(x => x.UserName != null && x.UserName.Contains(parameters.UserName));
            }
            
            var resultDetail = result.Skip(parameters.SkipCount).Take(parameters.MaxResultCount).ToList();
            return DataResult.ResultSuccess(resultDetail, "Thành công", resultDetail.Count());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<object> StatisticCorrectQuestionUser()
    {
        try
        {
            var userId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("Id"));
            var results = await context.Results.Where(a => a.UserId == userId).ToListAsync();
            int totalQuestions = 0;
            int correctQuestion = 0;
            int questionListening = 0;
            int questionReading = 0;
            foreach (var result in results)
            {
                if (!string.IsNullOrEmpty(result.Data))
                {
                    try
                    {
                        var resultData = JsonConvert.DeserializeObject<List<DataResultDto>>(result.Data);
                        if (resultData != null)
                        {
                            foreach (var question in resultData)
                            {
                                totalQuestions++;
                                if (question.PartId < (PART_TOEIC?)5)
                                {
                                    questionListening++;
                                    if (question.Answer != null)
                                    {
                                        if (question.Answer.IsBoolean)
                                        {
                                            correctQuestion++;
                                        }
                                    }
                                }
                                else
                                {
                                    questionReading++;
                                    if (question.Answer != null)
                                    {
                                        if (question.Answer.IsBoolean)
                                        {
                                            correctQuestion++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                }
            }

            var dataReturn = new
            {
                TotalQuestions = totalQuestions,
                CorrectAnswers = correctQuestion,
                QuestionListening = questionListening,
                QuestionReading = questionReading
            };
            return DataResult.ResultSuccess(dataReturn, "Thành công!");
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private int GetTotalQuestions(string data)
    {
        if (string.IsNullOrEmpty(data))
        {
            return 0;
        }
        try
        {
            using var jsonDocument = JsonDocument.Parse(data);
            var questionsArray = jsonDocument.RootElement;
            return questionsArray.GetArrayLength();
        }
        catch (Exception)
        {
            return 0;
        }
    }


    private async Task<object> GatherStatistics<T>(DbSet<T> dataSet, int numberRange) where T : class
    {
        try
        {
            DateTime now = DateTime.Now;
            int currentMonth = now.Month;
            int currentYear = now.Year;

            Dictionary<string, int> dataResult = new Dictionary<string, int>();

            if (currentMonth >= numberRange)
            {
                for (int index = currentMonth - numberRange + 1; index <= currentMonth; index++)
                {
                    int count = await dataSet
                        .CountAsync(x => EF.Property<DateTime?>(x, "CreationTime") != null &&
                                         EF.Property<DateTime?>(x, "CreationTime").Value.Month == index &&
                                         EF.Property<DateTime?>(x, "CreationTime").Value.Year == currentYear);
                    dataResult.Add(VietnameseMonths[index - 1], count);
                }
            }
            else
            {
                for (int index = 12 - (numberRange - currentMonth) + 1; index <= 12; index++)
                {
                    int count = await dataSet
                        .CountAsync(x => EF.Property<DateTime?>(x, "CreationTime") != null &&
                                         EF.Property<DateTime?>(x, "CreationTime").Value.Month == index &&
                                         EF.Property<DateTime?>(x, "CreationTime").Value.Year == currentYear - 1);
                    dataResult.Add(VietnameseMonths[index - 1], count);
                }

                for (int index = 1; index <= currentMonth; index++)
                {
                    int count = await dataSet
                        .CountAsync(x => EF.Property<DateTime?>(x, "CreationTime") != null &&
                                         EF.Property<DateTime?>(x, "CreationTime").Value.Month == index &&
                                         EF.Property<DateTime?>(x, "CreationTime").Value.Year == currentYear);
                    dataResult.Add(VietnameseMonths[index - 1], count);
                }
            }

            return DataResult.ResultSuccess(dataResult, "Thành công!");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
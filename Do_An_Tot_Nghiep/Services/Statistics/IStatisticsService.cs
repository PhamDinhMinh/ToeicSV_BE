using Do_An_Tot_Nghiep.Dto.Statistics;

namespace Do_An_Tot_Nghiep.Services.Upload;

public interface IStatisticsService
{
    Task<object> StatisticsUser(int NumberRange);
    Task<object> StatisticsQuestion(int NumberRange);
    Task<object> StatisticsPost(int NumberRange);
    Task<object> StatisticsCorrectQuestion();
    Task<object> StatisticsOrdinal(GetOrdinalDto parameters);
    Task<object> StatisticCorrectQuestionUser();
}
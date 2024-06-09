namespace Do_An_Tot_Nghiep.Services.Upload;

public interface IStatisticsService
{
    Task<object> StatisticsUser(int NumberRange);
    Task<object> StatisticsQuestion(int NumberRange);
    Task<object> StatisticsPost(int NumberRange);

    Task<object> StatisticsCorrectQuestion();
}
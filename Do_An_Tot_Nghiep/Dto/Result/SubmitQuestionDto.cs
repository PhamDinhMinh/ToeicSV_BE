namespace Do_An_Tot_Nghiep.Dto.Result;

public class SubmitQuestionDto
{
    public List<QuestionAnswerDto> ResultOfUser { get; set; }
    public DateTime? TimeStart { get; set; }
    public DateTime? TimeEnd { get; set; }
    public int? IdExam { get; set; }
}
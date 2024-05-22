namespace Do_An_Tot_Nghiep.Dto.Result;

public class SubmitQuestionDto
{
    public List<QuestionAnswerDto> resultOfUser { get; set; }
    public DateTime? TimeStart { get; set; }
    public DateTime? TimeEnd { get; set; }
    public int? IdExam { get; set; }
}
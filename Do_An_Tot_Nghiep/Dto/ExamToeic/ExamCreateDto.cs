namespace Do_An_Tot_Nghiep.Dto.ExamToeic;

public class ExamCreateDto
{
    public string NameExam { get; set; }
    public List<int>? ListQuestionPart1 { get; set; }
    public List<int>? ListQuestionPart2 { get; set; }
    public List<int>? ListQuestionPart3 { get; set; }
    public List<int>? ListQuestionPart4 { get; set; }
    public List<int>? ListQuestionPart5 { get; set; }
    public List<int>? ListQuestionPart6 { get; set; }
    public List<int>? ListQuestionPart7 { get; set; }
}
namespace Entities;

public class TestQuestionModel
{
    public Guid TestID { get; set; }
    public string Title { get; set; }
    public string Question { get; set; }
    public List<string> AnswerVariants { get; set; }
    public string Answer { get; set; }
}
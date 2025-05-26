using System.ComponentModel.DataAnnotations;

namespace Entities;

public class ExerciseSubmission
{
    [Key]
    public Guid SubmittedExerciseId { get; set; }
    
    public Guid ExerciseId { get; set; }

    public string ExerciseName { get; set; }
    public Guid StudentSubmittedId { get; set; }
    
    public DateTime SubmissionDate { get; set; }
    
    public string Status { get; set; }

    public double StarsReward { get; set; }
    public double MoneyReward { get; set; }
    public string? Feedback { get; set; }
}
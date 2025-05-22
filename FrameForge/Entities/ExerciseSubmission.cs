using System.ComponentModel.DataAnnotations;

namespace Entities;

public class ExerciseSubmission
{
    [Key]
    public Guid SubmittedExerciseId { get; set; }
    
    public Guid ExerciseId { get; set; }

    public string ExerciseName { get; set; }
    public Student StudentSubmitted { get; set; }
    
    public DateTime SubmissionDate { get; set; }
    
    public string Status { get; set; }
}
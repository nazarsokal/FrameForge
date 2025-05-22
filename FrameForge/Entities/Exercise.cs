using System.ComponentModel.DataAnnotations;

namespace Entities;

public class Exercise
{
    [Key]
    public Guid ExerciseId { get; set; }
    
    public Guid GroupId { get; set; }
    
    public string ExerciseName { get; set; }
    
    public string ExerciseDescription { get; set; }

    public string ExerciseRequirements { get; set; }
    
    public DateTime ExerciseStartDate { get; set; }

    public string ExererciseDifficulty { get; set; }

    public double MoneyReward { get; set; }
    
    public double StarsReward { get; set; }
    
}
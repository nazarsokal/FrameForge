using System.ComponentModel.DataAnnotations;

namespace Entities;

public class EnrolledLevels
{
    [Key]
    public Guid LevelsEnrolledKey { get; set; }
    public Guid StudentId { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string LevelTopicName { get; set; }

    [Required]
    public string State { get; set; }

    public double MoneyReward { get; set; }

    public int StarsReward { get; set; }
}
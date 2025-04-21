using System.ComponentModel.DataAnnotations;

namespace Entities;

public class MentorNpcQuotes
{
    [Key]
    public Guid QuoteId { get; set; }

    [Required]
    public int LevelNumber { get; set; }
    
    [Required]
    public int OrderNumber { get; set; }
    
    [Required]
    public string Quote { get; set; }
}
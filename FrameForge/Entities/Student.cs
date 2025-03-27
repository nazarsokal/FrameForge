using System.ComponentModel.DataAnnotations;

namespace Entities;

public class Student
{
    [Key]
    public Guid StudentId { get; set; }
    [Required]
    public string? Username { get; set; }
    [Required]
    public string? Email { get; set; }
    [Required]
    public string? Password { get; set; }
    [Required]
    public double? MoneyAmount { get; set; }
}
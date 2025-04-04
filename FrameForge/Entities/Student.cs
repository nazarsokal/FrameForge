using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Entities;

public class Student
{
    [Key]
    public Guid StudentId { get; set; }
    
    [Required]
    [JsonProperty("name")]
    [MaxLength(50)]
    public string? Username { get; set; }
    
    [Required] 
    [JsonProperty("email")]
    public string? Email { get; set; }
    public string? Password { get; set; }
    
    [JsonProperty("Id")]
    public string? GoogleId { get; set; }
    [Required]
    public double MoneyAmount { get; set; }
    
    [JsonProperty("picture")]
    public string? Picture { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is Student student)
        {
            return StudentId == student.StudentId;
        }
        
        return false;
    }
}
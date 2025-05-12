using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Entities;

public class Student : User
{
    public Guid? GroupId { get; set; }
    public Group? Group { get; set; }
    [Required]
    public double MoneyAmount { get; set; }
    public int StarsAmount { get; set; }
    
    public override bool Equals(object? obj)
    {
        if (obj is Student student)
        {
            return StudentId == student.StudentId;
        }
        
        return false;
    }
}
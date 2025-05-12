using System.ComponentModel.DataAnnotations;

namespace Entities;

public class Group
{
    [Key]
    public Guid Id { get; set; }

    public string GroupName { get; set; }

    public List<Student> Students { get; set; } = new();

    public Guid? TeacherId { get; set; }
    public Teacher? Teacher { get; set; }
}
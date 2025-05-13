namespace Entities;

public class RegisterViewModel
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public bool IsTeacher { get; set; }
}

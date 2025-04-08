using Entities;

namespace ServiceContracts;

public interface IRegistrationService
{
    public void RegisterStudent(Student student);
    
    public List<Student> GetStudents();

    public Student GetStudent(string username, string password);
    public Task<Student> RegisterStudentWithGoogle(Student? student);
}
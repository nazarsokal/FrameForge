using Entities;

namespace ServiceContracts;

public interface IRegistrationService
{
    public Task RegisterStudent(Student student);
    
    public Task<List<Student>> GetStudents();

    public Task<Student> GetStudent(string username, string password);
    public Task<Student> RegisterStudentWithGoogle(Student? student);
}
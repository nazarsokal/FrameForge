using Entities;

namespace ServiceContracts;

public interface IRegistrationService
{
    public Task RegisterStudent(Student student);
    
    public Task<List<User>> GetStudents();

    public Task<User> GetStudent(string username, string password);
    public Task<User> RegisterStudentWithGoogle(Student? student);
}
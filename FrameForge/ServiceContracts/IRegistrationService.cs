using Entities;

namespace ServiceContracts;

public interface IRegistrationService
{
    public Task RegisterUser(User user);
    
    public Task<List<User>> GetStudents();

    public Task<Student> GetStudent(string username, string password);
    public Task<Teacher> GetTeacher(string username, string password);
    public Task<User> RegisterStudentWithGoogle(User? user);
}
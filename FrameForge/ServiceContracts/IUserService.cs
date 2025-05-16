using Entities;

namespace ServiceContracts;

public interface IUserService
{
    public Task<object> GetUserById(Guid id);
    public Task<User> UpdateStudent(Student student);
    public User CreateStudent(Student student);
    public User DeleteStudent(int id);
}
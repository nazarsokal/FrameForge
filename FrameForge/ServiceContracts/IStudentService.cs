using Entities;

namespace ServiceContracts;

public interface IStudentService
{
    public Task<Student> GetStudentById(Guid id);
    public Student UpdateStudent(Student student);
    public Student CreateStudent(Student student);
    public Student DeleteStudent(int id);
}
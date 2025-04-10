using Entities;

namespace ServiceContracts;

public interface IStudentService
{
    public Student GetStudentById(int id);
    public Student UpdateStudent(Student student);
    public Student CreateStudent(Student student);
    public Student DeleteStudent(int id);
}
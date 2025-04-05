using Entities;

namespace ServiceContracts;

public interface IRegistrationService
{
    public void RegisterStudent(Student student);
    
    public List<Student> GetStudents();
    
    public Student RegisterStudentWithGoogle(Student? student);
}
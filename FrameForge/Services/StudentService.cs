using Entities;
using ServiceContracts;

namespace Services;

public class StudentService : IStudentService
{
    private readonly FrameForgeDbContext _context;

    public StudentService(FrameForgeDbContext context)
    {
        _context = context;
    }
    
    public Student GetStudentById(int id)
    {
        throw new NotImplementedException();
    }

    public Student UpdateStudent(Student student)
    {
        var studentFromDb = _context.Students.FirstOrDefault(st => st.StudentId == student.StudentId);
        
        if (studentFromDb == null) throw new NullReferenceException();
        
        studentFromDb.MoneyAmount = student.MoneyAmount;
        studentFromDb.StarsAmount = student.StarsAmount;
        _context.SaveChanges();
        
        return studentFromDb;
    }

    public Student CreateStudent(Student student)
    {
        throw new NotImplementedException();
    }

    public Student DeleteStudent(int id)
    {
        throw new NotImplementedException();
    }
}
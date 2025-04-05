using Entities;
using ServiceContracts;

namespace Services;

public class RegistrationService : IRegistrationService
{
    private readonly FrameForgeDbContext _dbContext;


    public RegistrationService(FrameForgeDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public void RegisterStudent(Student? student)
    {
        ArgumentNullException.ThrowIfNull(student);
        
        if (_dbContext.Students.Any(st => st.Email == student.Email && st.Username == student.Username))
            throw new InvalidOperationException("Student already exists");
        
        _dbContext.Students.Add(student);
        _dbContext.SaveChanges();
    }

    public Student GetStudent(string username,string password)
    {
        return _dbContext.Students.FirstOrDefault(x=>x.Username == username && x.Password == password);
    }
    public List<Student> GetStudents()
    {
        return _dbContext.Students.ToList();
    }

    public Student RegisterStudentWithGoogle(Student? student)
    {
        if (student == null) throw new NullReferenceException();

        if (CheckIfStudentExistsGoogle(student) == true)
        {
            var stFromDb = getStudentWithGoogle(student);
            return stFromDb;
        }
        
        student.StudentId = Guid.NewGuid();
        student.MoneyAmount = 10.0;
        
        _dbContext.Students.Add(student);
        _dbContext.SaveChanges();
        
        return student;
    }

    public bool CheckIfStudentExistsGoogle(Student? student)
    {
        if (student == null) throw new NullReferenceException();
        Student? stFromDb = _dbContext.Students.FirstOrDefault(st => st.GoogleId == student.GoogleId);
        
        if(stFromDb == null) return false;
        
        else return true;
    }

    private Student getStudentWithGoogle(Student student)
    {
        return _dbContext.Students.FirstOrDefault(st => st.GoogleId == student.GoogleId);
    }
}
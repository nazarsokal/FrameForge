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

        if (CheckIfStudentExistsGoogle(student) == true)
            return;
        else if (_dbContext.Students.Any(st => st.Email == student.Email && st.Username == student.Username))
            throw new InvalidOperationException("Student already exists");
        
        _dbContext.Students.Add(student);
        _dbContext.SaveChanges();
    }

    public List<Student> GetStudents()
    {
        return _dbContext.Students.ToList();
    }

    public bool CheckIfStudentExistsGoogle(Student student)
    {
        Student? stFromDb = _dbContext.Students.FirstOrDefault(st => st.GoogleId == student.GoogleId);
        
        if(stFromDb == null) return false;
        
        else return true;
    }
}
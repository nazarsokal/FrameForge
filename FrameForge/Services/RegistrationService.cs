using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;

namespace Services;

public class RegistrationService : IRegistrationService
{
    private readonly FrameForgeDbContext _dbContext;


    public RegistrationService(FrameForgeDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task RegisterStudent(Student? student)
    {
        ArgumentNullException.ThrowIfNull(student);
        
        if (_dbContext.Students.Any(st => st.Email == student.Email && st.Username == student.Username))
            throw new InvalidOperationException("Student already exists");
        
        _dbContext.Students.Add(student);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Student> GetStudent(string username,string password)
    {
        var user = await _dbContext.Students.SingleOrDefaultAsync(s => s.Username == username);
        if (user != null && PasswordHelper.VerifyPassword(password, user.Password))
        {
            SaveProfileImageAsync(user.Picture, user.StudentId);
            return user;
        }
        else
        {
            return null;
        }
    }
    public async Task<List<Student>> GetStudents()
    {
        return await _dbContext.Students.ToListAsync();
    }

    public async Task<Student> RegisterStudentWithGoogle(Student? student)
    {
        if (student == null) throw new NullReferenceException();

        if (await CheckIfStudentExistsGoogle(student) == true)
        {
            Student? stFromDb = await getStudentWithGoogle(student);
            await SaveProfileImageAsync(stFromDb.Picture, stFromDb.StudentId);
            return stFromDb;
        }
        
        student.StudentId = Guid.NewGuid();
        student.MoneyAmount = 10.0;
        
        _dbContext.Students.Add(student);
        await _dbContext.SaveChangesAsync();
        
        await SaveProfileImageAsync(student.Picture, student.StudentId);
        
        return student;
    }

    public async Task<bool> CheckIfStudentExistsGoogle(Student? student)
    {
        if (student == null) throw new NullReferenceException();
        Student? stFromDb = await _dbContext.Students.FirstOrDefaultAsync(st => st.GoogleId == student.GoogleId);
        
        if(stFromDb == null) return false;
        
        else return true;
    }

    private async Task<Student> getStudentWithGoogle(Student student)
    {
        return await _dbContext.Students.FirstOrDefaultAsync(st => st.GoogleId == student.GoogleId);
    }
    
    private async Task SaveProfileImageAsync(string imageUrl, Guid userId)
    {
        using var httpClient = new HttpClient();
        var imageBytes = await httpClient.GetByteArrayAsync(imageUrl);

        var filePath = Path.Combine("wwwroot/images/users", $"{userId}.jpg");
        await File.WriteAllBytesAsync(filePath, imageBytes);
    }
}
using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;

namespace Services;

public class StudentService : IStudentService
{
    private readonly FrameForgeDbContext _context;
    private readonly IAzureStorageService _azureStorageService = new AzureStorageService();
    public StudentService(FrameForgeDbContext context)
    {
        _context = context;
    }
    
    public async Task<Student> GetStudentById(Guid id)
    {
        Student? stById = await _context.Users.OfType<Student>().FirstOrDefaultAsync(s => s.UserId == id);
        if(stById == null) throw new NullReferenceException();
        
        return stById;
    }

    public async Task<Student> UpdateStudent(Student student)
    {
        var studentFromDb = _context.Users.OfType<Student>().FirstOrDefault(st => st.UserId == student.UserId);
        
        if (studentFromDb == null) throw new NullReferenceException();
        
        studentFromDb.MoneyAmount = student.MoneyAmount;
        studentFromDb.StarsAmount = student.StarsAmount;
        await _context.SaveChangesAsync();
        
        _context.Entry(student).State = EntityState.Detached;
        
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
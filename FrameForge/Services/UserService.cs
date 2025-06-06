using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;

namespace Services;

public class UserService : IUserService
{
    private readonly FrameForgeDbContext _context;
    private readonly IAzureStorageService _azureStorageService = new AzureStorageService();
    public UserService(FrameForgeDbContext context)
    {
        _context = context;
    }
    
    public async Task<object> GetUserById(Guid id)
    {
        object? stById = await _context.Users.OfType<object>().FirstOrDefaultAsync(u => ((User)u).UserId == id);
        if(stById == null) throw new NullReferenceException();
        
        return stById;
    }

    public async Task<object> GetUserByUserName(string userName) => await _context.Users.OfType<object>().FirstOrDefaultAsync(u => ((User)u).Username == userName);

    public async Task<User> UpdateStudent(Student student)
    {
        var studentFromDb = _context.Users.OfType<Student>().FirstOrDefault(st => st.UserId == student.UserId);
        
        if (studentFromDb == null) throw new NullReferenceException();
        
        studentFromDb.MoneyAmount = student.MoneyAmount;
        studentFromDb.StarsAmount = student.StarsAmount;
        await _context.SaveChangesAsync();
        
        _context.Entry(student).State = EntityState.Detached;
        
        return studentFromDb;
    }

    public User CreateStudent(Student student)
    {
        throw new NotImplementedException();
    }

    public User DeleteStudent(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Student>> GetStudentsFromGroup(Guid groupId)
    {
        return await _context.Users.OfType<Student>().Where(s => s.GroupId == groupId).ToListAsync();
    }
}
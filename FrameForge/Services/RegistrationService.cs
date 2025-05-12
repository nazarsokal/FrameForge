using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;

namespace Services;

public class RegistrationService : IRegistrationService
{
    private readonly FrameForgeDbContext _dbContext;
    private readonly IAzureStorageService _azureStorageService;


    public RegistrationService(FrameForgeDbContext dbContext, IAzureStorageService azureStorageService)
    {
        _dbContext = dbContext;
        _azureStorageService = azureStorageService;
    }
    
    public async Task RegisterStudent(Student? student)
    {
        ArgumentNullException.ThrowIfNull(student);
        
        if (_dbContext.Users.Any(st => st.Email == student.Email && st.Username == student.Username))
            throw new InvalidOperationException("Student already exists");
        
        student.StudentId = Guid.NewGuid();

        if (student.Picture == null)
        {
            var path = @"wwwroot/images/icons_mainPage/account.png";
            var pathInSt = await SaveDefaultProfileImageAsync(path, student.StudentId);
            student.Picture = pathInSt;
        }
        
        _dbContext.Users.Add(student);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<User> GetStudent(string username,string password)
    {
        var user = await _dbContext.Users.SingleOrDefaultAsync(s => s.Username == username);
        if (user != null && PasswordHelper.VerifyPassword(password, user.Password))
        {
            var studentImage = await _azureStorageService.GetUserPhoto(user.StudentId);
            user.Picture = Convert.ToBase64String(studentImage);
            return user;
        }
        else
        {
            return null;
        }
    }
    public async Task<List<User>> GetStudents()
    {
        return await _dbContext.Users.ToListAsync();
    }

    public async Task<User> RegisterStudentWithGoogle(Student? student)
    {
        if (student == null) throw new NullReferenceException();

        if (await CheckIfStudentExistsGoogle(student) == true)
        {
            User? stFromDb = await getStudentWithGoogle(student);
            
            // await SaveProfileImageAsync(stFromDb.Picture, stFromDb.StudentId);
            var studentImage = await _azureStorageService.GetUserPhoto(stFromDb.StudentId);
            stFromDb.Picture = Convert.ToBase64String(studentImage);
            
            return stFromDb;
        }
        
        student.StudentId = Guid.NewGuid();
        student.MoneyAmount = 10.0;

        var imagePath = await SaveProfileImageAsync(student.Picture, student.StudentId);
        student.Picture = imagePath; // Записується шлях до зображення

        _dbContext.Users.Add(student);
        await _dbContext.SaveChangesAsync(); // У БД записано imagePath

        var stImage = await _azureStorageService.GetUserPhoto(student.StudentId);
        student.Picture = Convert.ToBase64String(stImage); // Для повернення, не зберігається в БД

        return student;

    }

    public async Task<bool> CheckIfStudentExistsGoogle(Student? student)
    {
        if (student == null) throw new NullReferenceException();
        User? stFromDb = await _dbContext.Users.FirstOrDefaultAsync(st => st.GoogleId == student.GoogleId);
        
        if(stFromDb == null) return false;
        
        else return true;
    }

    private async Task<User> getStudentWithGoogle(Student student)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(st => st.GoogleId == student.GoogleId);
    }
    
    private async Task<string> SaveProfileImageAsync(string imageUrl, Guid userId)
    {
        using var httpClient = new HttpClient();
        var imageBytes = await httpClient.GetByteArrayAsync(imageUrl);
        
        return await _azureStorageService.UploadUserPhoto(imageBytes, userId);
    }

    private async Task<string> SaveDefaultProfileImageAsync(string path, Guid userId)
    {
        byte[] byteArray = File.ReadAllBytes(path);
        
        return await _azureStorageService.UploadUserPhoto(byteArray, userId);
    }
}
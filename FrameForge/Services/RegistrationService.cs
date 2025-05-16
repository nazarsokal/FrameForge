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
    
    public async Task RegisterUser(User? user)
    {
        ArgumentNullException.ThrowIfNull(user);
        
        if (_dbContext.Users.Any(st => st.Email == user.Email && st.Username == user.Username))
            throw new InvalidOperationException("User already exists");
        
        user.UserId = Guid.NewGuid();

        if (user.Picture == null)
        {
            var path = @"wwwroot/images/icons_mainPage/account.png";
            await SaveDefaultProfileImageAsync(path, user.UserId);
            
            var stImage = await _azureStorageService.GetUserPhoto(user.UserId);
            user.Picture = Convert.ToBase64String(stImage);
        }
        
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<User> GetStudent(string username,string password)
    {
        var user = await _dbContext.Users.OfType<Student>().SingleOrDefaultAsync(s => s.Username == username);
        if (user != null && PasswordHelper.VerifyPassword(password, user.Password))
        {
            var studentImage = await _azureStorageService.GetUserPhoto(user.UserId);
            user.Picture = Convert.ToBase64String(studentImage);
            return user;
        }
        else
        {
            return null;
        }
    }

    public async Task<Teacher> GetTeacher(string username, string password)
    {
        var user = await _dbContext.Users.OfType<Teacher>().SingleOrDefaultAsync(s => s.Username == username);
        if (user != null && PasswordHelper.VerifyPassword(password, user.Password))
        {
            var studentImage = await _azureStorageService.GetUserPhoto(user.UserId);
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

    public async Task<User> RegisterStudentWithGoogle(User? user)
    {
        if (user == null) throw new NullReferenceException();

        if (await CheckIfStudentExistsGoogle(user) == true)
        {
            User? stFromDb = await getStudentWithGoogle(user);
            
            // await SaveProfileImageAsync(stFromDb.Picture, stFromDb.StudentId);
            var studentImage = await _azureStorageService.GetUserPhoto(stFromDb.UserId);
            stFromDb.Picture = Convert.ToBase64String(studentImage);
            
            return stFromDb;
        }

        user.UserId = Guid.NewGuid();
        
        if (user is Student student)
        {
            student.MoneyAmount = 10.0;
        }
        

        var imagePath = await SaveProfileImageAsync(user.Picture, user.UserId);
        user.Picture = imagePath; // Записується шлях до зображення


        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync(); // У БД записано imagePath

        var stImage = await _azureStorageService.GetUserPhoto(user.UserId);
        user.Picture = Convert.ToBase64String(stImage); // Для повернення, не зберігається в БД

        return user;

    }

    public async Task<bool> CheckIfStudentExistsGoogle(User? user)
    {
        if (user == null) throw new NullReferenceException();
        User? stFromDb = await _dbContext.Users.FirstOrDefaultAsync(st => st.GoogleId == user.GoogleId);
        
        if(stFromDb == null) return false;
        
        else return true;
    }

    private async Task<User> getStudentWithGoogle(User User)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(st => st.GoogleId == User.GoogleId);
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
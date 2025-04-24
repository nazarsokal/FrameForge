namespace ServiceContracts;

public interface IAzureStorageService
{
    public Task UploadUserPhoto(string photoUrl, Guid userId);
    
    public Task<string> GetUserPhoto(Guid userId);
    
}
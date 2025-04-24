namespace ServiceContracts;

public interface IAzureStorageService
{
    public Task UploadUserPhoto(byte[] photoUrl, Guid userId);
    
    public Task<string> GetUserPhoto(Guid userId);
    
}
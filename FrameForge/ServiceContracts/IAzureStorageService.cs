using Entities;
using ServiceContracts.Enums;

namespace ServiceContracts;

public interface IAzureStorageService
{
    public Task<string> UploadUserPhoto(byte[] photoUrl, Guid userId);
    
    public Task<byte[]> GetUserPhoto(Guid userId);
    public Task<List<Algorithm>?> DownloadAllAlgorithms();
}
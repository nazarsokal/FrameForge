using Azure.Storage.Files.Shares;
using ServiceContracts;

namespace Services;

public class AzureStorageService : IAzureStorageService
{
    private const string SasToken =
        "BlobEndpoint=https://frameforge.blob.core.windows.net/;QueueEndpoint=https://frameforge.queue.core.windows.net/;FileEndpoint=https://frameforge.file.core.windows.net/;TableEndpoint=https://frameforge.table.core.windows.net/;SharedAccessSignature=sv=2024-11-04&ss=bfqt&srt=sco&sp=rwdlacupiytfx&se=2025-05-31T17:11:47Z&st=2025-04-24T09:11:47Z&spr=https,http&sig=K4KyYjzpW7jCTRrDvYoM%2FZksW58hIL3wGUzuWw1Zc70%3D";

    private readonly ShareClient _shareClient;

    public AzureStorageService()
    {
        // Initialize ShareClient using the SAS token connection string
        _shareClient = new ShareClient(SasToken, "frameforgefilestorage");
    }

    public async Task UploadUserPhoto(string photoPath, Guid userId)
    {
        try
        {
            ShareDirectoryClient rootDir = _shareClient.GetRootDirectoryClient();

            ShareDirectoryClient folder = rootDir.GetSubdirectoryClient("UserImages");

            string targetFileName = $"{userId}.jpg";
            
            ShareFileClient fileClient = folder.GetFileClient(targetFileName);
            
            FileInfo fileInfo = new FileInfo(photoPath);
            
            await fileClient.CreateAsync(fileInfo.Length);
            
            using FileStream stream = File.OpenRead(photoPath);
            await fileClient.UploadRangeAsync(
                new Azure.HttpRange(0, fileInfo.Length),
                stream
            );
        }
        catch (Exception ex)
        {
            // Log the exception for debugging
            Console.WriteLine($"Error uploading photo: {ex.Message}");
            throw; // Re-throw the exception to handle it upstream if needed
        }
    }

    public Task<string> GetUserPhoto(Guid userId)
    {
        throw new NotImplementedException();
    }
}
using Azure.Storage.Files.Shares;
using ServiceContracts;
using System.IO;
using System.Threading.Tasks;

namespace Services;

public class AzureStorageService : IAzureStorageService
{
    private const string SasToken =
        "BlobEndpoint=https://frameforge.blob.core.windows.net/;QueueEndpoint=https://frameforge.queue.core.windows.net/;FileEndpoint=https://frameforge.file.core.windows.net/;TableEndpoint=https://frameforge.table.core.windows.net/;SharedAccessSignature=sv=2024-11-04&ss=bfqt&srt=sco&sp=rwdlacupiytfx&se=2025-05-31T17:11:47Z&st=2025-04-24T09:11:47Z&spr=https,http&sig=K4KyYjzpW7jCTRrDvYoM%2FZksW58hIL3wGUzuWw1Zc70%3D";

    private readonly ShareClient _shareClient;

    public AzureStorageService()
    {
        _shareClient = new ShareClient(SasToken, "frameforgefilestorage");
    }

    public async Task UploadUserPhoto(byte[] photoData, Guid userId)
    {
        try
        {
            if (photoData == null || photoData.Length == 0)
            {
                throw new ArgumentException("Photo data cannot be null or empty.", nameof(photoData));
            }

            ShareDirectoryClient rootDir = _shareClient.GetRootDirectoryClient();
            ShareDirectoryClient folder = rootDir.GetSubdirectoryClient("UserImages");
            await folder.CreateIfNotExistsAsync();
            string targetFileName = $"{userId}.jpg";
            ShareFileClient fileClient = folder.GetFileClient(targetFileName);
            await fileClient.CreateAsync(photoData.Length);
            using MemoryStream stream = new MemoryStream(photoData);
            await fileClient.UploadRangeAsync(
                new Azure.HttpRange(0, photoData.Length),
                stream
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error uploading photo: {ex.Message}");
            throw;
        }
    }

    public Task<string> GetUserPhoto(Guid userId)
    {
        throw new NotImplementedException();
    }
}
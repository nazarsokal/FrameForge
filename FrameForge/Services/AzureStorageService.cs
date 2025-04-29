using Azure.Storage.Files.Shares;
using ServiceContracts;
using System.IO;
using System.Threading.Tasks;
using ServiceContracts.Enums;

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

    public async Task<string> UploadUserPhoto(byte[] photoData, Guid userId)
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
            
            return targetFileName;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error uploading photo: {ex.Message}");
            throw;
        }
    }

    public async Task<byte[]> GetUserPhoto(Guid userId)
    {
        try
        {
            ShareDirectoryClient rootDir = _shareClient.GetRootDirectoryClient();
            ShareDirectoryClient folder = rootDir.GetSubdirectoryClient("UserImages");
            string targetFileName = $"{userId}.jpg";
            ShareFileClient fileClient = folder.GetFileClient(targetFileName);

            bool fileExists = await fileClient.ExistsAsync();
            if (!fileExists)
            {
                throw new FileNotFoundException($"Image for user {userId} not found.");
            }

            var response = await fileClient.DownloadAsync();
            using Stream contentStream = response.Value.Content;
            using MemoryStream memoryStream = new MemoryStream();
            await contentStream.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving photo: {ex.Message}");
            throw;
        }
    }

    public async Task<Dictionary<FileExtensions, string>> DownloadAlgorithm(string algorithmName)
    {
        Dictionary<FileExtensions, string> algorithmFiles = new Dictionary<FileExtensions, string>();
        ShareDirectoryClient rootDir = _shareClient.GetRootDirectoryClient();
        ShareDirectoryClient folder = rootDir.GetSubdirectoryClient($"Algorithms/{algorithmName}");
        var list = new List<string>() {"index.html", "style.css", "script.js"};
        foreach (var file in list)
        {
            ShareFileClient fileClient = folder.GetFileClient(file);
            var download = await fileClient.DownloadAsync();
            using var reader = new StreamReader(download.Value.Content);
            string content = await reader.ReadToEndAsync();
            string ext = Path.GetExtension(file).TrimStart('.');

            if (Enum.TryParse<FileExtensions>(ext, true, out var fileExtension))
            {
                algorithmFiles.Add(fileExtension, content);
            }

        }
        
        return algorithmFiles;
    }
}
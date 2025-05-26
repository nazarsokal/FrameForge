using Azure.Storage.Files.Shares;
using ServiceContracts;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Azure;
using Entities;
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

    public async Task UploadCode(List<string> code, Guid taskId, Guid userId)
    {
        string fileName = "";
        ShareDirectoryClient rootDir = _shareClient.GetRootDirectoryClient();
        ShareDirectoryClient folder = rootDir.GetSubdirectoryClient("UserTasks");
        
        ShareDirectoryClient userDir = folder.GetSubdirectoryClient($"{taskId.ToString()}_{userId.ToString()}");
        await userDir.CreateIfNotExistsAsync();

        for (int i = 0; i < code.Count; i++)
        {
            byte[] fileBytes = Encoding.UTF8.GetBytes(code[0]);
            
            if(i == 0) fileName = "index.html";
            else if (i == 1) fileName = "style.css";
            else if (i == 2) fileName = "script.js";
            
            ShareFileClient fileClient = userDir.GetFileClient(fileName);
            await fileClient.CreateAsync(fileBytes.Length);

            using MemoryStream stream = new MemoryStream(fileBytes);
            await fileClient.UploadRangeAsync(
                new HttpRange(0, fileBytes.Length),
                stream
            );
        }
    }

    public async Task<ExerciseRequest> GetSubmittedTasks(Guid exerciseSubmissionId, Guid userId)
    {
        string fileName = "";
        ShareDirectoryClient rootDir = _shareClient.GetRootDirectoryClient();
        ShareDirectoryClient folder = rootDir.GetSubdirectoryClient("UserTasks");
        ExerciseRequest exerciseRequest = new ExerciseRequest();
        
        ShareDirectoryClient userDir = folder.GetSubdirectoryClient($"{exerciseSubmissionId.ToString()}_{userId.ToString()}");
        var list = new List<string>() {"index.html", "style.css", "script.js"};
        int i = 0;
        foreach (var file in list)
        {
            ShareFileClient fileClient = userDir.GetFileClient(file);
            var download = await fileClient.DownloadAsync();
            using var reader = new StreamReader(download.Value.Content);
            string content = await reader.ReadToEndAsync();
            
            if(i == 0)
            {
                exerciseRequest.Files[i].Name = "index.html";
                exerciseRequest.Files[i].Content = content;
            }
            else if (i == 1)
            {
                exerciseRequest.Files[i].Name = "style.css";
                exerciseRequest.Files[i].Content = content;
            }
            else if (i == 2)
            {
                exerciseRequest.Files[i].Name = "script.js";
                exerciseRequest.Files[i].Content = content;
            }

            i++;
        }
        
        return exerciseRequest;
    }
}
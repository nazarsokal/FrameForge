using Azure.Storage.Files.Shares;
using ServiceContracts;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Files.Shares.Models;
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

    public async Task<List<Algorithm>?> DownloadAllAlgorithms()
    {
        List<Algorithm>? allAlgorithms = new();
        ShareDirectoryClient rootDir = _shareClient.GetRootDirectoryClient();
        ShareDirectoryClient algorithmsDir = rootDir.GetSubdirectoryClient("Algorithms");

        await foreach (ShareFileItem item in algorithmsDir.GetFilesAndDirectoriesAsync())
        {
            if (!item.IsDirectory) continue;

            string algorithmName = item.Name;
            ShareDirectoryClient algorithmFolder = algorithmsDir.GetSubdirectoryClient(algorithmName);
            Algorithm algorithm = new() { AlgorithmName = algorithmName };

            // Define file mapping
            var fileMapping = new Dictionary<string, Action<string>>
            {
                { "index.html", content => algorithm.AlgorithmHtml = content },
                { "style.css",  content => algorithm.AlgorithmCss  = content },
                { "script.js",  content => algorithm.AlgorithmJs  = content },
            };

            foreach (var file in fileMapping.Keys)
            {
                ShareFileClient fileClient = algorithmFolder.GetFileClient(file);
                if (!await fileClient.ExistsAsync()) continue;

                var download = await fileClient.DownloadAsync();
                using var reader = new StreamReader(download.Value.Content);
                string content = await reader.ReadToEndAsync();

                fileMapping[file](content);
            }

            allAlgorithms.Add(algorithm);
        }

        return allAlgorithms;
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
            byte[] fileBytes = Encoding.UTF8.GetBytes(code[i]);
            
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

    public async Task<ExerciseRequest> GetSubmittedTasks(Guid taskId, Guid userId)
    {
        // Prepare the directory client for UserTasks/{taskId}_{userId}
        ShareDirectoryClient rootDir   = _shareClient.GetRootDirectoryClient();
        ShareDirectoryClient folder    = rootDir.GetSubdirectoryClient("UserTasks");
        string                   dirName = $"{taskId}_{userId}";
        ShareDirectoryClient userDir   = folder.GetSubdirectoryClient(dirName);

        // DTO to return
        var exerciseRequest = new ExerciseRequest
        {
            Files = new List<ExerciseFile>()  // make sure this property is a List<ExerciseFile>
        };

        // The exact same file names you used when uploading
        var fileNames = new[] { "index.html", "style.css", "script.js" };

        foreach (var name in fileNames)
        {
            ShareFileClient fileClient = userDir.GetFileClient(name);

            // If the file doesn't exist, skip it (optional)
            if (!await fileClient.ExistsAsync())
                continue;

            // Download and read the entire file
            var download = await fileClient.DownloadAsync();
            using var reader = new StreamReader(download.Value.Content, Encoding.UTF8);
            string content = await reader.ReadToEndAsync();

            // Add to your DTO
            exerciseRequest.Files.Add(new ExerciseFile
            {
                Name    = name,
                Content = content
            });
        }

        return exerciseRequest;
    }
}
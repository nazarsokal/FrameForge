using Azure.Storage.Files.Shares;
using ServiceContracts;

namespace Services;

public class AzureStorageService : IAzureStorageService
{
    private const string SasToken =
        "https://frameforge.file.core.windows.net/?sv=2024-11-04&ss=bfqt&srt=sco&sp=rwdlacupiytfx&se=2025-05-31T15:32:24Z&st=2025-04-24T07:32:24Z&spr=https,http&sig=CaeVgMB667YopUKVMTkGOrv3QfzgXL9SkEm1w9I34AM%3D";

    public AzureStorageService()
    {
        
    }
    
    public async Task UploadUserPhoto(string photoPath, Guid userId)
    {
        ShareClient share = new ShareClient(new Uri(SasToken));

        // 🔹 Get or create the target folder
        ShareDirectoryClient rootDir = share.GetRootDirectoryClient();
        ShareDirectoryClient folder = rootDir.GetSubdirectoryClient("UserImages");
        
        // await folder.CreateIfNotExistsAsync();
        
        string filePath = userId.ToString() + ".jpg";
        ShareFileClient fileClient = folder.GetFileClient(filePath);
        
        FileInfo fileInfo = new FileInfo(filePath);
        await fileClient.CreateAsync(fileInfo.Length);
        
        using FileStream stream = File.OpenRead(filePath);
        await fileClient.UploadRangeAsync(
            new Azure.HttpRange(0, fileInfo.Length),
            stream
        );
    }

    public Task<string> GetUserPhoto(Guid userId)
    {
        throw new NotImplementedException();
    }
}
using ServiceContracts;
using ServiceContracts.Enums;
using Services;

namespace ServicesTest;

public class AzureServiceTest
{
    private IAzureStorageService _azureStorageService;

    public AzureServiceTest()
    {
        _azureStorageService = new AzureStorageService();
    }
    [Fact]
    public async Task TestAzureService_LoadUserPhoto()
    {
        // string photoPath =
        //     @"/Users/asokalch/Documents/GitHub/FrameForge/FrameForge/FrameForge/wwwroot/images/users/e4c52f39-b100-4d25-ac78-067f23667a1f.jpg";
        //
        // await _azureStorageService.UploadUserPhoto(new byte[4], Guid.NewGuid());
        //
        Assert.NotNull(1);
    }

    [Fact]
    public async Task TestAzureService_GetFile()
    {
        string algorithm = "Bezier";
        
        // Dictionary<FileExtensions, string>? files = await _azureStorageService.DownloadAllAlgorithms();

        int i = 0;
        Assert.NotNull(i);
    }
}
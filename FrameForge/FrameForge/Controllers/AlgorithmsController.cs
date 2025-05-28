using System.Text.Json;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ServiceContracts;

namespace FrameForge.Controllers;

public class AlgorithmsController : Controller
{
    private readonly IAzureStorageService _azureStorageService;
    private readonly IMemoryCache _memoryCache;
    private readonly IAlgorithmsService _algorithmsService;
    private List<Algorithm>? _algorithms;
    private User student;

    public AlgorithmsController(IAzureStorageService azureStorageService, IMemoryCache memoryCache, IAlgorithmsService algorithmsService)
    {
        _azureStorageService = azureStorageService;
        _memoryCache = memoryCache;
        _algorithmsService = algorithmsService;
    }
    
    [Route("[action]")]
    public async Task<IActionResult> AlgorithmsOverview()
    {
        var algorithms = await GetAlgorithmsAsync();
        ViewBag.Algorithms = algorithms;
        student = GetUserFromSession();
        return View("Algorithms", student);
    }

    [HttpGet]
    [Route("[action]")]
    public async Task<IActionResult> TestAlgorithm(string algorithmName)
    {
        var algorithms = await GetAlgorithmsAsync();
        if (algorithms == null) return NotFound();
        
        var algorithm = algorithms.FirstOrDefault(p => p.AlgorithmName == algorithmName);
        if (algorithm == null) return NotFound();

        return View(algorithm);
    }
    
    private async Task<List<Algorithm>?> GetAlgorithmsAsync()
    {
        return await _memoryCache.GetOrCreateAsync("AlgorithmsCache", async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
            var algsFromAzure =  await _azureStorageService.DownloadAllAlgorithms();
            return await _algorithmsService.GetAlgorithms(algsFromAzure);   
        });
    }
    
    private User GetUserFromSession()
    {
        User user = new User();
        var userType = HttpContext.Session.GetString("UserType");
        if (userType == "Teacher")
        {
            var userJson = HttpContext.Session.GetString("Teacher");
            Teacher teacher = JsonSerializer.Deserialize<Teacher>(userJson);
            return teacher;
        }
        else
        {
            var userJson = HttpContext.Session.GetString("Student");
            Student st = JsonSerializer.Deserialize<Student>(userJson);
            return st;
        }

        return user;
    }
}
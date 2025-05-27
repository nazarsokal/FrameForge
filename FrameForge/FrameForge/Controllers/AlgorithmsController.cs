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
    private Student student;

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
        student = GetStudentFromSession();
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
    
    private Student GetStudentFromSession()
    {
        var studentJson = HttpContext.Session.GetString("Student");
        if (studentJson != null) student = JsonSerializer.Deserialize<Student>(studentJson);
        if(student == null) throw new NullReferenceException("Student is null");
        
        return student;
    }
}
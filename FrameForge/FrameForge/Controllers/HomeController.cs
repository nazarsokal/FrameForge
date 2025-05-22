using System.Text.Json;
using Entities;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts;

namespace FrameForge.Controllers;

public class HomeController : Controller
{
    private readonly IStudentService _studentService;

    public HomeController(IStudentService studentService)
    {
        _studentService = studentService;
    }
    [Route("/")]
    public async Task<IActionResult> Index()
    {
        var studentJson = HttpContext.Session.GetString("Student");
        if (studentJson != null)
        {
            var student = JsonSerializer.Deserialize<Student>(studentJson);
            // Use your student object!
            return View(student);
        }
        else
        {
            Student student = await _studentService.GetStudentById(Guid.Parse("131c8f25-d521-4c55-b4f6-c45e7cfaf164"));
            string userString = JsonSerializer.Serialize(student);
            HttpContext.Session.SetString("Student", userString);
            return View();
        }
        
    }
}
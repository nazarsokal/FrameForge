using System.Text.Json;
using Entities;
using Microsoft.AspNetCore.Mvc;

namespace FrameForge.Controllers;

public class HomeController : Controller
{
    [Route("/")]
    public IActionResult Index()
    {
        var studentJson = HttpContext.Session.GetString("Student");
        if (studentJson != null)
        {
            var student = JsonSerializer.Deserialize<Student>(studentJson);
            // Use your student object!
            return View(student);
        }
        
        return View();
    }
}
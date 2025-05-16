using System.Text.Json;
using Entities;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts;

namespace FrameForge.Controllers;

public class HomeController : Controller
{
    private readonly IUserService _userService;

    public HomeController(IUserService userService)
    {
        _userService = userService;
    }
    [Route("/")]
    public async Task<IActionResult> Index()
    {
        User user = new User();
        var userType = HttpContext.Session.GetString("UserType");
        if (userType != null)
        {
            if (userType == "Student")
            {
                var userJson = HttpContext.Session.GetString("Student");
                user = JsonSerializer.Deserialize<Student>(userJson);
            }
            else if(userType == "Teacher")
            {
                var userJson = HttpContext.Session.GetString("Teacher");
                user = JsonSerializer.Deserialize<Teacher>(userJson);
            }
            // Use your student object!
            return View(user);
        }
        
        return View();
    }
}
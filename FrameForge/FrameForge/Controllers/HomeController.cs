using Entities;
using Microsoft.AspNetCore.Mvc;

namespace FrameForge.Controllers;

public class HomeController : Controller
{
    [Route("/")]
    public IActionResult Index(Student student)
    {
        return View(student);
    }
}
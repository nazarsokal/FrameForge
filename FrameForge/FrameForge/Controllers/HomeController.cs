using Microsoft.AspNetCore.Mvc;

namespace FrameForge.Controllers;

public class HomeController : Controller
{
    [Route("/")]
    public IActionResult Index()
    {
        return View();
    }
}
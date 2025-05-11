using Microsoft.AspNetCore.Mvc;

namespace FrameForge.Controllers;

public class AlgorithmsController : Controller
{
    public AlgorithmsController()
    {
        
    }
    [Route("[action]")]
    public IActionResult Algorithms()
    {
        return View();
    }
}
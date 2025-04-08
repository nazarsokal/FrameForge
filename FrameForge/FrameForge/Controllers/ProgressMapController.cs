using Microsoft.AspNetCore.Mvc;

namespace FrameForge.Controllers;

public class ProgressMapController : Controller
{
    [Route("[action]")]
    public IActionResult Map()
    {
        return View();
    }

    [Route("[action]{levelName}")]
    public IActionResult ViewLevel(string levelName)
    {
        return View(levelName);
    }
}
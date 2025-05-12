using Microsoft.AspNetCore.Mvc;

namespace FrameForge.Controllers;

public class BattleController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}
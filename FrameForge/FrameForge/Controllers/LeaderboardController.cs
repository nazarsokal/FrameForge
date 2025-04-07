using Microsoft.AspNetCore.Mvc;

namespace FrameForge.Controllers
{
    public class LeaderboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using Services;

namespace FrameForge.Controllers
{
    public class LeaderboardController : Controller
    {
        LeaderboardService _leaderboardService;
        public LeaderboardController(LeaderboardService leaderboardService)
        {
            _leaderboardService = leaderboardService;
        }
        [Route("[action]")]
        public IActionResult Leaderboard()
        {
            return View(_leaderboardService.GetAllSorted());
        }
    }
}

using System.Text.Json;
using Entities;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using Services;

namespace FrameForge.Controllers
{
    public class LeaderboardController : Controller
    {
        LeaderboardService _leaderboardService;
        User _student;
        public LeaderboardController(LeaderboardService leaderboardService)
        {
            _leaderboardService = leaderboardService;
        }
        [Route("[action]")]
        public IActionResult Leaderboard()
        {
            ViewBag.StudentsLists = _leaderboardService.GetAllSorted();
            _student = GetUserFromSession();
            return View(_student);
        }

        private User GetUserFromSession()
        {
            User user = new User();
            var userType = HttpContext.Session.GetString("UserType");
            if (userType == "Teacher")
            {
                var userJson = HttpContext.Session.GetString("Teacher");
                Teacher teacher = JsonSerializer.Deserialize<Teacher>(userJson);
                return teacher;
            }
            else
            {
                var userJson = HttpContext.Session.GetString("Student");
                Student st = JsonSerializer.Deserialize<Student>(userJson);
                return st;
            }

            return user;
        }
    }
}

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
        Student _student;
        public LeaderboardController(LeaderboardService leaderboardService)
        {
            _leaderboardService = leaderboardService;
        }
        [Route("[action]")]
        public IActionResult Leaderboard()
        {
            ViewBag.StudentsLists = _leaderboardService.GetAllSorted();
            _student = GetStudentFromSession();
            return View(_student);
        }
        
        private Student GetStudentFromSession()
        {
            var studentJson = HttpContext.Session.GetString("Student");
            if (studentJson != null) _student = JsonSerializer.Deserialize<Student>(studentJson);
            if(_student == null) throw new NullReferenceException("Student is null");
        
            return _student;
        }
    }
}

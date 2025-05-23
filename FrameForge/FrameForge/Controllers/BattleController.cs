using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using System.Text.Json;
using Entities;


namespace FrameForge.Controllers;

[Route("[controller]")]
public class BattleController : Controller
{
    private readonly IBattleService _battleService;
    private readonly IStudentService _studentService;

    public BattleController(IBattleService battleService, IStudentService studentService)
    {
        _battleService = battleService;
        _studentService = studentService;
    }

    [HttpGet]
    [Route("[action]")]
    public IActionResult BattleLobby()
    {
        return View();
    }
    [HttpGet]
    [Route("[action]")]
    public IActionResult WaitingForOpponent()
    {
        return View("WaitingPage");
    }
    
    [HttpPost]
    [Route("[action]")]
    public IActionResult ExecuteBattle(BattleRoom room)
    {
        ViewBag.Room = room;
        return View("BattleRoom", room);
    }

    // [HttpPost]
    // [Route("[action]")]
    // public async Task<IActionResult> CreateBattle()
    // {
    //     var studentString = HttpContext.Session.GetString("Student");
    //     if (string.IsNullOrEmpty(studentString))
    //         return RedirectToAction("Login", "Account");
    //
    //     var student = JsonConvert.DeserializeObject<Student>(studentString);
    //     var room = await _battleService.CreateRoom(student);
    //     
    //     HttpContext.Session.SetString("BattleRoom", JsonConvert.SerializeObject(room));
    //     return RedirectToAction("BattleRoom");
    // }

//     [HttpPost]
//     [Route("[action]")]
//     public async Task<IActionResult> JoinBattle(Guid roomId)
//     {
//         var studentString = HttpContext.Session.GetString("Student");
//         if (string.IsNullOrEmpty(studentString))
//             return RedirectToAction("Login", "Account");
//
//         var student = JsonConvert.DeserializeObject<Student>(studentString);
//         var room = await _battleService.JoinRoom(roomId, student);
//         
//         HttpContext.Session.SetString("BattleRoom", JsonConvert.SerializeObject(room));
//         return RedirectToAction("BattleRoom");
//     }
//
//     [HttpGet]
//     [Route("[action]")]
//     public async Task<IActionResult> BattleRoom()
//     {
//         var roomString = HttpContext.Session.GetString("BattleRoom");
//         var studentString = HttpContext.Session.GetString("Student");
//         
//         if (string.IsNullOrEmpty(roomString) || string.IsNullOrEmpty(studentString))
//             return RedirectToAction("BattleLobby");
//
//         var room = JsonConvert.DeserializeObject<BattleRoom>(roomString);
//         var student = JsonConvert.DeserializeObject<Student>(studentString);
//         
//         ViewBag.CurrentPlayerId = student.StudentId;
//         return View(room);
//     }
//
//     [HttpPost]
//     [Route("[action]")]
//     public async Task<IActionResult> SubmitAnswer(string answer)
//     {
//         var roomString = HttpContext.Session.GetString("BattleRoom");
//         var studentString = HttpContext.Session.GetString("Student");
//         
//         if (string.IsNullOrEmpty(roomString) || string.IsNullOrEmpty(studentString))
//             return Json(new { success = false });
//
//         var room = JsonConvert.DeserializeObject<BattleRoom>(roomString);
//         var student = JsonConvert.DeserializeObject<Student>(studentString);
//
//         var result = await _battleService.SubmitAnswer(room.roomId, student.StudentId, answer);
//         return Json(result);
//     }
//
//   
}
using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using System.Text.Json;
using Entities;
using Services.Hubs;


namespace FrameForge.Controllers;

[Route("[controller]")]
public class BattleController : Controller
{
    private readonly IBattleService _battleService;
    private BattleSingleton _battlesingleton;
    private IUserService _studentService;

    public BattleController(IBattleService battleService, BattleSingleton battlesingleton, IUserService studentService)
    {
        _battleService = battleService;
        _battlesingleton = battlesingleton;
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
    public async Task<IActionResult> ExecuteBattle(BattleRoom room)
    {
        var student1 = (Student)await _studentService.GetUserById(room.Player1Id);
        var student12 = (Student)await _studentService.GetUserById((Guid)room.Player2Id);

        ViewBag.Room = room;
        ViewBag.Student = student1;
        ViewBag.Student2 = student12;
        return View("BattleRoom", room);
    }
    
    
    
    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> EndBattle(Guid roomId)
    {
        if (!_battlesingleton.CheckRoom(roomId))
        {
            _battlesingleton.AddRoomToHistory(roomId);
            var room = await _battleService.GetRoomStatus(roomId);
            var player1 = (Student)await _studentService.GetUserById(room.Player1Id);
            var player2 = (Student)await _studentService.GetUserById((Guid)room.Player2Id);
            player1.MoneyAmount += room.Player1Score;
            player2.MoneyAmount += room.Player2Score;
            await _studentService.UpdateStudent(player1);
            await _studentService.UpdateStudent(player2);

            await _battleService.EndBattle(roomId);
        }
        return Ok();
    }
    
    [HttpGet]
    [Route("[action]")]
    public async Task<IActionResult> GetStudentById(Guid id)
    {
        var student = (Student)await _studentService.GetUserById(id);
        return Ok(student);
    }
    
    [HttpPost]
    [Route("LeaveRoomOnUnload")]
    public async Task<IActionResult> EndBattleOnClose()
    {
        var form = await Request.ReadFormAsync();
        if (!form.TryGetValue("roomId", out var roomIdStr) || !Guid.TryParse(roomIdStr, out var roomId))
        {
            return BadRequest("Invalid roomId");
        }
        await _battleService.LeaveRoom(roomId); 
        return Ok();
    }
}
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

    public BattleController(IBattleService battleService, BattleSingleton battlesingleton)
    {
        _battleService = battleService;
        _battlesingleton = battlesingleton;
        
        
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
    
    
    
    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> EndBattle(Guid roomId)
    {
        if (!_battlesingleton.CheckRoom(roomId))
        {
            await _battleService.EndBattle(roomId);
            _battlesingleton.AddRoomToHistory(roomId);
        }
        return Ok();
    }
}
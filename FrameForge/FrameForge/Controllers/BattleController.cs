using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using System.Text.Json;
using Entities;
using Newtonsoft.Json;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace FrameForge.Controllers;

public class BattleController : Controller
{
    IBattleService _battleService;
    public BattleController(IBattleService battleService)
    {
        _battleService = battleService;
    }
    
    [Route("[action]")]
    public IActionResult Battle()
    {
        return View("Battle");
    }
    
    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> StartBattle()
    {
        var curStudentString = HttpContext.Session.GetString("Student");
        Student curStudent = null;
        if (curStudentString != null) curStudent = JsonConvert.DeserializeObject<Student>(curStudentString);
        if (await _battleService.IsRoomsToEnterExsist())
        {
            await _battleService.EnterTheRoom(curStudent);
            return RedirectToAction("Battle");
        }
        else
        {
            await _battleService.CreateRoom(curStudent);
            return RedirectToAction("Battle");
        }
    }

}
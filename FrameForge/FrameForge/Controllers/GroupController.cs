using System.Text.Json;
using Entities;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts;

namespace FrameForge.Controllers;

public class GroupController : Controller
{
    private readonly IGroupService _groupService;
    public Teacher? teacher;

    public GroupController(IGroupService groupService)
    {
        _groupService = groupService;
    }
    
    [Route("[action]")]
    public async Task<IActionResult> GroupsOverview()
    {
        teacher = getTeacherFromSession();

        List<Group>? groups = await _groupService.GetGroupByTeacherId(teacher.UserId);
        // if (groups.Count == 0) return RedirectToAction("CreateGroup");
        ViewBag.Groups = groups;
        
        return View(teacher);
    }

    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> SubmitGroup(string groupName)
    {
        teacher = getTeacherFromSession();
        var group = new Group();
        
        group.TeacherId = teacher.UserId;
        group.GroupName = groupName;
        group.Id = Guid.NewGuid();
        // group.Teacher = teacher;
        
        await _groupService.AddGroup(group);
        
        return RedirectToAction(nameof(GroupsOverview), teacher);
    }

    [Route("[action]")]
    public IActionResult CreateGroup()
    {
        return View();
    }
    
    private Teacher getTeacherFromSession()
    {
        var studentJson = HttpContext.Session.GetString("Teacher");
        if (studentJson != null) teacher = JsonSerializer.Deserialize<Teacher>(studentJson);
        if(teacher == null) throw new NullReferenceException("Student is null");
        
        return teacher;
    }
}
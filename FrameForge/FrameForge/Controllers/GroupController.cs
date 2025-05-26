using System.Text.Json;
using Entities;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts;

namespace FrameForge.Controllers;

public class GroupController : Controller
{
    private readonly IGroupService _groupService;
    private readonly IUserService _userService;
    private readonly IExerciseService _exerciseService;
    public Teacher? teacher;

    public GroupController(IGroupService groupService, IUserService userService, IExerciseService exerciseService)
    {
        _groupService = groupService;
        _userService = userService;
        _exerciseService = exerciseService;
    }
    
    [Route("[action]")]
    public async Task<IActionResult> GroupsOverview()
    {
        teacher = getTeacherFromSession();

        List<Group>? groups = await _groupService.GetGroupByTeacherId(teacher.UserId);
        groups.ForEach(u => u.Teacher = teacher);
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

    [Route("[action]")]
    public async Task<IActionResult> AssignStudent(string studentName, Guid groupId)
    {
        Student student = (Student)await _userService.GetUserByUserName(studentName);
        await _groupService.AssignStudent(groupId, student.UserId);
        
        return RedirectToAction(nameof(GroupsOverview), teacher);
    }

    public async Task<IActionResult> CompletedTasksOverview(Guid groupId)
    {
        var students = _userService.GetStudentsFromGroup(groupId);

        var submittedExercises = await _exerciseService.GetSubmissions(Guid.Empty);
        return await CompletedTasksOverview(groupId);
    }
    
    private Teacher getTeacherFromSession()
    {
        var studentJson = HttpContext.Session.GetString("Teacher");
        if (studentJson != null) teacher = JsonSerializer.Deserialize<Teacher>(studentJson);
        if(teacher == null) throw new NullReferenceException("Student is null");
        
        return teacher;
    }
}
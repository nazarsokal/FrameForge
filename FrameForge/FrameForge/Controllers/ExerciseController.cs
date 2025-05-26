using System.Text.Json;
using Entities;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts;

namespace FrameForge.Controllers;

public class ExerciseController : Controller
{
    private readonly IAzureStorageService _azureStorageService;
    private readonly IExerciseService _exerciseService;
    private readonly IGroupService _groupService;
    private Student student;

    public ExerciseController(IAzureStorageService azureStorageService, IExerciseService exerciseService, IGroupService groupService)
    {
        _azureStorageService = azureStorageService;
        _exerciseService = exerciseService;
        _groupService = groupService;
    }
    
    public async Task<IActionResult> CompleteExercise(Guid ExerciseId)
    {
        /*await _exerciseService.GetExercises(ExerciseId);*/
        return View();
    }

    [HttpGet]
    [Route("[action]")]
    public async Task<IActionResult> CreateExercise()
    {
        Teacher teacher = (Teacher)GetUserFromSession();
        List<Group> groupList = await _groupService.GetGroupByTeacherId(teacher.UserId);
        ViewBag.Groups = groupList;
        
        return View(teacher);
    }

    [HttpPost]
    [Route("[action]")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateExercise(Exercise exercise, string groupName)
    {
        Teacher teacher = (Teacher)GetUserFromSession();
        
        exercise.ExerciseId = Guid.NewGuid();
        exercise.ExerciseStartDate = DateTime.Now;
        
        var groups = await _groupService.GetGroupByTeacherId(teacher.UserId);
        var group = groups.FirstOrDefault(x => x.GroupName == groupName);
        
        exercise.GroupId = group.Id;
        
        await _exerciseService.AddExercise(exercise);
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    [Route("[action]")]
    public async Task<IActionResult> ExercisesOverview(Guid groupId)
    {
        var user = GetUserFromSession();
        
        var groups = await _groupService.GetGroupByTeacherId(user.UserId);
        var exercises = await _exerciseService.GetExercises(groupId);

        ViewBag.GroupName = groups.Where(g => g.Id == groupId).Select(n => n.GroupName);
        ViewBag.Exercises = exercises;
        
        return View(user);
    }
    
    private User GetUserFromSession()
    {
        User user = new User();
        var userType = HttpContext.Session.GetString("UserType");
        if(userType == "Teacher")
        {
            var userJson = HttpContext.Session.GetString("Teacher");
            user = JsonSerializer.Deserialize<Teacher>(userJson);
        }
        return user;
    }
}
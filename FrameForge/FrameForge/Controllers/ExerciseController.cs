using System.Text.Json;
using System.Text.Json.Serialization;
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
    
    [HttpGet]
    [Route("[controller]/[action]")]
    public async Task<IActionResult> CompleteExercise(Guid exerciseId)
    {
        student = (Student)GetUserFromSession();
        List<Exercise>? exercises = await _exerciseService.GetExercises(student.GroupId);
        
        var exercise = exercises.FirstOrDefault(e => e.ExerciseId == exerciseId);
        ViewBag.Exercise = exercise;
        
        return View(student);
    }

    [HttpPost]
    [Route("[controller]/[action]")]
    public async Task<IActionResult> CompleteExercise([FromBody] ExerciseRequest request)
    {
        student = (Student)GetUserFromSession();
        var codes = new List<string>();
        ExerciseSubmission submission = new ExerciseSubmission()
        {
            ExerciseId = request.Exercise.ExerciseId,
            ExerciseName = request.Exercise.ExerciseName,
            Status = "Completed",
            StudentSubmittedId = student.UserId,
            SubmissionDate = DateTime.Now,
            SubmittedExerciseId = Guid.NewGuid()
        };
        
        foreach (var file in request.Files)
        {
            codes.Add(file.Content); 
        }
        
        await _azureStorageService.UploadCode(codes, request.Exercise.ExerciseId, student.UserId);
        await _exerciseService.SubmitExercise(submission);

        return Ok(new { redirectUrl = Url.Action("Index", "Home") });
    }

    [HttpGet]
    [Route("[action]")]
    public async Task<IActionResult> CreateExercise()
    {
        Teacher teacher = (Teacher)GetUserFromSession();
        List<Group> groupList = await _groupService.GetGroupByTeacherId(teacher.UserId);
        ViewBag.Groups = groupList;
        
        return View();
    }

    [HttpPost]
    [Route("[action]")]
    public async Task<ActionResult> CreateExercise(Exercise exercise, string groupName)
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

    [HttpGet]
    public async Task<IActionResult> CheckExercise(Guid exerciseId)
    {
        
        return await CheckExercise(exerciseId);
    }
    
    private User GetUserFromSession()
    {
        User user = new User();
        var userType = HttpContext.Session.GetString("UserType");
        if(userType == "Teacher")
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

public class ExerciseRequest
{
    [JsonPropertyName("exercise")]
    public Exercise Exercise { get; set; }
    
    [JsonPropertyName("action")]
    public string Action { get; set; }

    [JsonPropertyName("files")]
    public List<ExerciseFile> Files { get; set; }

    [JsonPropertyName("language")]
    public string Language { get; set; }

    [JsonPropertyName("result")]
    public ExerciseResult Result { get; set; }

    [JsonPropertyName("stdin")]
    public string Stdin { get; set; }

    [JsonPropertyName("_id")]
    public string Id { get; set; }
}


public class ExerciseFile
{
    public string Name { get; set; }
    public string Content { get; set; }
}

public class ExerciseResult
{
    public bool Success { get; set; }
    public string Output { get; set; }
}

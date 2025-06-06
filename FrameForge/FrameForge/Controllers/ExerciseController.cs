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
    private readonly IUserService _userService;
    private Student student;

    public ExerciseController(IAzureStorageService azureStorageService, IExerciseService exerciseService, IGroupService groupService, IUserService userService)
    {
        _azureStorageService = azureStorageService;
        _exerciseService = exerciseService;
        _groupService = groupService;
        _userService = userService;
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

    [HttpGet]
    [Route("[controller]/[action]")]
    public async Task<IActionResult> ReviewRatedTask(Guid exerciseId)
    {
        student = (Student)GetUserFromSession();
        var ratedExercise = await _exerciseService.GetSubmission(exerciseId);
        var exercise = await _exerciseService.GetExercise(ratedExercise.ExerciseId);
        var code = await _azureStorageService.GetSubmittedTasks(exercise.ExerciseId, student.UserId);
        
        ViewBag.Exercise = exercise;
        ViewBag.RatedExercise = ratedExercise;
        ViewBag.ExerciseCode = code;
        
        return View("ReviewRatedTask", student);
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
    public async Task<IActionResult> RateExercise(Guid submittedExerciseId, Guid userId)
    {
        Teacher teacher = (Teacher)GetUserFromSession();
        // var submittedExercises = await _exerciseService.GetSubmission(submittedExerciseId);
        var exercise = await _exerciseService.GetExercise(submittedExerciseId);
        var exerciseRequestFromAzure = await _azureStorageService.GetSubmittedTasks(submittedExerciseId, userId);
        
        HttpContext.Session.SetString("submittedUserId", userId.ToString());
        
        ViewBag.SubmittedExercises = exercise;
        ViewBag.ExerciseCode = exerciseRequestFromAzure;
        
        return View(teacher);
    }

    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> RateExercise(Guid submittedExerciseId, string feedback,
        string moneyReward, string starsReward)
    {
        Teacher teacher = (Teacher)GetUserFromSession();
        
        var exercise = await _exerciseService.GetSubmission(submittedExerciseId);
        await _exerciseService.RateExercise(exercise.ExerciseId, feedback, Convert.ToDouble(moneyReward), int.Parse(starsReward));
        
        var submittedUserId = Guid.Parse(HttpContext.Session.GetString("submittedUserId"));
        Student student = (Student)await _userService.GetUserById(submittedUserId);
        
        await _userService.UpdateStudent(student);
        
        return RedirectToAction("Index", "Home");
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


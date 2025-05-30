using System.Text.Json;
using Entities;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.Enums;

namespace FrameForge.Controllers;

public class ProgressMapController : Controller
{
    private readonly IProgressMapService _service;
    private readonly IUserService _userService;
    private List<EnrolledLevels>? userLevelsEnrolledInProgress;
    private List<EnrolledLevels>? userLevelsEnrolledCompleted;
    private readonly List<string> availableLevels = new List<string>() { "CG_IntroductionLevel",  "CG_Level2", "CG_Level3", "CG_Level4", "CG_Level5"}; 
    private Student? student;

    public ProgressMapController(IProgressMapService service, IUserService userService)
    {
        _service = service;
        _userService = userService;
    }
    [Route("[action]")]
    public async Task<IActionResult> Map()
    {
        var student = GetStudentFromSession();
        userLevelsEnrolledInProgress = _service.GetUsersEnrolledLevelsInProgress(student);
        userLevelsEnrolledCompleted = _service.GetUsersEnrolledLevelsCompleted(student);

        ViewBag.CompletedLevels = userLevelsEnrolledCompleted;
        ViewBag.InProgressLevels = userLevelsEnrolledInProgress;
        return View(student);
    }

    [Route("[controller]/[action]")]
    public IActionResult ViewLevel(string levelName)
    {
        ViewBag.LevelName = levelName;
        var student = GetStudentFromSession();
        userLevelsEnrolledInProgress = _service.GetUsersEnrolledLevelsInProgress(student);
        userLevelsEnrolledCompleted = _service.GetUsersEnrolledLevelsCompleted(student);
        
        if (userLevelsEnrolledInProgress == null)
            userLevelsEnrolledInProgress = new List<EnrolledLevels>(); 
        if(userLevelsEnrolledCompleted == null)
            userLevelsEnrolledCompleted = new List<EnrolledLevels>();
        
        EnrolledLevels? isLevelEnrolled =
            userLevelsEnrolledInProgress.FirstOrDefault(l => l.LevelTopicName == levelName && student.UserId == l.StudentId);        
        EnrolledLevels? isLevelCompleted =
            userLevelsEnrolledCompleted.FirstOrDefault(l => l.LevelTopicName == levelName && student.UserId == l.StudentId);
        // if (isLevelEnrolled == null && isLevelCompleted == null)
        // {
        //     if (isPreviousLevelCompleted(userLevelsEnrolledCompleted, levelName))
        //     {
        //         _service.EnrolOnLevel(student, levelName);    
        //         return View(levelName);
        //     }
        //     else
        //     {
        //         throw new Exception("Invalid level");
        //     }
        // }
        
        return View(levelName, student);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("[action]")]
    public async Task<IActionResult> CompleteLevel(string levelName, MoneyStarsResult moneyResult)
    {
        var studentCompleted = GetStudentFromSession();
        _service.CompleteOnLevel(studentCompleted, levelName, moneyResult.Stars, moneyResult.Money);
        
        var index = availableLevels.IndexOf(levelName);
        
        if (index != availableLevels.Count - 1)
        {
            await _service.SetNextLevel(studentCompleted, availableLevels[index+1]);
        }
        
        // studentCompleted.MoneyAmount += moneyResult.Money;
        // studentCompleted.StarsAmount += moneyResult.Stars;
        
        //var studentUpdated =  await _userService.UpdateStudent(studentCompleted);
        
        //string userString = JsonSerializer.Serialize(studentUpdated);
        //HttpContext.Session.SetString("Student", userString);
        
        return RedirectToAction("Map");
    }
    
    private Student GetStudentFromSession()
    {
        var studentJson = HttpContext.Session.GetString("Student");
        if (studentJson != null) student = JsonSerializer.Deserialize<Student>(studentJson);
        if(student == null) throw new NullReferenceException("Student is null");
        
        return student;
    }

    private bool isPreviousLevelCompleted(List<EnrolledLevels> userLevelsEnrolledCompleted, string levelName)
    {
        var student = GetStudentFromSession();
        int indexAmount = 0;
        foreach (var level in availableLevels)
        {
            bool isLevelContained = userLevelsEnrolledCompleted.Any(l => l.LevelTopicName == level && student.UserId == l.StudentId);
            if(isLevelContained) indexAmount++;
        }

        int indexOfLevelName = 0;
        bool isLevelContainedAl = availableLevels.Any(l => l == levelName);
        if (isLevelContainedAl)
        {
            indexOfLevelName = availableLevels.IndexOf(levelName);
        }
        
        if((indexOfLevelName+1) - indexAmount > 1) return false;
        
        return true;
    }
}
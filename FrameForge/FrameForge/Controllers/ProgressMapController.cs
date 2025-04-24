using System.Text.Json;
using Entities;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.Enums;

namespace FrameForge.Controllers;

public class ProgressMapController : Controller
{
    private readonly IProgressMapService _service;
    private readonly IStudentService _studentService;
    private List<EnrolledLevels>? userLevelsEnrolledInProgress;
    private List<EnrolledLevels>? userLevelsEnrolledCompleted;
    private readonly List<string> availableLevels = new List<string>() { "CG_IntroductionLevel",  "CG_Level2", "CG_Level3", "CG_Level4", "CG_Level5"}; 
    private Student? student;

    public ProgressMapController(IProgressMapService service, IStudentService studentService)
    {
        _service = service;
        _studentService = studentService;
    }
    [Route("[action]")]
    public IActionResult Map()
    {
        var student = GetStudentFromSession();
        userLevelsEnrolledInProgress = _service.GetUsersEnrolledLevelsInProgress(student);
        userLevelsEnrolledCompleted = _service.GetUsersEnrolledLevelsCompleted(student);
        ViewBag.CompletedLevels = userLevelsEnrolledCompleted;
        ViewBag.InProgressLevels = userLevelsEnrolledInProgress;
        return View(student);
    }

    [Route("[action]")]
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
            userLevelsEnrolledInProgress.FirstOrDefault(l => l.LevelTopicName == levelName && student.StudentId == l.StudentId);        
        EnrolledLevels? isLevelCompleted =
            userLevelsEnrolledCompleted.FirstOrDefault(l => l.LevelTopicName == levelName && student.StudentId == l.StudentId);
        if (isLevelEnrolled == null && isLevelCompleted == null)
        {
            if (isPreviousLevelCompleted(userLevelsEnrolledCompleted, levelName))
            {
                _service.EnrolOnLevel(student, levelName);    
                return View(levelName);
            }
            else
            {
                throw new Exception("Invalid level");
            }
        }
        
        return View(levelName);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("[action]")]
    public IActionResult CompleteLevel(string levelName, MoneyStarsResult moneyResult)
    {
        var studentCompleted = GetStudentFromSession();
        _service.CompleteOnLevel(studentCompleted, levelName, moneyResult.Stars, moneyResult.Money);
        
        studentCompleted.MoneyAmount += moneyResult.Money;
        studentCompleted.StarsAmount += moneyResult.Stars;
        
        var studentUpdated = _studentService.UpdateStudent(studentCompleted);
        
        string userString = JsonSerializer.Serialize(studentUpdated);
        HttpContext.Session.SetString("Student", userString);
        
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
            bool isLevelContained = userLevelsEnrolledCompleted.Any(l => l.LevelTopicName == level && student.StudentId == l.StudentId);
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
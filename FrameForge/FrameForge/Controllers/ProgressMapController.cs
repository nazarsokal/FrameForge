using System.Text.Json;
using Entities;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts;

namespace FrameForge.Controllers;

public class ProgressMapController : Controller
{
    private readonly IProgressMapService _service;
    private List<EnrolledLevels>? userLevelsEnrolled;
    private Student? student;

    public ProgressMapController(IProgressMapService service)
    {
        _service = service;
    }
    [Route("[action]")]
    public IActionResult Map()
    {
        var student = GetStudentFromSession();
        userLevelsEnrolled = _service.GetUsersEnrolledLevels(student);
        ViewBag.CompletedLevels = userLevelsEnrolled;
        
        return View();
    }

    [Route("[action]")]
    public IActionResult ViewLevel(string levelName)
    {
        var student = GetStudentFromSession();
        userLevelsEnrolled = _service.GetUsersEnrolledLevels(student);
        if (userLevelsEnrolled == null)
        {
            userLevelsEnrolled = new List<EnrolledLevels>(); 
        }
        
        var isLevelEnrolled =
            userLevelsEnrolled.Any(l => l.LevelTopicName == levelName && student.StudentId == l.StudentId);
        if (!isLevelEnrolled)
        {
            _service.EnrolOnLevel(student, levelName);    
            return View(levelName);
        }
        
        return View(levelName);
    }
    
    private Student GetStudentFromSession()
    {
        var studentJson = HttpContext.Session.GetString("Student");
        if (studentJson != null) student = JsonSerializer.Deserialize<Student>(studentJson);
        if(student == null) throw new NullReferenceException("Student is null");
        
        return student;
    }
}
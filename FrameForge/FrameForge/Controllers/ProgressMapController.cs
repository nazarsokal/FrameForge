using System.Text.Json;
using Entities;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.Enums;

namespace FrameForge.Controllers;

public class ProgressMapController : Controller
{
    private readonly IProgressMapService _progressMapService;
    private readonly IStudentService _studentService;
    private readonly IMentorService _mentorService;
    private List<EnrolledLevels>? _userLevelsEnrolledInProgress;
    private List<EnrolledLevels>? _userLevelsEnrolledCompleted;
    private readonly Dictionary<int, string> _availableLevels = new Dictionary<int, string>(); 
    private Student? _student;

    public ProgressMapController(IProgressMapService progressMapService, IStudentService studentService, IMentorService mentorService)
    {
        _progressMapService = progressMapService;
        _studentService = studentService;
        _mentorService = mentorService;
        _availableLevels = new Dictionary<int, string>() {
            { 1, "CG_IntroductionLevel" }, { 2, "CG_BezierCurveLevel" }, { 3, "CG_FractalsLevel" }, { 4, "CG_ColorModelsLevel" }, {5 ,"CG_MovingImagesLevel" }};
    }
    [Route("[action]")]
    public IActionResult Map()
    {
        var student = GetStudentFromSession();
        _userLevelsEnrolledInProgress = _progressMapService.GetUsersEnrolledLevelsInProgress(student);
        _userLevelsEnrolledCompleted = _progressMapService.GetUsersEnrolledLevelsCompleted(student);
        ViewBag.CompletedLevels = _userLevelsEnrolledCompleted;
        ViewBag.InProgressLevels = _userLevelsEnrolledInProgress;
        return View();
    }

    [Route("[action]")]
    public async  Task<IActionResult> ViewLevel(string levelName)
    {
        ViewBag.LevelName = levelName;
        var student = GetStudentFromSession();
        _userLevelsEnrolledInProgress = _progressMapService.GetUsersEnrolledLevelsInProgress(student);
        _userLevelsEnrolledCompleted = _progressMapService.GetUsersEnrolledLevelsCompleted(student);
        
        if (_userLevelsEnrolledInProgress == null)
            _userLevelsEnrolledInProgress = new List<EnrolledLevels>(); 
        if(_userLevelsEnrolledCompleted == null)
            _userLevelsEnrolledCompleted = new List<EnrolledLevels>();
        
        EnrolledLevels? isLevelEnrolled =
            _userLevelsEnrolledInProgress.FirstOrDefault(l => l.LevelTopicName == levelName && student.StudentId == l.StudentId);        
        EnrolledLevels? isLevelCompleted =
            _userLevelsEnrolledCompleted.FirstOrDefault(l => l.LevelTopicName == levelName && student.StudentId == l.StudentId);
        
        var levelKey = _availableLevels.Where(kvp => kvp.Value == levelName).Select(kvp => kvp.Key).FirstOrDefault();
        List<MentorNpcQuotes> quotesList = await _mentorService.GetMentorQuotesByLevelNumber(levelKey);
        ViewBag.Quotes = quotesList;
        
        if (isLevelEnrolled == null && isLevelCompleted == null)
        {
            if (isPreviousLevelCompleted(_userLevelsEnrolledCompleted, levelName))
            {
                _progressMapService.EnrolOnLevel(student, levelName);
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
        _progressMapService.CompleteOnLevel(studentCompleted, levelName, moneyResult.Stars, moneyResult.Money);
        
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
        if (studentJson != null) _student = JsonSerializer.Deserialize<Student>(studentJson);
        if(_student == null) throw new NullReferenceException("Student is null");
        
        return _student;
    }

    private bool isPreviousLevelCompleted(List<EnrolledLevels> userLevelsEnrolledCompleted, string levelName)
    {
        var student = GetStudentFromSession();
        int indexAmount = 0;
        foreach (var level in _availableLevels.Values)
        {
            bool isLevelContained = userLevelsEnrolledCompleted.Any(l => l.LevelTopicName == level && student.StudentId == l.StudentId);
            if(isLevelContained) indexAmount++;
        }

        int indexOfLevelName = 0;
        bool isLevelContainedAl = _availableLevels.Values.Any(l => l == levelName);
        if (isLevelContainedAl)
        {
            indexOfLevelName = _availableLevels.FirstOrDefault(kvp => kvp.Value == levelName).Key;
        }
        
        if(indexOfLevelName - indexAmount > 1) return false;
        
        return true;
    }
}
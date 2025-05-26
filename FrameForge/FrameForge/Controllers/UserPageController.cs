using System.Text.Json;
using Entities;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using Services;

namespace FrameForge.Controllers;

public class UserPageController : Controller
{
    private readonly LeaderboardService _leaderboardService;
    private readonly IProgressMapService _progressMapService;
    private readonly IExerciseService _exerciseService;
    private Student student = new Student();

    public UserPageController(LeaderboardService leaderboardService, IProgressMapService progressMapService, IExerciseService exerciseService)
    {
        _leaderboardService = leaderboardService;
        _progressMapService = progressMapService;
        _exerciseService = exerciseService;
    }
    
    [HttpGet]
    [Route("[action]")]
    public async Task<IActionResult> UserPage()
    {
        List<LevelsToDisplay> completedLevelsToDisplay = new List<LevelsToDisplay>();
        List<LevelsToDisplay> enrolledLevelsToDisplay = new List<LevelsToDisplay>();
        student = GetStudentFromSession();
        
        var completedLevels = _progressMapService.GetUsersEnrolledLevelsCompleted(student);
        var enrolledLevels = _progressMapService.GetUsersEnrolledLevelsInProgress(student);
        foreach (var completedLevel in completedLevels)
        {
            string levelName = " ";
            if(completedLevel.LevelTopicName == "CG_IntroductionLevel") levelName = "Вступ до комп'ютерної графіки";
            else if(completedLevel.LevelTopicName == "CG_Level2") levelName = "Крива Безьє";
            else if(completedLevel.LevelTopicName == "CG_Level3") levelName = "Побудова фрактальних зображень";
            else if(completedLevel.LevelTopicName == "CG_Level4") levelName = "Комп'ютерні моделі кольорів";
            else if(completedLevel.LevelTopicName == "CG_Level5") levelName = "Побудова рухомих зображень";
            
            completedLevelsToDisplay.Add(new LevelsToDisplay() {Name = levelName, MoneyRewarded = completedLevel.MoneyReward, StarsRewarded = completedLevel.StarsReward});
        }        
        
        foreach (var enrolledLevel in enrolledLevels)
        {
            string levelName = " ";
            if(enrolledLevel.LevelTopicName == "CG_IntroductionLevel") levelName = "Вступ до комп'ютерної графіки";
            else if(enrolledLevel.LevelTopicName == "CG_Level2") levelName = "Крива Безьє";
            else if(enrolledLevel.LevelTopicName == "CG_Level3") levelName = "Побудова фрактальних зображень";
            else if(enrolledLevel.LevelTopicName == "CG_Level4") levelName = "Комп'ютерні моделі кольорів";
            else if(enrolledLevel.LevelTopicName == "CG_Level5") levelName = "Побудова рухомих зображень";
            
            enrolledLevelsToDisplay.Add(new LevelsToDisplay() {Name = levelName, MoneyRewarded = enrolledLevel.MoneyReward, StarsRewarded = enrolledLevel.StarsReward});
        }
        
        var studentsPlace = _leaderboardService.GetStudentsPlace(student);
        var completedExercises = await _exerciseService.GetSubmissions(student.UserId);
        
        ViewBag.completedExercises = completedExercises;
        ViewBag.StudentsPlace = studentsPlace;
        ViewBag.EnrolledLevels = enrolledLevelsToDisplay;
        ViewBag.CompletedLevels = completedLevelsToDisplay;
        
        return View("user_page", student);
    }
    
    private Student GetStudentFromSession()
    {
        var studentJson = HttpContext.Session.GetString("Student");
        if (studentJson != null) student = JsonSerializer.Deserialize<Student>(studentJson);
        if(student == null) throw new NullReferenceException("Student is null");
        
        return student;
    }
}
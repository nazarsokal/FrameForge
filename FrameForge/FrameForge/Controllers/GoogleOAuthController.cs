using System.Text.Json;
using Entities;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.Helpers;
using Services;

namespace FrameForge.Controllers;

[Route("[controller]")]
public class GoogleOAuthController : Controller
{
    private readonly IGoogleOAuthService _googleOAuthService;
    private readonly IRegistrationService _registrationService;
    private readonly IProgressMapService _progressMapService;

    public GoogleOAuthController(IGoogleOAuthService googleOAuthService, IRegistrationService registrationService, IProgressMapService progressMapService)
    {
        _googleOAuthService = googleOAuthService;
        _registrationService = registrationService;
        _progressMapService = progressMapService;
    }

    [Route("[action]")]
    public IActionResult RedirectOnOAuthServer(bool isTeacher)
    {
        string scope = "openid https://www.googleapis.com/auth/userinfo.profile https://www.googleapis.com/auth/userinfo.email";
        string redirectUrl = $"http://localhost:5118/GoogleOAuth/Code";
        var codeVerifier = $"{Guid.NewGuid()}{Guid.NewGuid()}";
        
        HttpContext.Session.SetString("isTeacher", isTeacher.ToString());
        HttpContext.Session.SetString("codeVerifier", codeVerifier);
        
        var codeChallenge = Sha256Helper.ComputeHash(codeVerifier);
        
        var url = _googleOAuthService.GenerateOAuthRequestUrl(scope, redirectUrl, codeChallenge);
        return Redirect(url);
    }

    [Route("[action]")]
    public async Task<IActionResult> Code(string code)
    {
        string? codeVerifier = HttpContext.Session.GetString("codeVerifier");
        bool isTeacher = Convert.ToBoolean(HttpContext.Session.GetString("isTeacher"));
        
        string redirectUrl = $"http://localhost:5118/GoogleOAuth/Code";
        var tokenResult = await _googleOAuthService.ExchangeCodeOnToken(code, codeVerifier, redirectUrl);

        if (!isTeacher)
        {
            User? studentInfo = await _googleOAuthService.GetUserInfo(tokenResult.AccessToken);
            if (studentInfo == null) throw new NullReferenceException();
            var studentFromGoogleInfo = new Student()
            {
                Username = studentInfo.Username,
                Email = studentInfo.Email,
                GoogleId = studentInfo.GoogleId,
                Picture = studentInfo.Picture,
            };
            
            Student user = await _registrationService.RegisterStudentWithGoogle(studentFromGoogleInfo) as Student;  
            if (user is Student student)
            {
                var enrolledLevelsList =  _progressMapService.GetUsersEnrolledLevelsCompleted(student);
                var enrolledLevelsListCompleted =  _progressMapService.GetUsersEnrolledLevelsInProgress(student);
                if (enrolledLevelsList.Count == 0 && enrolledLevelsListCompleted.Count == 0)
                {
                    await _progressMapService.SetNextLevel(student, "CG_IntroductionLevel");
                }
                string userString = JsonSerializer.Serialize(student);
                HttpContext.Session.SetString("UserType", "Student");
                HttpContext.Session.SetString("Student", userString);
            }
            
        }
        else
        {
            User? studentInfo = await _googleOAuthService.GetUserInfo(tokenResult.AccessToken);
            if (studentInfo == null) throw new NullReferenceException();
            var teacherFromGoogleInfo = new Teacher()
            {
                Username = studentInfo.Username,
                Email = studentInfo.Email,
                GoogleId = studentInfo.GoogleId,
                Picture = studentInfo.Picture,
            };
            Teacher user = await _registrationService.RegisterStudentWithGoogle(teacherFromGoogleInfo) as Teacher;

            if (user is Teacher teacher)
            {
                string userString = JsonSerializer.Serialize(teacher);
                HttpContext.Session.SetString("UserType", "Teacher");
                HttpContext.Session.SetString("Teacher", userString);
            }
        }
        
        return RedirectToAction("Index", "Home");
    }
}
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using Services;
using System.Drawing.Drawing2D;
using System.Text.Json;

namespace FrameForge.Controllers;
public class RegistrationController : Controller
{
    private readonly IRegistrationService _registrationService;
    private readonly IProgressMapService _progressMapService;

    public RegistrationController(IGoogleOAuthService googleOAuthService, IRegistrationService registrationService, IProgressMapService progressMapService)
    {
        _registrationService = registrationService;
        _progressMapService = progressMapService;
    }
    // GET
    [Route("[action]")]
    public IActionResult Registration()
    {
        return View("sing_up_page");
    }
    [Route("[action]")]
    public IActionResult RegistrationWithFormData()
    {
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("Registration/Create")]
    public async Task<IActionResult> Create(Student student)
    {
        if (student != null)
        {
            student.GoogleId = null;
            student.MoneyAmount = 20.0;
            student.Password = PasswordHelper.HashPassword(student.Password);
            await _registrationService.RegisterStudent(student);

            var enrolledLevelsList =  _progressMapService.GetUsersEnrolledLevelsCompleted(student);
            var enrolledLevelsListCompleted =  _progressMapService.GetUsersEnrolledLevelsInProgress(student);
            if (enrolledLevelsList.Count == 0 && enrolledLevelsListCompleted.Count == 0)
            {
                await _progressMapService.SetNextLevel(student, "CG_IntroductionLevel");
            }
            
            string userString = JsonSerializer.Serialize(student);
            HttpContext.Session.SetString("Student", userString);
        }
        return RedirectToAction("Index", "Home");
    }

}
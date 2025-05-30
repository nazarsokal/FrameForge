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
    public async Task<IActionResult> Create(RegisterViewModel model)
    {
        if (model != null)
        {
            if (model.IsTeacher)
            {
                var teacher = new Teacher
                {
                    Username = model.Name,
                    Email = model.Email,
                    Password = PasswordHelper.HashPassword(model.Password)
                };

                await _registrationService.RegisterUser(teacher);
                HttpContext.Session.SetString("UserType", "Teacher");
                HttpContext.Session.SetString("Teacher", JsonSerializer.Serialize(teacher));
            }
            else
            {
                var student = new Student
                {
                    Username = model.Name,
                    Email = model.Email,
                    Password = PasswordHelper.HashPassword(model.Password),
                    MoneyAmount = 20.0
                };

                await _registrationService.RegisterUser(student);
                HttpContext.Session.SetString("UserType", "Student");
                HttpContext.Session.SetString("Student", JsonSerializer.Serialize(student));

                var enrolledLevelsList = _progressMapService.GetUsersEnrolledLevelsCompleted(student);
                var enrolledLevelsListCompleted = _progressMapService.GetUsersEnrolledLevelsInProgress(student);
                if (enrolledLevelsList.Count == 0 && enrolledLevelsListCompleted.Count == 0)
                {
                    await _progressMapService.SetNextLevel(student, "CG_IntroductionLevel");
                }
            }
        }

        return RedirectToAction("Index", "Home");
    }

}
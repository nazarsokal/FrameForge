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

    public RegistrationController(IGoogleOAuthService googleOAuthService, IRegistrationService registrationService)
    {
        _registrationService = registrationService;
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
    public IActionResult Create(Student student)
    {
        if (student != null)
        {
            student.GoogleId = null;
            student.MoneyAmount = 20.0;
            student.Password = PasswordHelper.HashPassword(student.Password);
            _registrationService.RegisterStudent(student);
            
            string userString = JsonSerializer.Serialize(student);
            HttpContext.Session.SetString("Student", userString);
        }
        return RedirectToAction("Index", "Home");
    }

}
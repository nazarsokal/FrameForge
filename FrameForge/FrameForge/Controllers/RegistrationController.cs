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
    public async Task<IActionResult> Create(Student student)
    {
        if (student != null)
        {
            var password = student.Password;
            student.GoogleId = null;
            student.MoneyAmount = 20.0;
            student.Password = PasswordHelper.HashPassword(student.Password);
            await _registrationService.RegisterStudent(student);

            Student studentFromDb = await _registrationService.GetStudent(student.Username, password);
            string userString = JsonSerializer.Serialize(studentFromDb);
            HttpContext.Session.SetString("Student", userString);
        }
        return RedirectToAction("Index", "Home");
    }

}
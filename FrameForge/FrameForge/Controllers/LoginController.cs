using Microsoft.AspNetCore.Mvc;
using Entities;
using Newtonsoft.Json;
using ServiceContracts;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace FrameForge.Controllers
{
    public class LoginController : Controller
    {
        private readonly IRegistrationService _registrationService;

        public LoginController(IRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }
        [Route("[action]")]
        public IActionResult Login()
        {
            return View("login_page");
        }
        [HttpPost]
        [Route("[action]")]

        public async Task<IActionResult> LoginWithLoginAndPassword(InputLoginData? ld)
        {
            if (ld != null) 
            {
                if (ld.IsTeacher)
                {
                    User? user = await _registrationService.GetTeacher(ld.Username, ld.Password);
                    
                    if (user != null)
                    {
                        string userString = JsonSerializer.Serialize(user);
                        HttpContext.Session.SetString("UserType", "Teacher");
                        HttpContext.Session.SetString("Teacher", userString);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    User? user = await _registrationService.GetStudent(ld.Username, ld.Password);
                    
                    if (user != null)
                    {
                        string userString = JsonSerializer.Serialize(user);
                        HttpContext.Session.SetString("UserType", "Student");
                        HttpContext.Session.SetString("Student", userString);
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            else
            {
                return NotFound();
            }
            
            return View("login_page");
        }
    }
}

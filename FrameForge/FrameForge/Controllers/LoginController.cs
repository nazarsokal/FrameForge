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
                Student? student = await _registrationService.GetStudent(ld.Username, ld.Password);
                if (student != null)
                {
                     string userString = JsonSerializer.Serialize(student);
                     HttpContext.Session.SetString("Student", userString);
                     return RedirectToAction("Index", "Home");
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return NotFound();
            }
        }
    }
}

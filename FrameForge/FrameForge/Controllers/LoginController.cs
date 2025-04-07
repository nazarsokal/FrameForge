using Microsoft.AspNetCore.Mvc;
using Entities;
using ServiceContracts;
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
        [HttpGet]
        [Route("Login/LoginWithLoginAndPassword")]

        public IActionResult LoginWithLoginAndPassword(InputLoginData ld)
        {
            if (ld != null) 
            {
                var student = _registrationService.GetStudent(ld.Username, ld.Password);
                if (student!=null)
                {
                     return RedirectToAction("Index", "Home", student);
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

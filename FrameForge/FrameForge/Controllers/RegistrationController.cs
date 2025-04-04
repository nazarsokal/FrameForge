using Microsoft.AspNetCore.Mvc;

namespace FrameForge.Controllers;

public class RegistrationController : Controller
{
    // GET
    [Route("[action]")]
    public IActionResult Registration()
    {
        return View();
    }
}
using Microsoft.AspNetCore.Mvc;

namespace FrameForge.Controllers
{
    public class LogOutController : Controller
    {
        [Route("[action]")]
        public IActionResult logOut()
        {
            HttpContext.Session.Remove("Student");
            return RedirectToAction("Index","Home");
        }
    }
}

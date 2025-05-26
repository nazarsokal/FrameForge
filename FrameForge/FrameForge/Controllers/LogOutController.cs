using Microsoft.AspNetCore.Mvc;

namespace FrameForge.Controllers
{
    public class LogOutController : Controller
    {
        [Route("[action]")]
        public IActionResult logOut()
        {
            var userType = HttpContext.Session.GetString("UserType");
            HttpContext.Session.Remove(userType);
            HttpContext.Session.Remove("UserType");
            return RedirectToAction("Index","Home");
        }
    }
}

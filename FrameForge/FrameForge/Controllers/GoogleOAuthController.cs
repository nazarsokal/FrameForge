using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using Services;

namespace FrameForge.Controllers;

public class GoogleOAuthController : Controller
{
    private readonly IGoogleOAuthService _googleOAuthService;

    public GoogleOAuthController(IGoogleOAuthService googleOAuthService)
    {
        _googleOAuthService = googleOAuthService;
    }

    public IActionResult RedirectOnOAuthServer()
    {
        var url = _googleOAuthService.GenerateOAuthRequestUrl();
        return Redirect(url);
    }

    public IActionResult Code(string code)
    {
        var tokenResult = _googleOAuthService.ExchangeCodeOnToken(code);
        return Ok();
    }
}
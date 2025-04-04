using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.Helpers;
using Services;

namespace FrameForge.Controllers;

[Route("[controller]")]
public class GoogleOAuthController : Controller
{
    private readonly IGoogleOAuthService _googleOAuthService;
    
    public GoogleOAuthController(IGoogleOAuthService googleOAuthService)
    {
        _googleOAuthService = googleOAuthService;
    }

    [Route("[action]")]
    public IActionResult RedirectOnOAuthServer()
    {
        string scope = "openid https://www.googleapis.com/auth/userinfo.profile https://www.googleapis.com/auth/userinfo.email";
        string redirectUrl = $"http://localhost:5118/GoogleOAuth/Code";
        var codeVerifier = $"{Guid.NewGuid()}{Guid.NewGuid()}";
        
        HttpContext.Session.SetString("codeVerifier", codeVerifier);
        
        var codeChallenge = Sha256Helper.ComputeHash(codeVerifier);
        
        var url = _googleOAuthService.GenerateOAuthRequestUrl(scope, redirectUrl, codeChallenge);
        return Redirect(url);
    }

    [Route("[action]")]
    public async Task<IActionResult> Code(string code)
    {
        string? codeVerifier = HttpContext.Session.GetString("codeVerifier");
        
        string redirectUrl = $"http://localhost:5118/GoogleOAuth/Code";
        var tokenResult = await _googleOAuthService.ExchangeCodeOnToken(code, codeVerifier, redirectUrl);
        var UserResult = await _googleOAuthService.GetUserInfo(tokenResult.AccessToken);
        return Ok();
    }
}
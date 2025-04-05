using Entities;

namespace ServiceContracts;

public interface IGoogleOAuthService
{
    /// <summary>
    /// Getting request url based on client ID
    /// </summary>
    /// <param name="scope"></param>
    /// <param name="redirectUrl"></param>
    /// <param name="codeChallenge"></param>
    /// <returns></returns>
    public string GenerateOAuthRequestUrl(string scope, string redirectUrl, string codeChallenge);

    /// <summary>
    /// Exchanging received code on token by making POST request to google API
    /// </summary>
    /// <param name="code"></param>
    /// <param name="codeVerifier"></param>
    /// <returns></returns>
    public Task<TokenResult> ExchangeCodeOnToken(string code, string codeVerifier, string redirectUrl);
    
    public object RefreshToke(string refreshToken);

    public Task<Student> GetUserInfo(string accessToken);
}
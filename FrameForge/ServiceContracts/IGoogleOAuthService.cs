namespace ServiceContracts;

public interface IGoogleOAuthService
{
    public string GenerateOAuthRequestUrl(string scope, string redirectUrl, string codeChallenge);

    public object ExchangeCodeOnToken(string code);
    
    public object RefreshToke(string refreshToken);
}
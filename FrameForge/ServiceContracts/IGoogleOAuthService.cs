namespace ServiceContracts;

public interface IGoogleOAuthService
{
    public string GenerateOAuthRequestUrl();

    public object ExchangeCodeOnToken(string code);
    
    public object RefreshToke(string refreshToken);
}
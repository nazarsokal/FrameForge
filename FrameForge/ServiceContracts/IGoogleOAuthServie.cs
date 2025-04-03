namespace ServiceContracts;

public interface IGoogleOAuthServie
{
    public string GenerateOAuthRequestUrl();

    public object ExchangeCodeOnToken();
    
    public object RefreshToke(string refreshToken);
}
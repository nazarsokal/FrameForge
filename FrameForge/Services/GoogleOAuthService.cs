using ServiceContracts;

namespace Services;

public class GoogleOAuthService : IGoogleOAuthService
{
    private const string ClientId = "93167673295-e3ud1ef5u8s9f5fvlrpgpe15eqkcnblh.apps.googleusercontent.com";
    private const string ClientSecret = "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";
    public string GenerateOAuthRequestUrl()
    {
        throw new NotImplementedException();
    }

    public object ExchangeCodeOnToken(string code)
    {
        throw new NotImplementedException();
    }

    public object RefreshToke(string refreshToken)
    {
        throw new NotImplementedException();
    }
}
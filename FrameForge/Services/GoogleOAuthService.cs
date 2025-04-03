using Microsoft.AspNetCore.WebUtilities;
using ServiceContracts;

namespace Services;

public class GoogleOAuthService : IGoogleOAuthService
{
    private const string ClientId = "93167673295-e3ud1ef5u8s9f5fvlrpgpe15eqkcnblh.apps.googleusercontent.com";
    private const string ClientSecret = "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";
    public string GenerateOAuthRequestUrl(string scope, string redirectUrl, string codeChallenge)
    {
        var oAuthEndpoint = $"https://accounts.google.com/o/oauth2/v2/auth";

        var queryParams = new Dictionary<string, string>
        {
            { "client_id", ClientId },
            { "redirect_uri", redirectUrl },
            { "response_type", "code" },
            { "scope", scope },
            { "code_challenge", codeChallenge },
            { "code_challenge_method", "S256" }
        };
        var url = QueryHelpers.AddQueryString(oAuthEndpoint, queryParams);
        
        return url;
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
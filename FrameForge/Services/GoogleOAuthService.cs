using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;
using ServiceContracts;
using ServiceContracts.Helpers;

namespace Services;

public class GoogleOAuthService : IGoogleOAuthService
{
    private const string ClientId = "93167673295-e3ud1ef5u8s9f5fvlrpgpe15eqkcnblh.apps.googleusercontent.com";
    private const string ClientSecret = "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";
    private TokenResult tokenResult;
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

    public async Task<TokenResult> ExchangeCodeOnToken(string code, string codeVerifier, string redirectUrl)
    {
        var tokenEndpoint = $"https://oauth2.googleapis.com/token";

        var authParams = new Dictionary<string, string>
        {
            { "client_id", ClientId },
            { "client_secret", ClientSecret },
            { "code", code },
            { "code_verifier", codeVerifier },
            { "grant_type", "authorization_code" },
            { "redirect_uri", redirectUrl },
            {"Content-Type", "application/x-www-form-urlencoded" }
        };
        
        tokenResult = await HttpClientHelper.SendPostRequest<TokenResult>(tokenEndpoint, authParams);
        return tokenResult;
    }

    public object RefreshToke(string refreshToken)
    {
        throw new NotImplementedException();
    }

    public async Task<string> GetUserInfo(string accessToken)
    {
        string UserInfoEndpoint = "https://www.googleapis.com/oauth2/v2/userinfo";
        var request = new HttpRequestMessage(HttpMethod.Get, UserInfoEndpoint);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        
        HttpClient _httpClient = new HttpClient();
        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error getting user info: {response.StatusCode}");
        }

        var jsonResponse = await response.Content.ReadAsStringAsync();
        // var str = JsonSerializer.Deserialize<string>(jsonResponse);
        
        return jsonResponse;
    }
}
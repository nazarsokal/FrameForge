using System.Text;
using System.Text.Json;
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

    public async Task<TokenResult> ExchangeCodeOnToken(string code, string codeVerifier)
    {
        var tokenEndpoint = $"https://oauth2.googleapis.com/token";

        var authParams = new Dictionary<string, string>
        {
            { "client_id", ClientId },
            { "client_secret", ClientSecret },
            { "code", code },
            { "code_verifier", codeVerifier },
            { "grant_type", "authorization_code" },
            { "redirect_uri", tokenEndpoint }
        };
        
        var tokenResult = await SendPostRequestAsync<TokenResult>(tokenEndpoint, authParams);
        return tokenResult;
    }

    public object RefreshToke(string refreshToken)
    {
        throw new NotImplementedException();
    }
    
    private static async Task<TokenResult> SendPostRequestAsync<T>(string url, Dictionary<string, string> data)
    {
        using (HttpClient httpClient = new HttpClient()) // Creating HttpClient directly
        {
            try
            {
                string jsonData = JsonSerializer.Serialize(data);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<TokenResult>(responseBody, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                else
                {
                    throw new Exception($"Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Exception: {ex.Message}");
            }
        }
    }
}
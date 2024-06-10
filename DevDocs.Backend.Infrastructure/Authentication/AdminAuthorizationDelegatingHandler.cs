using System.Net.Http.Headers;
using System.Net.Http.Json;
using DevDocs.Backend.Infrastructure.Authentication.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

namespace DevDocs.Backend.Infrastructure.Authentication;

internal sealed class AdminAuthorizationDelegatingHandler : DelegatingHandler
{
    private readonly KeycloakOptions _keycloakOptions;

    public AdminAuthorizationDelegatingHandler(IOptions<KeycloakOptions> keycloakOptions)
    {
        _keycloakOptions = keycloakOptions.Value;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var authorizationToken = await GetAuthorizationToken(cancellationToken);

        request.Headers.Authorization = new AuthenticationHeaderValue(
            JwtBearerDefaults.AuthenticationScheme,
            authorizationToken.AccessToken);

        var httpResponseMessage = await base.SendAsync(request, cancellationToken);

        if (!httpResponseMessage.IsSuccessStatusCode)
        {
            var errorContent = await httpResponseMessage.Content.ReadAsStringAsync();
            Console.WriteLine($"Error Content: {errorContent}");
        }

        httpResponseMessage.EnsureSuccessStatusCode();

        return httpResponseMessage;
    }

    private async Task<AuthorizationToken> GetAuthorizationToken(CancellationToken cancellationToken)
    {
        var authorizationRequestParameters = new KeyValuePair<string, string>[]
        {
            new("client_id", _keycloakOptions.AdminClientId),
            new("client_secret", _keycloakOptions.AdminClientSecret),
            new("scope", "openid email"),
            new("grant_type", "client_credentials")
        };

        var authorizationRequestContent = new FormUrlEncodedContent(authorizationRequestParameters);

        using var authorizationRequest = new HttpRequestMessage(
            HttpMethod.Post,
            new Uri(_keycloakOptions.TokenUrl))
        {
            Content = authorizationRequestContent
        };

        var authorizationResponse = await base.SendAsync(authorizationRequest, cancellationToken);

        if (!authorizationResponse.IsSuccessStatusCode)
        {
            var error = await authorizationResponse.Content.ReadAsStringAsync();
            throw new ApplicationException($"Failed to acquire token: {error}");
        }

        return await authorizationResponse.Content.ReadFromJsonAsync<AuthorizationToken>(cancellationToken) ??
               throw new ApplicationException("Failed to deserialize token response.");
    }
}

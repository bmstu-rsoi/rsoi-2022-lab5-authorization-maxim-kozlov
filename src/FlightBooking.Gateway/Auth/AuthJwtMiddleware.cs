using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace FlightBooking.Gateway.Auth;

public class AuthJwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly JwtConfiguration _jwtConfiguration;
    private readonly ConfigurationManager<OpenIdConnectConfiguration> _configurationManager;
    private readonly ILogger<AuthJwtMiddleware> _logger;
    
    public AuthJwtMiddleware(RequestDelegate next,
        IOptions<JwtConfiguration> jwtConfiguration,
        ILogger<AuthJwtMiddleware> logger)
    {
        _next = next;
        _jwtConfiguration = jwtConfiguration.Value;
        _logger = logger;
        
        _configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
            _jwtConfiguration.Issuer + "/.well-known/openid-configuration",
            new OpenIdConnectConfigurationRetriever(),
            new HttpDocumentRetriever());
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (!string.IsNullOrWhiteSpace(token))
                await AttachUserToContextAsync(context, token);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error in middleware {middleware}.", nameof(AuthJwtMiddleware));
        }
        
        await _next(context);
    }

    private async Task AttachUserToContextAsync(HttpContext context, string token, CancellationToken cancellationToken = default)
    {
        var discoveryDocument = await _configurationManager.GetConfigurationAsync(cancellationToken);
        var signingKeys = discoveryDocument.SigningKeys;
        
        var validationParameters = new TokenValidationParameters
        {
            RequireExpirationTime = true,
            RequireSignedTokens = true,
            ValidateIssuer = true,
            ValidIssuer = _jwtConfiguration.Issuer,
            ValidateIssuerSigningKey = true,
            IssuerSigningKeys = signingKeys,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(2),
            ValidateAudience = false,
        };
        
        var tokenHandler = new JwtSecurityTokenHandler();
        
        try
        {
            if (tokenHandler.CanReadToken(token))
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                context.User = principal;
            }
        }
        catch (SecurityTokenException e)
        {
            _logger.LogInformation(e, "Authorization Jwt token: {token} is invalid.", token);
        }
    }
}
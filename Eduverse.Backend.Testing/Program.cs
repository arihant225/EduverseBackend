using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

// Retrieve the token from the request or any other source
string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IkFrYW5zaGEgU29sYW5raSIsImVtYWlsIjoiamFyaWhhbnQyMjVAZ21haWwuY29tIiwiUGhvbmUiOiI3MDI0ODU3MjM3Iiwicm9sZSI6InNlbGYiLCJuYmYiOjE2ODc2OTg2MzAsImV4cCI6MTY4NzY5OTIzMCwiaWF0IjoxNjg3Njk4NjMwLCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjUxMTQvIn0.DR5JpEs3hxbXK3rLfvssLZyzN5h-i6AvI_HbYspfi-A";

// Set the signing key
string signingKey = "hhy4hyqt635t6g3t6gyuew73673yhfreheuhiuhuh4737y5hi2ujekkfcnkjn37487895";
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));

// Configure token validation parameters
var tokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuer = false,
    ValidateAudience = false,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = false,
    IssuerSigningKey = key
};

try
{
    // Validate the token
    var claimsPrincipal = new JwtSecurityTokenHandler().ValidateToken(token, tokenValidationParameters, out var validatedToken);
    // Token is valid
    // Access claims using claimsPrincipal.Claims
    // You can also retrieve specific claim values like claimsPrincipal.FindFirst("claim_name")?.Value
}
catch (SecurityTokenValidationException ex)
{
    Console.WriteLine(ex);
}
catch (Exception ex)
{
    Console.WriteLine(ex);
}

using Microsoft.IdentityModel.Tokens;
using POD.Models;
using System;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace PODWebAPI.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool IsAuthorized(
            HttpActionContext actionContext)
        {
            try
            {
                string authHeader =
                    HttpContext.Current.Request.Headers["Authorization"];

                if (string.IsNullOrWhiteSpace(authHeader))
                    return false;

                if (!authHeader.StartsWith(
                    "Bearer ",
                    StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }

                string token = authHeader.Substring(7).Trim();

                if (string.IsNullOrWhiteSpace(token))
                    return false;

                LoginResponse sessionData =ValidateToken(token);

                if (sessionData == null)
                    return false;

                // Store validated user
                HttpContext.Current.Items["SessionData"] = sessionData;

                return true;
            }
            catch
            {
                return false;
            }
        }

        private LoginResponse ValidateToken(string token)
        {
            try
            {
                string secretKey =
                    ConfigurationManager.AppSettings["JwtSecretKey"];

                string issuer =
                    ConfigurationManager.AppSettings["JwtIssuer"];

                string audience =
                    ConfigurationManager.AppSettings["JwtAudience"];

                if (string.IsNullOrWhiteSpace(secretKey) ||
                    string.IsNullOrWhiteSpace(issuer) ||
                    string.IsNullOrWhiteSpace(audience))
                {
                    return null;
                }

                var key = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(secretKey)
                );

                var validationParameters =
                    new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = key,

                        ValidateIssuer = true,
                        ValidIssuer = issuer,

                        ValidateAudience = true,
                        ValidAudience = audience,

                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };

                var tokenHandler =
                    new JwtSecurityTokenHandler();

                SecurityToken validatedToken;

                ClaimsPrincipal principal =
                    tokenHandler.ValidateToken(
                        token,
                        validationParameters,
                        out validatedToken
                    );

                string userId =
                    principal.FindFirst(
                        ClaimTypes.NameIdentifier)?.Value;

                string userName =
                    principal.FindFirst(
                        ClaimTypes.Name)?.Value;

                string email =
                    principal.FindFirst(
                        ClaimTypes.Email)?.Value;

                string role =
                    principal.FindFirst(
                        ClaimTypes.Role)?.Value;

                if (string.IsNullOrWhiteSpace(userId))
                    return null;

                return new LoginResponse
                {
                    Userid = userId,
                    UserName = userName,
                    Email = email,
                    Role = role
                };
            }
            catch
            {
                return null;
            }
        }
    }
}
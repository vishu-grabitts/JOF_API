using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace PODWebAPI.Filters
{
    public class TokenAuthorizeAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(
            HttpActionContext actionContext)
        {
            try
            {
                string authorization =
                    actionContext.Request.Headers.Authorization?.ToString();

                if (string.IsNullOrWhiteSpace(authorization))
                {
                    SetUnauthorizedResponse(
                        actionContext,
                        "Authorization token is required.");

                    return;
                }

                string token = ExtractToken(authorization);

                string configuredToken =
                    ConfigurationManager.AppSettings["ApiToken"];

                if (string.IsNullOrWhiteSpace(configuredToken))
                {
                    SetUnauthorizedResponse(
                        actionContext,
                        "API token is not configured.");

                    return;
                }

                if (!string.Equals(
                        token,
                        configuredToken,
                        StringComparison.Ordinal))
                {
                    SetUnauthorizedResponse(
                        actionContext,
                        "Invalid authorization token.");

                    return;
                }
            }
            catch
            {
                SetUnauthorizedResponse(
                    actionContext,
                    "Unauthorized.");
            }
        }

        private string ExtractToken(string authorization)
        {
            // Expected:
            // Authorization: token: abc123

            if (authorization.StartsWith(
                    "token:",
                    StringComparison.OrdinalIgnoreCase))
            {
                return authorization
                    .Substring(6)
                    .Trim();
            }

            return string.Empty;
        }

        private void SetUnauthorizedResponse(
            HttpActionContext actionContext,
            string message)
        {
            actionContext.Response =
                actionContext.Request.CreateResponse(
                    HttpStatusCode.Unauthorized,
                    new
                    {
                        success = false,
                        message = message
                    });
        }
    }
}
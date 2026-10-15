using Microsoft.IdentityModel.Tokens;
using POD.DAL;
using POD.Logics;
using POD.Models;
using POD.Models.POD.Models;
using System;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;

namespace PODWebAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api")]
    public class LoginController : ApiController
    {
        Output output = new Output();

        [HttpPost]
        [Route("Login")]
        public HttpResponseMessage Login([FromBody] LoginRequest parameters)
        {
            try
            {
                LoginUserModel user;
                JwtTokenResponse tokenResponse;
                LoginResponse sessionData;

                // Validate request
                if (parameters == null)
                {
                    output.ResponseCode = 400;
                    output.Message = "Invalid request.";
                    output.Data = null;

                    return Request.CreateResponse(
                        HttpStatusCode.BadRequest,
                        output
                    );
                }
                if (string.IsNullOrWhiteSpace(parameters.Username))
                {
                    output.ResponseCode = 400;
                    output.Message = "Username is required.";
                    output.Data = null;

                    return Request.CreateResponse(
                        HttpStatusCode.BadRequest,
                        output
                    );
                }

               if (string.IsNullOrWhiteSpace(parameters.Password))
                {
                    output.ResponseCode = 400;
                    output.Message = "Password is required.";
                    output.Data = null;

                    return Request.CreateResponse(
                        HttpStatusCode.BadRequest,
                        output
                    );
                }

               LoginBLL objBLL = new LoginBLL();
                user = objBLL.ValidateUser(parameters.Username,parameters.Password
                );
                if (user == null)
                {
                    output.ResponseCode = 401;
                    output.Message = "Invalid username or password.";
                    output.Data = null;
                }
                else
                {
                    tokenResponse = GenerateJwtToken(user);
                    sessionData = new LoginResponse
                    {
                        Token = tokenResponse.Token,
                        TokenExpireOn = tokenResponse.ExpireOn,
                   };


                    output.ResponseCode = 200;
                    output.Message = "Login successful.";
                    output.Data = sessionData;
                }


                return Request.CreateResponse(HttpStatusCode.OK,output);
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);

                output.ResponseCode = 500;
                output.Message = ex.Message;
                output.Data = null;
                return Request.CreateResponse(
                    HttpStatusCode.InternalServerError,
                    output
                );
            }
        }


        private JwtTokenResponse GenerateJwtToken(
            LoginUserModel user)
        {
            string secretKey =
                ConfigurationManager.AppSettings["JwtSecretKey"];

            string issuer =
                ConfigurationManager.AppSettings["JwtIssuer"];

            string audience =
                ConfigurationManager.AppSettings["JwtAudience"];


            if (string.IsNullOrWhiteSpace(secretKey))
            {
                throw new Exception(
                    "JwtSecretKey is not configured."
                );
            }


            DateTime expireOn =DateTime.UtcNow.AddHours(8);

            var claims = new[]
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    user.UserId.ToString()
                ),

                new Claim(
                    ClaimTypes.Name,
                    user.Username ?? ""
                ),

                new Claim(
                    ClaimTypes.Email,
                    user.Email ?? ""
                ),

                new Claim(
                    ClaimTypes.Role,
                    user.Role ?? ""
                )
            };


            var key =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(secretKey)
                );


            var credentials =
                new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256
                );


            var token =
                new JwtSecurityToken(
                    issuer: issuer,
                    audience: audience,
                    claims: claims,
                    expires: expireOn,
                    signingCredentials: credentials
                );


            string tokenString =
                new JwtSecurityTokenHandler()
                .WriteToken(token);


            return new JwtTokenResponse
            {
                Token = tokenString,
                ExpireOn = expireOn
            };
        }
        public class JwtTokenResponse
        {
            public string Token { get; set; }

            public DateTime ExpireOn { get; set; }
        }
    }
}
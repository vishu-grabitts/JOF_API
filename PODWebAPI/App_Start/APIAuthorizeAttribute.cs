using POD.Logics;
using PODWebAPI.AES256Encryption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace PODWebAPI
{
    public class APIAuthorizeAttribute : AuthorizeAttribute
    {
        private string _reason = "";
        public override void OnAuthorization(HttpActionContext filterContext)
        {
            if (Authorize(filterContext))
            {
                return;
            }
            HandleUnauthorizedRequest(filterContext);
        }
        protected override void HandleUnauthorizedRequest(HttpActionContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);
            filterContext.Response = filterContext.Request.CreateResponse(HttpStatusCode.Forbidden);
            if (!string.IsNullOrEmpty(_reason))
            {
                Output output = new Output();
                output.ResponseCode = 403;
                output.Message = _reason;
                output.Data = null;
                filterContext.Response = filterContext.Request.CreateResponse(HttpStatusCode.OK, output);
            }
        }

        private bool Authorize(HttpActionContext actionContext)
        {
            try
            {
                _reason = "";
                var encodedString = actionContext.Request.Headers.GetValues("Token").First();
                bool validFlag = false;

                if (!string.IsNullOrEmpty(encodedString))
                {
                    var key = EncryptionLibrary.DecryptText(encodedString);
                    string[] parts = key.Split(new char[] { ':' });

                    //var profileid = Convert.ToInt32(parts[0]);       // profileid
                    //var RandomKey = parts[1];                     // Random Key
                    long ticks = long.Parse(parts[0]);            // Ticks
                    DateTime IssuedOn = (new DateTime(1970, 1, 1)).AddMilliseconds(ticks);// new DateTime(ticks);

                    //TokenBLL tokenBLL = new TokenBLL();
                    //Token token = tokenBLL.GetToken(profileid);

                    //if (encodedString != token.tokenkey)
                    //{
                    //    _reason = "Invalid Token";
                    //    validFlag = false;
                    //}
                    //else 

                    if ((DateTime.UtcNow < IssuedOn.AddHours(1)))
                    {
                        validFlag = true;
                    }
                    else
                    {
                        _reason = "Token Expired";
                        validFlag = false;

                    }
                }
                return validFlag;
            }
            catch (Exception ex)
            {
                _reason = "Invalid Request";
                //  Helper.LogException(ex, "APIAUthorizeAttribute_Authorize");
                return false;
            }
        }
    }
}
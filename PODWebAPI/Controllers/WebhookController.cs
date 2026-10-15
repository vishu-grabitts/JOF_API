using POD.DAL;
using POD.Logics;
using POD.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace PODWebAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/Webhook")]
    [APIAuthorizeAttribute]
    public class WebhookController : ApiController
    {
        Output output = new Output();
        private readonly WebhookBLL _webhookBLL;

        public WebhookController()
        {
            _webhookBLL = new WebhookBLL();
        }
        [HttpPost]
        [Route("postWebhook")]
        public async Task<HttpResponseMessage> PostWebhook(Webhookmodel requestModel)
        {            
            try
            {                              
                var result = await _webhookBLL.PostWebhook(requestModel);
                if (result != null && result.Count > 0)
                {
                    output.ResponseCode = 100;
                    output.Message = "Success";
                    output.Data = result;              
                }
                else
                {
                    output.ResponseCode = 500;
                    output.Message = "Failed.";
                    output.Data = null;
                }
                return Request.CreateResponse(HttpStatusCode.OK, output);
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
                output.ResponseCode = 500;
                output.Message = ex.Message.ToString();
                output.Data = null;
                return Request.CreateResponse(HttpStatusCode.InternalServerError, output);
            }          
        }
    }
}
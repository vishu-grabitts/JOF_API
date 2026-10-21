
using Newtonsoft.Json;
using POD.DAL;
using POD.Logics;
using POD.Models;
using POD.Models.RoutePlan;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace PODWebAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/RoutePlan")]
  
    public class RoutePlanController : ApiController
    {

        [HttpGet]
        [Route("GetCustomerDetails")]
        public HttpResponseMessage GetCustomerDetails()
        {
            try
            {
                var objBLL = new RoutePlanBLL();

                var response = new GetCustomerDetailsResponseVM
                {
                    StatusCode = 0,
                    StatusMessage = "Success",
                    ResponseObject = objBLL.GetCustomerDetails_BLL()
                };

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);

                var response = new GetCustomerDetailsResponseVM
                {
                    StatusCode = 500,
                    StatusMessage = "Failed to retrieve customer details.",
                    ResponseObject = null
                };

                return Request.CreateResponse(
                    HttpStatusCode.InternalServerError,
                    response);
            }
        }



        [HttpPost]
        [Route("SyncCustomers")]
        public HttpResponseMessage SyncCustomers(
            [FromBody] SyncCustomerRequestVM request)
        {
            try
            {
                var objBLL = new RoutePlanBLL();

                var result = objBLL.SyncCustomers_BLL(request);

                var response = new
                {
                    StatusCode = 0,
                    result
                };

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (ArgumentException ex)
            {
                var response = new
                {
                    StatusCode = 400,
                    StatusMessage = ex.Message
                };

                return Request.CreateResponse(
                    HttpStatusCode.BadRequest, response);
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);

                var response = new
                {
                    StatusCode = 500,
                    StatusMessage = "Failed to synchronize customer records."
                };

                return Request.CreateResponse(
                    HttpStatusCode.InternalServerError, response);
            }
        }



    }
}

using POD.Logics;
using POD.Models;
using System;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PODWebAPI.Controllers
{
    [RoutePrefix("DPOS/api")]
    public class ExternalVoucherController : CustomBaseController
    {
        private readonly ExternalVoucherBLL _bll;

        public ExternalVoucherController()
        {
            _bll = new ExternalVoucherBLL();
        }

        [HttpPost]
        [Route("Savevoucher")]
        public HttpResponseMessage SubmitSalesOrder(
            [FromBody] SalesOrderRequest request)
        {
            try
            {
                if (request == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest,new {
                            success = false,
                            message = "Validation failed."
                        });
                }

                VoucherResponse result = _bll.SubmitSalesOrder(request);

                if (result.Success)
                {
                    return Request.CreateResponse(
                        HttpStatusCode.OK,
                        result);
                }

                if (result.Message == "Duplicate voucher.")
                {
                    return Request.CreateResponse(
                        HttpStatusCode.Conflict,
                        result);
                }

                return Request.CreateResponse(
                    HttpStatusCode.BadRequest,
                    result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(
                    HttpStatusCode.InternalServerError,
                    new
                    {
                        success = false,
                        message = ex.Message
                    });
            }
        }
    }
}
using POD.Logics;
using POD.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Linq;
using System.Drawing;
using System.Threading.Tasks;
using System.Threading;
using System.Configuration;
using Newtonsoft.Json;
using System.Web;
using POD.DAL;
using POD.Models.Customer;

namespace PODWebAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/Customer")]
    [APIAuthorizeAttribute]
    public class CustomerController : ApiController
    {
        Output output = new Output();
        [HttpGet]
        [Route("GetCompanyDetail")]
        public HttpResponseMessage GetCompanyDetail(string companyseries)
        {
            var objBLL = new CustomerBLL();
            try
            {
                    output.ResponseCode = 100;
                    output.Message = "Success";
                    output.Data = objBLL.GetCompanyDetail_BLL(companyseries);
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


        #region user authontication
        [HttpPost]
        [Route("CustomerLogin")]
        public HttpResponseMessage CustomerLogin([FromBody] CustomerLoginVM model)
        {
            var objBLL = new CustomerBLL();
            try
            {
                var data = objBLL.CustomerLogin_BLL(model);
                if (data.Count > 0)
                {
                    output.ResponseCode = 100;
                    output.Message = "Success";
                    output.Data = data;
                }
                else
                {
                    output.ResponseCode = 500;
                    output.Message = "Not a valid user.";
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
            //return Request.CreateResponse(HttpStatusCode.OK, output);
        }
         

        [HttpPost]
        [Route("RegisterCustomer")]
        public HttpResponseMessage RegisterCustomer([FromBody] RegisterCustomerVM model)
        {
            var objBLL = new CustomerBLL();
            try
            {
                var data = objBLL.RegisterCustomer_BLL(model);
                if (data > 0)
                {
                    output.ResponseCode = 100;
                    output.Message = "Success";
                    output.Data = data;
                }
                else
                {
                    output.ResponseCode = 500;
                    output.Message = "Failed to create customer!";
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
            //return Request.CreateResponse(HttpStatusCode.OK, output);
        }




        [HttpGet]
        [Route("SendEmailForVerifyEmailId")]
        public HttpResponseMessage SendEmailForVerifyEmailId(string email, bool isCheckDuplicate, int companyid)
        { 
            var objBLL = new CustomerBLL();
            try
            {
                output.ResponseCode = 100;
                output.Message = "Success";
                output.Data = objBLL.SendEmailForVerifyEmailId_BLL(email, isCheckDuplicate, companyid);
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



        [HttpGet]
        [Route("SendSMSForVerifyMobile")]
        public HttpResponseMessage SendSMSForVerifyMobile(string mobile, bool isCheckDuplicate, int companyid)
        {
            var objBLL = new CustomerBLL();
            try
            {
                output.ResponseCode = 100;
                output.Message = "Success";
                output.Data = objBLL.SendSMSForVerifyMobile_BLL(mobile, isCheckDuplicate, companyid);
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




        //[HttpPost]
        //[Route("forgotpassword")]
        //public HttpResponseMessage ForgotPassword([FromBody] UserData objUA)
        //{
        //    UserAccountBLL UserAccount = new UserAccountBLL();
        //    try
        //    {
        //        int affectedRow = UserAccount.ForgotPassword_BLL(objUA.loginid);
        //        if (affectedRow > 0)
        //        {
        //            output.ResponseCode = 100;
        //            output.Message = "Success";
        //            output.Data = affectedRow;
        //        }
        //        else
        //        {
        //            output.ResponseCode = 500;
        //            output.Message = "Not a valid user.";
        //            output.Data = null;
        //        }
        //        return Request.CreateResponse(HttpStatusCode.OK, output);
        //    }
        //    catch (Exception ex)
        //    {
        //        Helper.LogException(ex);
        //        output.ResponseCode = 500;
        //        output.Message = ex.Message.ToString();
        //        output.Data = null;
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, output);
        //    }
        //    //return Request.CreateResponse(HttpStatusCode.OK, output);
        //}


        //[HttpPost]
        //[Route("Updatelogouthistory")]
        //public HttpResponseMessage Updatelogouthistory(UserData objUA)
        //{
        //    UserAccountBLL UserAccount = new UserAccountBLL();
        //    try
        //    {
        //        bool isupdated = UserAccount.updatelogouthistory_BLL(objUA);
        //        if (isupdated)
        //        {
        //            output.ResponseCode = 100;
        //            output.Message = "Success";
        //            output.Data = isupdated;
        //        }
        //        else
        //        {
        //            output.ResponseCode = 500;
        //            output.Message = "Not a valid user.";
        //            output.Data = null;
        //        }
        //        return Request.CreateResponse(HttpStatusCode.OK, output);
        //    }
        //    catch (Exception ex)
        //    {
        //        Helper.LogException(ex);
        //        output.ResponseCode = 500;
        //        output.Message = ex.Message.ToString();
        //        output.Data = null;
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, output);
        //    }
        //    //return Request.CreateResponse(HttpStatusCode.OK, output);
        //}

        #endregion


    }

}

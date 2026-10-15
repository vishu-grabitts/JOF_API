using iText.Html2pdf;
using iText.Kernel.Pdf;
using iTextSharp.text;
using Newtonsoft.Json;
using POD.DAL;
using POD.Logics;
using POD.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace PODWebAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/POD")]
    [APIAuthorizeAttribute]
    public class PODController : ApiController
    {
        Output output = new Output();

        //[HttpGet]
        //public IHttpActionResult Test() {
        //    return Ok("Test");
        //}

        #region Comman Masters
        [HttpGet]
        [Route("getUserNotifications")]
        public HttpResponseMessage getUserNotifications(int empid)
        {
            MasterBLL objMasterBLL = new MasterBLL();
            try
            {
                var MasterDatalist = objMasterBLL.getUserNotifications_BLL(empid);
                if (MasterDatalist.Count > 0)
                {
                    output.ResponseCode = 100;
                    output.Message = "Success";
                    output.Data = MasterDatalist;
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
        }

        [HttpGet]
        [Route("UpdateUserNotificationRead")]
        public HttpResponseMessage UpdateUserNotificationRead(int empid)
        {
            MasterBLL objMasterBLL = new MasterBLL();
            try
            {
                output.ResponseCode = 100;
                output.Message = "Success";
                output.Data = objMasterBLL.UpdateUserNotificationRead_BLL(empid);
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
        [Route("GetUserNotificationCount")]
        public HttpResponseMessage GetUserNotificationCount(int empid)
        {
            MasterBLL objMasterBLL = new MasterBLL();
            try
            {
                output.ResponseCode = 100;
                output.Message = "Success";
                output.Data = objMasterBLL.GetUserNotificationCount_BLL(empid);
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
        [Route("getActionMaster")]
        public HttpResponseMessage getActionMaster()
        {
            MasterBLL objMasterBLL = new MasterBLL();
            try
            {
                List<ActionMaster> MasterDatalist = objMasterBLL.getActionMaster_BLL();
                if (MasterDatalist.Count > 0)
                {
                    output.ResponseCode = 100;
                    output.Message = "Success";
                    output.Data = MasterDatalist;
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
        }

        [HttpGet]
        [Route("getReasonMaster")]
        public HttpResponseMessage getReasonMaster(int companyid)
        {
            MasterBLL objMasterBLL = new MasterBLL();
            try
            {
                List<ReasonMaster> ReasonMasterlist = objMasterBLL.getReasonMaster_BLL(companyid);
                if (ReasonMasterlist.Count > 0)
                {
                    output.ResponseCode = 100;
                    output.Message = "Success";
                    output.Data = ReasonMasterlist;
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
        }


        [HttpGet]
        [Route("GetSysSettings")]
        public HttpResponseMessage GetSysSettings(int companyid, string key = null)
        {
            MasterBLL objMasterBLL = new MasterBLL();
            try
            {
                var ReasonMasterlist = objMasterBLL.GetSysSettings_BLL(companyid, key);
                if (ReasonMasterlist.Count > 0)
                {
                    output.ResponseCode = 100;
                    output.Message = "Success";
                    output.Data = ReasonMasterlist;
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
        }

        #endregion

        #region user authontication
        [HttpPost]
        [Route("UserLoginV2")]
        public HttpResponseMessage UserLoginV2([FromBody] UserData objUA)
        {
            UserAccountBLL UserAccount = new UserAccountBLL();

            try
            {
                List<UserData> Userdata =
                    UserAccount.GetUserLoginData_BLL(objUA);

                if (Userdata.Count > 0)
                {
                    output.ResponseCode = 100;
                    output.Message = "Success";

                    // ONLY JWT TOKEN IN RESPONSE
                    output.Data = new
                    {
                        jwttoken = Userdata[0].jwttoken
                    };
                }
                else
                {
                    output.ResponseCode = 500;
                    output.Message = "Not a valid user.";
                    output.Data = null;
                }

                return Request.CreateResponse(
                    HttpStatusCode.OK,
                    output
                );
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);

                output.ResponseCode = 500;
                output.Message = ex.Message.ToString();
                output.Data = null;

                return Request.CreateResponse(
                    HttpStatusCode.InternalServerError,
                    output
                );
            }
        }
        [HttpPost]

        [Route("UserLogin")]

        public HttpResponseMessage UserLogin([FromBody] UserData objUA)

        {

            UserAccountBLL UserAccount = new UserAccountBLL();

            try
            {

                List<UserData> Userdata = UserAccount.GetUserLoginData_BLL(objUA);

                if (Userdata.Count > 0)

                {

                    output.ResponseCode = 100;

                    output.Message = "Success";

                    output.Data = Userdata;

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
        [Route("forgotpassword")]
        public HttpResponseMessage ForgotPassword([FromBody] UserData objUA)
        {
            UserAccountBLL UserAccount = new UserAccountBLL();
            try
            {
                int affectedRow = UserAccount.ForgotPassword_BLL(objUA.loginid);
                if (affectedRow > 0)
                {
                    output.ResponseCode = 100;
                    output.Message = "Success";
                    output.Data = affectedRow;
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
        [Route("Updatelogouthistory")]
        public HttpResponseMessage Updatelogouthistory(UserData objUA)
        {
            UserAccountBLL UserAccount = new UserAccountBLL();
            try
            {
                bool isupdated = UserAccount.updatelogouthistory_BLL(objUA);
                if (isupdated)
                {
                    output.ResponseCode = 100;
                    output.Message = "Success";
                    output.Data = isupdated;
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
        [Route("UpdateUser")]
        public HttpResponseMessage UpdateUser(UserData objUA)
        {
            UserAccountBLL UserAccount = new UserAccountBLL();
            try
            {
                bool isupdated = UserAccount.updateuser_BLL(objUA);
                if (isupdated)
                {
                    output.ResponseCode = 100;
                    output.Message = "Success";
                    output.Data = isupdated;
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
        [Route("UpdatePic")]
        public HttpResponseMessage UpdatePic(UserData objUA)
        {
            UserAccountBLL UserAccount = new UserAccountBLL();
            try
            {
                string signaturepath = UserAccount.updatepic_BLL(objUA);
                if (signaturepath != null)
                {
                    output.ResponseCode = 100;
                    output.Message = "Success";
                    output.Data = signaturepath;
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
        [Route("UpdateEmail")]
        public HttpResponseMessage UpdateEmail(UserData objUA)
        {
            UserAccountBLL UserAccount = new UserAccountBLL();
            try
            {
                bool isupdated = UserAccount.updateemail_BLL(objUA);
                if (isupdated)
                {
                    output.ResponseCode = 100;
                    output.Message = "Success";
                    output.Data = isupdated;
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
        [Route("SendOTPEmail")]
        public HttpResponseMessage SendOTPEmail(UserData objUA, string langcode)
        {
            MasterBLL UserAccount = new MasterBLL();
            try
            {
                DataTable dtMailConfig = UserAccount.getMailConfigBLL(objUA.companyid, "otp", langcode, null);
                if (dtMailConfig.Rows.Count > 0)
                {
                    NotificationBLL notificationBLL = new NotificationBLL();
                    int otp = GenerateOTP();
                    string message = dtMailConfig.Rows[0]["body"].ToString().Trim();
                    string Body = message.Replace("@otp", otp.ToString());
                    string Subject = dtMailConfig.Rows[0]["subject"].ToString();

                    bool sent = notificationBLL.SendEmail(objUA.companyid, Subject, Body, objUA.emailid, string.Empty, string.Empty);
                    if (sent)
                    {

                        output.ResponseCode = 100;
                        output.Message = "Success";
                        output.Data = otp;
                    }
                    else
                    {
                        output.ResponseCode = 500;
                        output.Data = null;
                    }
                }
                else
                {
                    output.ResponseCode = 500;
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

        [HttpPost]
        [Route("UpdateAbout")]
        public HttpResponseMessage UpdateAbout(UserData objUA)
        {
            UserAccountBLL UserAccount = new UserAccountBLL();
            try
            {
                bool isupdated = UserAccount.updateabout_BLL(objUA);
                if (isupdated)
                {
                    output.ResponseCode = 100;
                    output.Message = "Success";
                    output.Data = isupdated;
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
        [Route("UpdateContact")]
        public HttpResponseMessage UpdateContact(UserData objUA)
        {
            UserAccountBLL UserAccount = new UserAccountBLL();
            try
            {
                bool isupdated = UserAccount.updatecontact_BLL(objUA);
                if (isupdated)
                {
                    output.ResponseCode = 100;
                    output.Message = "Success";
                    output.Data = isupdated;
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
        [Route("UpdateAddress")]
        public HttpResponseMessage UpdateAddress(EmployeeAddress objUA)
        {
            UserAccountBLL UserAccount = new UserAccountBLL();
            try
            {
                bool isupdated = UserAccount.updateaddress_BLL(objUA);
                if (isupdated)
                {
                    output.ResponseCode = 100;
                    output.Message = "Success";
                    output.Data = isupdated;
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

        [HttpGet]
        [Route("GetEmployeeAddress")]
        public HttpResponseMessage GetEmployeeAddress(string empcode)
        {
            UserAccountBLL userAccountBLL = new UserAccountBLL();
            try
            {
                var obj = userAccountBLL.GetEmployeeAddress_BLL(empcode);
                if (obj.Count > 0)
                {
                    output.ResponseCode = 100;
                    output.Message = "Success";
                    output.Data = obj;
                }
                else
                {
                    output.ResponseCode = 500;
                    output.Message = "No data";
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

        [HttpGet]
        [Route("DeleteEmployeeAddress")]
        public HttpResponseMessage Deleteidproof(int id)
        {
            UserAccountBLL userJobRequestBLL = new UserAccountBLL();
            try
            {
                bool isdeleted = userJobRequestBLL.DeleteEmployeeAddress_BLL(id);
                if (isdeleted)
                {
                    output.ResponseCode = 100;
                    output.Message = "Success";
                    output.Data = isdeleted;
                }
                else
                {
                    output.ResponseCode = 500;
                    output.Message = "Failed to delete.";
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


        [HttpPost]
        [Route("Updateloginhistory")]
        public HttpResponseMessage Updateloginhistory(UserData objUA)
        {
            UserAccountBLL UserAccount = new UserAccountBLL();
            try
            {
                bool isupdated = UserAccount.updateloginhistory_BLL(objUA);
                if (isupdated)
                {
                    output.ResponseCode = 100;
                    output.Message = "Success";
                    output.Data = isupdated;
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
        [Route("Saveidproof")]
        public HttpResponseMessage Saveidproof(IdProof objimage)
        {
            UserAccountBLL userJobRequestBLL = new UserAccountBLL();
            try
            {
                string signaturepath = userJobRequestBLL.saveidproof_BLL(objimage);
                if (signaturepath != null)
                {
                    output.ResponseCode = 100;
                    output.Message = "Success";
                    output.Data = signaturepath;
                }
                else
                {
                    output.ResponseCode = 500;
                    output.Message = "Signature not saved";
                    output.Data = null;
                }
                return Request.CreateResponse(HttpStatusCode.OK, output);
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
                output.ResponseCode = 500;
                output.Message = ex.ToString();
                output.Data = null;
                return Request.CreateResponse(HttpStatusCode.InternalServerError, output);
            }
        }

        [HttpGet]
        [Route("Getidproof")]
        public HttpResponseMessage GetIdProof(string empcode)
        {
            UserAccountBLL userAccountBLL = new UserAccountBLL();
            try
            {
                List<IdProof> lstidproof = userAccountBLL.getidproof_BLL(empcode);
                if (lstidproof.Count > 0)
                {
                    output.ResponseCode = 100;
                    output.Message = "Success";
                    output.Data = lstidproof;
                }
                else
                {
                    output.ResponseCode = 500;
                    output.Message = "No data";
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

        [HttpPost]
        [Route("Deleteidproof")]
        public HttpResponseMessage Deleteidproof(IdProof objimage)
        {
            UserAccountBLL userJobRequestBLL = new UserAccountBLL();
            try
            {
                bool isdeleted = userJobRequestBLL.deleteidproof_BLL(objimage);
                if (isdeleted)
                {
                    output.ResponseCode = 100;
                    output.Message = "Success";
                    output.Data = isdeleted;
                }
                else
                {
                    output.ResponseCode = 500;
                    output.Message = "SIgnature no saved.";
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

        [HttpGet]
        [Route("GetUserBranchData")]
        public HttpResponseMessage GetUserBranchData(int userid)
        {
            UserAccountBLL UserAccount = new UserAccountBLL();
            try
            {
                List<BranchData> BranchData = UserAccount.GetUserBranchData_BLL(userid);
                if (BranchData.Count > 0)
                {
                    output.ResponseCode = 100;
                    output.Message = "Success";
                    output.Data = BranchData;
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
        }

        [HttpGet]
        [Route("GetUserBranches")]
        public HttpResponseMessage GetUserBranches(int roleid, int userid)
        {
            UserAccountBLL UserAccount = new UserAccountBLL();
            try
            {
                List<BranchData> BranchData = UserAccount.GetUserBranches_BLL(roleid, userid);
                if (BranchData.Count > 0)
                {
                    output.ResponseCode = 100;
                    output.Message = "Success";
                    output.Data = BranchData;
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
        }
        #endregion

        #region Jobrequests
        [HttpGet]
        [Route("GetCounter")]
        public HttpResponseMessage GetCounter(int userid, string branchcode, int empid, int roleid)
        {
            JobRequestBLL userJobRequestBLL = new JobRequestBLL();
            try
            {
                List<dashboardcounter> objgetcounter = userJobRequestBLL.GetCounter_BLL(userid, branchcode, empid, roleid);
                if (objgetcounter.Count > 0)
                {
                    output.ResponseCode = 100;
                    output.Message = "Success";
                    output.Data = objgetcounter;
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
        }

        [HttpGet]
        [Route("GetUserjobrequest")]
        public HttpResponseMessage GetUserjobrequest(int userid, int branchid, int empid)
        {
            JobRequestBLL userJobRequestBLL = new JobRequestBLL();
            try
            {
                List<JobRequest> JobRequestData = userJobRequestBLL.getJobrequest_BLL(userid, branchid, empid);
                if (JobRequestData.Count > 0)
                {
                    output.ResponseCode = 100;
                    output.Message = "Success";
                    output.Data = JobRequestData;
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
        }

        [HttpGet]
        [Route("GetUserjobrequestData")]
        public HttpResponseMessage GetUserjobrequestData(int roleid, int userid, string branchcode, int empid, string type, long tagno)
        {
            JobRequestBLL userJobRequestBLL = new JobRequestBLL();
            try
            {
                List<JobRequest> JobRequestData = userJobRequestBLL.getJobrequestData_BLL(roleid, userid, branchcode, empid, type, tagno);
                //if (JobRequestData.Count > 0)
                //{
                output.ResponseCode = 100;
                output.Message = "Success";
                output.Data = JobRequestData;
                //}
                //else
                //{
                //    output.ResponseCode = 500;
                //    output.Message = "There is no data for user/branch.";
                //    output.Data = null;
                //}
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
        [Route("GetUserOfflineData")]
        public HttpResponseMessage GetUserOfflineData(int roleid, int userid, string branchcode, int empid, string type)
        {
            JobRequestBLL userJobRequestBLL = new JobRequestBLL();
            try
            {
                List<JobRequest> JobRequestData = userJobRequestBLL.getUserOfflineData_BLL(roleid, userid, branchcode, empid, type);
                if (JobRequestData.Count > 0)
                {
                    output.ResponseCode = 100;
                    output.Message = "Success";
                    output.Data = JobRequestData;
                }
                else
                {
                    output.ResponseCode = 500;
                    output.Message = "There is no data for user/branch.";
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


        //Not In Use
        //[HttpPost]
        //[Route("SaveJobActionMultiple")]
        //public HttpResponseMessage SaveJobActionMultiple(int userid, [FromBody] JobRequest objjobreq)
        //{
        //    JobRequestBLL userJobRequestBLL = new JobRequestBLL();
        //    try
        //    {
        //        List<JobRequest> JobRequestData = userJobRequestBLL.SaveJobAction(userid, objjobreq);
        //        if (JobRequestData != null)
        //        {
        //            if (objjobreq.jhstatusid == 3 || objjobreq.jhstatusid == 5)
        //            {
        //                NotificationBLL objNotificationBLL = new NotificationBLL();
        //                Notification objNotification = new Notification();
        //                objNotification.companyid = 1;
        //                objNotification.actionid = objjobreq.jhstatusid;

        //                bool issent = objNotificationBLL.SendnotificationByAction_BLL(objjobreq, objNotification);
        //                //if (objjobreq.jhstatusid == 5)
        //                //{
        //                //    if (objjobreq.custemail != null)
        //                //    {
        //                //        if (objjobreq.custemail.Trim() != "")
        //                //        {
        //                //            SendVoucherReport(objjobreq.br_compid, objjobreq.voucherno, objjobreq.custemail + (string.IsNullOrEmpty(ConfigurationManager.AppSettings["ADMINEMAIL"]) ? "" : "," + ConfigurationManager.AppSettings["ADMINEMAIL"]));
        //                //            //Task.Run( () => {
        //                //            //    Task.Delay(1000 * 5).ConfigureAwait(true);

        //                //            //}).ConfigureAwait(false);


        //                //            //string reseltsendmail = userJobRequestBLL.SendVoucherReport_BLL(objjobreq.br_compid, objjobreq.voucherno, objjobreq.custemail + ",dev.mishra@neelkanthdrugs.in");
        //                //            //string reseltsendmail = userJobRequestBLL.SendVoucherReport_BLL(objjobreq.voucherno, "sandeep.ng999@gmail.com");
        //                //        }
        //                //    }
        //                //}
        //                output.ResponseCode = 100;
        //                output.Message = "Success";
        //                output.Data = JobRequestData;
        //            }
        //            else
        //            {
        //                output.ResponseCode = 100;
        //                output.Message = "Success";
        //                output.Data = JobRequestData;
        //            }
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
        [HttpPost]
        [Route("SaveJobActionMultiple")]
        public async Task<HttpResponseMessage> SaveJobActionMultiple(JobRequestMultiple objjobreq, int userid, string langcode = "en")
        {
            JobRequestBLL userJobRequestBLL = new JobRequestBLL();
            try
            {
                List<JobRequestMultiple> JobRequestData = userJobRequestBLL.SaveJobActionMultiple(userid, objjobreq);
                if (JobRequestData != null)
                {
                    foreach (var item in objjobreq.JobRequestModel)
                    {
                        if (item.jhstatusid == 3 || item.jhstatusid == 5)
                        {
                            NotificationBLL objNotificationBLL = new NotificationBLL();
                            Notification objNotification = new Notification();
                            objNotification.companyid = item.br_compid;
                            objNotification.actionid = item.jhstatusid;

                            if (item.jhstatusid == 5)
                            {
                                item.jobscheduletime = item.actiontime;// DateTime.Now;
                            }
                            #region not used
                            //  bool issent = objNotificationBLL.SendnotificationByAction_BLL(objjobreq, objNotification);

                            //if (item.jhstatusid == 3)
                            //{
                            //    SendEmailOnPickVoucher(objjobreq, item.br_compid, langcode);
                            //}
                            //if (objjobreq.jhstatusid == 5)
                            //{
                            //    if (objjobreq.custemail != null)
                            //    {
                            //        if (objjobreq.custemail.Trim() != "")
                            //        { 
                            //            SendVoucherReport(objjobreq.br_compid, objjobreq.voucherno, objjobreq.custemail + (string.IsNullOrEmpty(ConfigurationManager.AppSettings["ADMINEMAIL"])?"":","+ ConfigurationManager.AppSettings["ADMINEMAIL"]) );

                            //            //Task.Run(() => {
                            //            //    Task.Delay(1000 * 5).ConfigureAwait(true);
                            //            //    SendVoucherReport(objjobreq.br_compid, objjobreq.voucherno, objjobreq.custemail + ",dev.mishra@neelkanthdrugs.in");
                            //            //}).ConfigureAwait(false);
                            //            // string reseltsendmail = userJobRequestBLL.SendVoucherReport_BLL(objjobreq.br_compid, objjobreq.voucherno, objjobreq.custemail + ",dev.mishra@neelkanthdrugs.in");
                            //            //string reseltsendmail = userJobRequestBLL.SendVoucherReport_BLL(objjobreq.voucherno, "sandeep.ng999@gmail.com");
                            //        }
                            //    }
                            //}
                            #endregion
                            output.ResponseCode = 100;
                            output.Message = "Success";
                            output.Data = JobRequestData;
                        }
                        else
                        {
                            output.ResponseCode = 100;
                            output.Message = "Success";
                            output.Data = JobRequestData;
                        }
                        //if (!string.IsNullOrWhiteSpace(item.vhid))
                        //{
                        //    List<string> itemCodeList = userJobRequestBLL.GetItemCode(item.vhid);
                        //    foreach (var itemCode in itemCodeList)
                        //    {
                        //        if (!string.IsNullOrWhiteSpace(itemCode))
                        //        {
                        //            string[] prefix = itemCode.Split('-');
                        //            if (prefix.Length > 0 && prefix[0].Equals("rd", StringComparison.OrdinalIgnoreCase))
                        //            {
                        //                await userJobRequestBLL.UpdateJobOrder(itemCode, objjobreq.JobRequestModel[0].deliveryaddrtype);
                        //            }
                        //        }
                        //    }
                        //}
                    }
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
        [Route("SaveJobActionNew")]
        public HttpResponseMessage SaveJobActionNew(int userid, [FromBody] JobRequest objjobreq, string langcode = "en")
        {

            //try
            //{
            //    NotificationBLL objNotificationBLL = new NotificationBLL();
            //    Notification objNotification = new Notification();
            //    objNotification.companyid = objjobreq.br_compid;
            //    objNotification.actionid = objjobreq.jhstatusid;

            //    if (objjobreq.jhstatusid == 5 || objjobreq.jhstatusid == 3)
            //    {
            //        objjobreq.jobscheduletime = DateTime.Now;
            //    }
            //    bool issent = objNotificationBLL.SendnotificationByAction_BLL(objjobreq, objNotification);
            //}catch(Exception ex)
            //{

            //}


            JobRequestBLL userJobRequestBLL = new JobRequestBLL();
            try
            {
                List<JobRequest> JobRequestData = userJobRequestBLL.SaveJobActionNew(userid, objjobreq);
                if (JobRequestData != null)
                {
                    if (objjobreq.jhstatusid == 3 || objjobreq.jhstatusid == 5)
                    {
                        NotificationBLL objNotificationBLL = new NotificationBLL();
                        Notification objNotification = new Notification();
                        objNotification.companyid = objjobreq.br_compid;
                        objNotification.actionid = objjobreq.jhstatusid;

                        if (objjobreq.jhstatusid == 5)
                        {
                            objjobreq.jobscheduletime = objjobreq.actiontime;// DateTime.Now;
                        }
                        bool issent = objNotificationBLL.SendnotificationByAction_BLL(objjobreq, objNotification);

                        if (objjobreq.jhstatusid == 3)
                        {
                            SendEmailOnPickVoucher(objjobreq, objjobreq.br_compid, langcode);
                        }
                        //if (objjobreq.jhstatusid == 5)
                        //{
                        //    if (objjobreq.custemail != null)
                        //    {
                        //        if (objjobreq.custemail.Trim() != "")
                        //        { 
                        //            SendVoucherReport(objjobreq.br_compid, objjobreq.voucherno, objjobreq.custemail + (string.IsNullOrEmpty(ConfigurationManager.AppSettings["ADMINEMAIL"])?"":","+ ConfigurationManager.AppSettings["ADMINEMAIL"]) );

                        //            //Task.Run(() => {
                        //            //    Task.Delay(1000 * 5).ConfigureAwait(true);
                        //            //    SendVoucherReport(objjobreq.br_compid, objjobreq.voucherno, objjobreq.custemail + ",dev.mishra@neelkanthdrugs.in");
                        //            //}).ConfigureAwait(false);
                        //            // string reseltsendmail = userJobRequestBLL.SendVoucherReport_BLL(objjobreq.br_compid, objjobreq.voucherno, objjobreq.custemail + ",dev.mishra@neelkanthdrugs.in");
                        //            //string reseltsendmail = userJobRequestBLL.SendVoucherReport_BLL(objjobreq.voucherno, "sandeep.ng999@gmail.com");
                        //        }
                        //    }
                        //}
                        output.ResponseCode = 100;
                        output.Message = "Success";
                        output.Data = JobRequestData;
                    }
                    else
                    {
                        output.ResponseCode = 100;
                        output.Message = "Success";
                        output.Data = JobRequestData;
                    }
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

        //Savesignatureimages_BLL
        #endregion

        #region JobHistory
        [HttpGet]
        [Route("GetJobHistory")]
        public HttpResponseMessage GetJobHistory(int jobid)//string vhid)
        {
            JobRequestBLL userJobRequestBLL = new JobRequestBLL();
            try
            {
                List<JobHistoryModule> JobHistoryData = userJobRequestBLL.getJobHistory_BLL(jobid);// vhid);
                if (JobHistoryData.Count > 0)
                {
                    output.ResponseCode = 100;
                    output.Message = "Success";
                    output.Data = JobHistoryData;
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
        }

        #endregion

        #region signature and images
        [HttpPost]
        [Route("SaveSignature")]
        public HttpResponseMessage SaveSignature(JobImagesModule objjobimage)
        {
            JobRequestBLL userJobRequestBLL = new JobRequestBLL();
            try
            {
                string signaturepath = userJobRequestBLL.Savesignatureimages_BLL(objjobimage);
                if (signaturepath != null)
                {
                    output.ResponseCode = 100;
                    output.Message = "Success";
                    output.Data = signaturepath;
                }
                else
                {
                    output.ResponseCode = 500;
                    output.Message = "SIgnature no saved.";
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

        [HttpPost]
        [Route("Savejobimages")]
        public HttpResponseMessage Savejobimages(int userid, JobImagesModule objimage)
        {
            JobRequestBLL userJobRequestBLL = new JobRequestBLL();
            try
            {
                string signaturepath = userJobRequestBLL.Savejobimages_BLL(userid, objimage);
                if (signaturepath != null)
                {
                    output.ResponseCode = 100;
                    output.Message = "Success";
                    output.Data = signaturepath;
                }
                else
                {
                    output.ResponseCode = 500;
                    output.Message = "SIgnature no saved.";
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

        [HttpPost]
        [Route("Deletejobimages")]
        public HttpResponseMessage Deletejobimages(JobImagesModule objjobimage)
        {
            JobRequestBLL userJobRequestBLL = new JobRequestBLL();
            try
            {
                bool isdeleted = userJobRequestBLL.Deletejobimages_BLL(objjobimage);
                if (isdeleted)
                {
                    output.ResponseCode = 100;
                    output.Message = "Success";
                    output.Data = isdeleted;
                }
                else
                {
                    output.ResponseCode = 500;
                    output.Message = "SIgnature no saved.";
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

        [HttpGet]
        [Route("GetjobImages")]
        public HttpResponseMessage GetjobImages(int jaid, int jobid)
        {
            JobRequestBLL objJobRequestBLL = new JobRequestBLL();
            try
            {
                List<JobImagesModule> jobimages = objJobRequestBLL.getJobimages_BLL(jaid, jobid);
                if (jobimages.Count > 0)
                {
                    output.ResponseCode = 100;
                    output.Message = "Success";
                    output.Data = jobimages;
                }
                else
                {
                    output.ResponseCode = 500;
                    output.Message = "No data";
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

        #endregion

        #region Voucheritems
        [HttpGet]
        [Route("GetItems")]
        public HttpResponseMessage GetItems(int branchid, int empcode)
        {
            ItemsBLL objItemsBLL = new ItemsBLL();
            try
            {
                List<ItemsModule> ItemData = objItemsBLL.getItems_BLL(branchid, empcode);
                if (ItemData.Count > 0)
                {
                    output.ResponseCode = 100;
                    output.Message = "Success";
                    output.Data = ItemData;
                }
                else
                {
                    output.ResponseCode = 500;
                    output.Message = "No data";
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
        [HttpGet]
        [Route("GetItemsData")]
        public HttpResponseMessage GetItemsData(int roleid, string branchcode, int empcode)
        {
            ItemsBLL objItemsBLL = new ItemsBLL();
            try
            {
                List<ItemsModule> ItemData = objItemsBLL.getItemsData_BLL(roleid, branchcode, empcode);
                if (ItemData.Count > 0)
                {
                    output.ResponseCode = 100;
                    output.Message = "Success";
                    output.Data = ItemData;
                }
                else
                {
                    output.ResponseCode = 500;
                    output.Message = "No data";
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

        [HttpGet]
        [Route("getofflineitemData")]
        public HttpResponseMessage getofflineitemData(int roleid, string branchcode, int userid, string itemtype)
        {
            ItemsBLL objItemsBLL = new ItemsBLL();
            try
            {
                List<ItemsModule> ItemData = objItemsBLL.getofflineitem_BLL(branchcode, userid, itemtype);
                if (ItemData.Count > 0)
                {
                    output.ResponseCode = 100;
                    output.Message = "Success";
                    output.Data = ItemData;
                }
                else
                {
                    output.ResponseCode = 500;
                    output.Message = "No data";
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

        [HttpGet]
        [Route("GetItemsDatabyvoucherno")]
        public HttpResponseMessage GetItemsDatabyvoucherno(int companyid, string voucherno)
        {
            ItemsBLL objItemsBLL = new ItemsBLL();
            try
            {
                List<ItemsModule> ItemData = objItemsBLL.getItemsDatabyvhid_BLL(companyid, voucherno);
                if (ItemData.Count > 0)
                {
                    output.ResponseCode = 100;
                    output.Message = "Success";
                    output.Data = ItemData;
                }
                else
                {
                    output.ResponseCode = 500;
                    output.Message = "No data";
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

        [HttpGet]
        [Route("GetDeliveredItemsDatabyjobid")]
        public HttpResponseMessage GetDeliveredItemsDatabyjobid(int jobid, string itemtype)
        {
            ItemsBLL objItemsBLL = new ItemsBLL();
            try
            {
                List<ItemsModule> ItemData = new List<ItemsModule>();
                if (itemtype == "h")
                {
                    ItemData = objItemsBLL.getDeliveredHandlingUnitLoad_BLL(jobid);
                }
                else
                {
                    ItemData = objItemsBLL.getDeliveredItemsDatabyjobid_BLL(jobid);
                }

                if (ItemData.Count > 0)
                {
                    output.ResponseCode = 100;
                    output.Message = "Success";
                    output.Data = ItemData;
                }
                else
                {
                    output.ResponseCode = 500;
                    output.Message = "No data";
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


        [HttpPost]
        [Route("SaveItemsData")]
        public HttpResponseMessage SaveItemsData(int jobid, string drivercode, List<ItemsModule> lstitemsModules, string itemtype, int loadid, int companyid)
        {
            LoadJobRequestBLL loadJobRequestBLL = new LoadJobRequestBLL();
            ItemsBLL objItemsBLL = new ItemsBLL();
            try
            {
                if (itemtype == "h")
                {
                    var handlingUnitIds = lstitemsModules.Select(x => x.handlingunitid).ToList();
                    lstitemsModules = loadJobRequestBLL.GetItemsDatabyLoadId_BLL(companyid, loadid).Where(x => handlingUnitIds.Contains(x.handlingunitid)).ToList();
                }

                bool isupdated = objItemsBLL.saveItemData_BLL(jobid, drivercode, lstitemsModules);
                if (isupdated)
                {
                    output.ResponseCode = 100;
                    output.Message = "Success";
                    output.Data = isupdated;
                }
                else
                {
                    output.ResponseCode = 500;
                    output.Message = "Error!!! Please try again.";
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


        [HttpGet]
        [Route("GetofflineItems")]
        public HttpResponseMessage GetofflineItems(int userid, string branchcode)
        {
            ItemsBLL objItemsBLL = new ItemsBLL();
            try
            {
                List<ItemsModule> ItemData = objItemsBLL.getofflineItems_BLL(userid, branchcode);
                if (ItemData.Count > 0)
                {
                    output.ResponseCode = 100;
                    output.Message = "Success";
                    output.Data = ItemData;
                }
                else
                {
                    output.ResponseCode = 500;
                    output.Message = "No data";
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
        #endregion

        #region sendmail

        public bool SendEmailOnPickVoucher(JobRequest objjobreq, int companyid, String langcode)
        {
            try
            {
                MasterBLL masterBLL = new MasterBLL();
                //try
                //{
                //      companyid = objjobreq.br_compid;
                var mailConfig = masterBLL.getMailConfigBLL(companyid, "VoucherPicked", langcode, objjobreq.deliverytypecode);
                var subject = new StringBuilder(mailConfig.Rows[0]["subject"] + "");
                var body = new StringBuilder(mailConfig.Rows[0]["body"] + "");
                var mailids = objjobreq.custemail + (string.IsNullOrEmpty(ConfigurationManager.AppSettings["ADMINEMAIL"]) ? "" : "," + ConfigurationManager.AppSettings["ADMINEMAIL"]);
                var voucherno = objjobreq.voucherno;
                //var trackingurl = string.Format("{0}{1}/{2}/{3}",
                //    System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority),
                //    voucherno, objjobreq.drivercode, objjobreq.jobscheduletime.ToString("ddMMyy")
                //    );

                var tracking_map_type = masterBLL.GetSysSettings_BLL(companyid, "tracking_map_type").Select(x => x.value).FirstOrDefault();

                var trackingurl = string.Format("{0}/customer/{1}?voucherno={2}&companyid={3}", System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority), (tracking_map_type == "google" ? "driverdetail.html" : "driverdetail_leaflet.html"), voucherno, companyid);

                body.Replace("@customername", objjobreq.firmname);
                body.Replace("@drivername", objjobreq.drivername);
                body.Replace("@trackingurl", trackingurl);
                body.Replace("@scheduledate", objjobreq.jobscheduletime.ToString("dd-MM-yyyy"));
                body.Replace("@voucherno", voucherno);
                body.Replace("@voucherdate", objjobreq.voucherdate.ToString("dd-MM-yyyy"));


                subject.Replace("@customername", objjobreq.firmname);
                subject.Replace("@drivername", objjobreq.drivername);
                subject.Replace("@trackingurl", trackingurl);
                subject.Replace("@scheduledate", objjobreq.jobscheduletime.ToString("dd-MM-yyyy"));
                subject.Replace("@voucherno", voucherno);
                subject.Replace("@voucherdate", objjobreq.voucherdate.ToString("dd-MM-yyyy"));

                if (new NotificationBLL().SendEmailWithAttachment(companyid, subject.ToString(), body.ToString(), mailids, null, null, null))
                {
                    return true;
                }
                else
                {
                    return false;
                }
                //}
                //catch (Exception ex)
                //{
                //    return false;
                //}
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
                return false;
            }
        }


        [HttpGet]
        [Route("SendVoucherReport")]
        public HttpResponseMessage SendVoucherReport(int companyid, string invoiceno, string mailids, string langcode = "en", string deliverytype = "d")
        {
            JobRequestBLL userJobRequestBLL = new JobRequestBLL();
            MasterBLL masterBLL = new MasterBLL();
            try
            {
                DataTable dt = new DataTable();
                var path_en = System.Web.HttpContext.Current.Server.MapPath("~/Files/" + companyid + "_" + invoiceno + "_" + "en" + "_report.pdf");
                var path_id = System.Web.HttpContext.Current.Server.MapPath("~/Files/" + companyid + "_" + invoiceno + "_" + "id" + "_report.pdf");

                if (!System.IO.File.Exists(path_id))
                    dt = GenerateVoucher(companyid, invoiceno, "en");

                if (!System.IO.File.Exists(path_id))
                    dt = GenerateVoucher(companyid, invoiceno, "id");

                if (dt == null)
                {
                    return null;
                }

                var mailConfig = masterBLL.getMailConfigBLL(companyid, "VoucherReport", langcode, deliverytype);

                var path = System.Web.HttpContext.Current.Server.MapPath("~/Files/" + companyid + "_" + invoiceno + "_" + langcode + "_report.pdf");

                var subject = new StringBuilder(mailConfig.Rows[0]["subject"] + "");
                var body = new StringBuilder(mailConfig.Rows[0]["body"] + "");
                mailids = mailids + (string.IsNullOrEmpty(ConfigurationManager.AppSettings["ADMINEMAIL"]) ? "" : "," + ConfigurationManager.AppSettings["ADMINEMAIL"]);


                if (dt.Rows.Count > 0)
                {
                    var row = dt.Rows[0];

                    body.Replace("@customername", row["firmname"] + "");
                    body.Replace("@drivername", row["empfirstname"] + " " + row["emplastname"]);
                    //body.Replace("@trackingurl", trackingurl);
                    body.Replace("@scheduledate", row["scheduletime"] + "");
                    body.Replace("@voucherno", row["orderno"] + "");
                    body.Replace("@voucherdate", row["orderdate"] + "");
                    body.Replace("@deliverydate", row["actiontime"] + "");


                    subject.Replace("@customername", row["firmname"] + "");
                    subject.Replace("@drivername", row["empfirstname"] + " " + row["emplastname"]);
                    //subject.Replace("@trackingurl", trackingurl);
                    subject.Replace("@scheduledate", row["scheduletime"] + "");
                    subject.Replace("@voucherno", row["orderno"] + "");
                    subject.Replace("@voucherdate", row["orderdate"] + "");
                    subject.Replace("@deliverydate", row["actiontime"] + "");

                }





                //voucherno
                //  new ; 
                // string reseltsendmail = userJobRequestBLL.SendVoucherReport_BLL(companyid, invoiceno, mailids);
                if (new NotificationBLL().SendEmailWithAttachment(companyid, subject.ToString(), body.ToString(), mailids, null, null, path))
                {
                    output.ResponseCode = 100;
                    output.Message = "Success";
                    output.Data = mailids;
                }
                else
                {
                    output.ResponseCode = 500;
                    output.Message = "Mail not sent.";
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

        //[HttpGet]
        //[Route("SendVoucherReportNew")]
        //public string SendWithAttachment_UsingRDLC(int companyId, string invoiceNo, string mailID)
        //{
        //    MasterBLL oMasterBLL = new MasterBLL();

        //    DataTable dt_Mail = new DataTable();
        //    dt_Mail = oMasterBLL.getMailConfigBLL(companyId, "VoucherReport");
        //    string Result = string.Empty;
        //    try
        //    {
        //        if (dt_Mail.Rows.Count > 0)
        //        {
        //            string Body = string.Empty;
        //            string Subject = string.Empty;
        //            string Host = string.Empty;
        //            string MailTo = string.Empty;
        //            string MailCC = string.Empty;
        //            string UserName = string.Empty;
        //            string Password = string.Empty;
        //            string FromMailID = string.Empty;
        //            string ToMailID = string.Empty;
        //            string CCMailid = string.Empty;
        //            string Attachment = string.Empty;

        //            StringBuilder sb = new StringBuilder();
        //            ToMailID = MailID + ",";
        //            // CCMailid = "himanshuchhikara2440@gmail.com,";
        //            string _fileName = string.Empty;
        //            _fileName = InvoiceNo.ToString() + "_Report";
        //            dt = oCommon.Get_VoucherDetails_All(InvoiceNo);
        //            dt2 = oCommon.Get_VoucherImage_All(InvoiceNo);
        //            dt3 = oCommon.Get_VoucherFooterDetails_All(InvoiceNo);


        //            // CCMailid += "himanshuchhikara2440@gmail.com,";
        //            ToMailID = ToMailID.Replace(",,", ",");
        //            CCMailid = CCMailid.Replace(",,", ",");
        //            //ToMailID = "shashankbirthare@merinoservices.com";

        //            if (dt.Rows.Count > 0)
        //            {
        //                // Body = "<p>Dear Customer,</p><p>Kindly find the Quotation for your Order No. <strong>@CallID</strong> attached herewith.</p><p>Thank you.</p><p>Regards,</p><p>Customer Service </p><p>Awal Gulf Manufacturing Co.</p>";
        //                // Subject = "Quotation for Order no. @CallID ";
        //                Host = dt_Mail.Rows[0]["hostname"].ToString();
        //                Body = dt_Mail.Rows[0]["body"].ToString();
        //                MailTo = dt_Mail.Rows[0]["mailto"].ToString();
        //                MailCC = dt_Mail.Rows[0]["mailcc"].ToString();
        //                Subject = dt_Mail.Rows[0]["subject"].ToString();
        //                UserName = dt_Mail.Rows[0]["frommailid"].ToString();
        //                Password = dt_Mail.Rows[0]["password"].ToString();

        //                if (MailTo != "")
        //                {
        //                    ToMailID = MailTo + ",";
        //                }
        //                CCMailid += MailCC + ",";
        //                ToMailID = ToMailID.Replace(",,", ",");
        //                CCMailid = CCMailid.Replace(",,", ",");
        //                //ToMailID = "shashankbirthare@merinoservices.com";

        //                // Body = Body.Replace("@FromWH", dt_data.Rows[0]["From"].ToString()).Replace("@ToWH", dt_data.Rows[0]["TO"].ToString()).Replace("@DeliveryNote", dt_data.Rows[0]["DeliveryNoteNo"].ToString()).Replace("@Date", dt_data.Rows[0]["DeliveryDate"].ToString());
        //                // Subject = Subject.Replace("@FromWH", dt_data.Rows[0]["From"].ToString()).Replace("@ToWH", dt_data.Rows[0]["TO"].ToString()).Replace("@DeliveryNote", dt_data.Rows[0]["DeliveryNoteNo"].ToString());
        //                FromMailID = dt_Mail.Rows[0]["frommailid"].ToString();
        //                MailMessage mm = new MailMessage();
        //                mm.From = new MailAddress(FromMailID);
        //                mm.Subject = Subject;
        //                mm.Body = Body;
        //                if (CCMailid.Remove(CCMailid.Length - 1) != "")
        //                {
        //                    mm.CC.Add(CCMailid.Remove(CCMailid.Length - 1));
        //                }
        //                if (ToMailID.Remove(ToMailID.Length - 1) != "")
        //                {
        //                    mm.To.Add(ToMailID.Remove(ToMailID.Length - 1));
        //                }
        //                BindDelivery_Mail(InvoiceNo, _fileName);


        //                System.Net.Mail.Attachment attachment;
        //                attachment = new System.Net.Mail.Attachment(Server.MapPath("MailPDF/" + _fileName + ".pdf"));
        //                mm.Attachments.Add(attachment);

        //                mm.IsBodyHtml = true;
        //                SmtpClient smtp = new SmtpClient();
        //                smtp.Host = Host;
        //                smtp.EnableSsl = true;
        //                smtp.Port = 587;
        //                smtp.Credentials = new System.Net.NetworkCredential(UserName, Password);
        //                smtp.Send(mm);
        //                //res = true;
        //                Result = "Success";
        //            }
        //        }
        //        else
        //        {
        //            Result = "Error: Please maintain mail configuration.";
        //        }
        //        // }

        //    }
        //    catch (Exception ex)
        //    {
        //        Result = "Error :" + ex.StackTrace.ToString();
        //    }
        //    return Result;
        //}

        #endregion

        [HttpGet]
        [Route("GeneratePDF")]
        public DataTable GenerateVoucher(int companyid, string voucherNo, string langcode)
        {
            var filesPath = System.Web.HttpContext.Current.Server.MapPath("~/Files/");
            var path = System.Web.HttpContext.Current.Server.MapPath("~/Content/");
            var txtFile = new StringBuilder(File.ReadAllText(System.IO.Path.Combine(path, "VoucherReportInvoice.html")));
            var footerHtml = new StringBuilder("<div style='text-align:center;width: 100%;'><div style='font-size:16px;'><b>@lbl_footer_line1</b></div> <div style='font-size:14px;'> @lbl_footer_line2 </div> </div>");

            var fileName = companyid + "_" + voucherNo + "_" + langcode + "_report.pdf";
            var fileWithPath = Path.Combine(filesPath, fileName);
            try
            {
                if (File.Exists(fileWithPath))
                {
                    File.Delete(fileWithPath);
                }
            }
            catch { }
            JobRequestBLL userJobRequestBLL = new JobRequestBLL();

            var voucherDetailTd = userJobRequestBLL.GetVoucherDetails_BLL(companyid, voucherNo);

            if (voucherDetailTd.Rows.Count > 0)
            {
                var row = voucherDetailTd.Rows[0];

                ItemsBLL objItemsBLL = new ItemsBLL();
                List<ItemsModule> ItemData = objItemsBLL.getDeliveredItemsDatabyjobid_BLL(int.Parse(voucherDetailTd.Rows[0]["jobid"] + ""));


                txtFile.Replace("@companylogo", "data:image/png;base64," + ImagePathToBase64(Path.Combine(filesPath, row["comapanyimage"] + "")));
                txtFile.Replace("@voucherno", row["orderno"] + "");
                txtFile.Replace("@orderdate", row["orderdate"] + "");

                txtFile.Replace("@firmname", row["firmname"] + "");
                txtFile.Replace("@contactpersonname", row["contactpersonname"] + "");
                txtFile.Replace("@contactno", row["contactno"] + "");
                txtFile.Replace("@address", row["address"] + "");

                txtFile.Replace("@empfirstname", row["empfirstname"] + "");
                txtFile.Replace("@emplastname", row["emplastname"] + "");

                var ind = 0;
                var sbItems = new StringBuilder();
                if (ItemData.Count > 0)
                {
                    foreach (var item in ItemData)
                    {
                        ind++;
                        sbItems.Append(string.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td><td>{7}</td><td>{8}</td></tr>",
                            ind, item.itemcode, item.itemname, item.color, item.size, item.orderqty, item.itemuom, item.rejectedqty == 0 ? null : (decimal?)item.rejectedqty, item.rejectedreason
                            ));
                    }
                }
                txtFile.Replace("@items", sbItems.ToString());

                txtFile.Replace("@vsname", row["vsname"] + "");

                txtFile.Replace("@actiontime", row["actiontime"] + "");

                txtFile.Replace("@vehiclenumber", row["vehiclenumber"] + "");

                txtFile.Replace("@approvername", row["approvername"] + "");

                txtFile.Replace("@latitude", row["latitude"] + "");
                txtFile.Replace("@longitude", row["longitude"] + "");


                try
                {
                    int jaid = 0;
                    int jobid = 0;

                    int.TryParse(row["jaid"] + "", out jaid);
                    int.TryParse(row["jobid"] + "", out jobid);

                    List<JobImagesModule> jobimages = userJobRequestBLL.getJobimages_BLL(jaid, jobid);

                    var driver = jobimages.Where(x => x.remark == "Driver").FirstOrDefault()?.image;
                    var receiver = jobimages.Where(x => x.remark == "Receiver").FirstOrDefault()?.image;

                    var others = jobimages.Where(x => x.remark == "" || x.remark == null).ToList();

                    txtFile.Replace("@driversignature", "data:image/png;base64," + ImagePathToBase64(Path.Combine(filesPath, (driver + "").Split('/').Last())));
                    txtFile.Replace("@receiversignature", "data:image/png;base64," + ImagePathToBase64(Path.Combine(filesPath, (receiver + "").Split('/').Last())));

                    var sbOtherImages = new StringBuilder();
                    foreach (var other in others)
                    {
                        var base64Img = "data:image/png;base64," + ImagePathToBase64(Path.Combine(filesPath, (other.image + "").Split('/').Last()));
                        sbOtherImages.Append(string.Format("<div style='border: 3px solid #232323;display:inline-block;'><img src='{0}' height='200' /></div>", base64Img));
                    }
                    txtFile.Replace("@proofimages", sbOtherImages.ToString());

                    if (ItemData.Count >= 30)
                    {
                        txtFile.Replace("@signaturelinestart", "<div style='page-break-after:always'></div>");
                        txtFile.Replace("@prooflinestart", "");
                    }
                    else
                    {
                        txtFile.Replace("@signaturelinestart", "");

                        if (ItemData.Count > 23)
                        {
                            txtFile.Replace("@prooflinestart", "<div style='page-break-after:always'></div>");
                        }
                        else
                        {
                            txtFile.Replace("@prooflinestart", "");
                        }
                    }


                    var voucherLBLPath = File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath(String.Format("~/content/lang/{0}_lang.json", langcode)));
                    var lblDic = JsonConvert.DeserializeObject<Dictionary<string, string>>(voucherLBLPath);
                    foreach (var item in lblDic)
                    {
                        txtFile.Replace("@" + item.Key, item.Value);
                        footerHtml.Replace("@" + item.Key, item.Value);
                    }
                }
                catch (Exception ex)
                {
                    Helper.LogException(ex);
                }

                HtmlToPDFItext7(txtFile.ToString(), filesPath, fileName, true, footerHtml.ToString());
            }

            return voucherDetailTd;
        }

        public string HtmlToPDFItext7(string htmlString, string savePath, string fileName = "attachment.pdf", bool isDisplayFooter = true, string footerHtml = null)
        {
            try
            {
                if (!Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                }

                string pdfFullPath = Path.Combine(savePath, fileName);

                //Setting destination
                //FileOutputStream fileOutputStream = new FileOutputStream(new File(dirPath + "/USER-16-PF-Report.pdf"));

                PdfWriter pdfWriter = new PdfWriter(pdfFullPath);
                ConverterProperties converterProperties = new ConverterProperties();
                PdfDocument pdfDocument = new PdfDocument(pdfWriter);


                //For setting the PAGE SIZE
                pdfDocument.SetDefaultPageSize(iText.Kernel.Geom.PageSize.A4);// (new PageSize(PageSize.A3));

                var document = HtmlConverter.ConvertToDocument(htmlString, pdfDocument, converterProperties);

                document.Close();
                return pdfFullPath;
            }catch(Exception ex)
            {
                Helper.LogException(ex);
            }
            return null;
            //return GeneratePDFByItext();
        }


        //public string HtmlToPDF(string htmlString, string savePath, string fileName = "attachment.pdf", bool isDisplayFooter = true, string footerHtml = null)
        //{
            
        //    //read parameters from the webpage
        //    // string htmlString = File.ReadAllText(path);// "TxtBaseUrl.Text";
        //    string pdf_page_size =  "A4"; //"A3";//
        //    string pdf_orientation = "Portrait";//Landscape

        //    PdfPageSize pageSize = (PdfPageSize)Enum.Parse(typeof(PdfPageSize), pdf_page_size, true);
        //    PdfPageOrientation pdfOrientation = (PdfPageOrientation)Enum.Parse(typeof(PdfPageOrientation), pdf_orientation, true);

        //    int webPageWidth = 1024;
        //    try
        //    {
        //        webPageWidth = 1024;
        //    }
        //    catch { }

        //    int webPageHeight = 0;
        //    try
        //    {
        //        // webPageHeight = Convert.ToInt32(TxtHeight.Text);
        //    }
        //    catch { }

        //    // instantiate a html to pdf converter object
        //    HtmlToPdf converter = new HtmlToPdf();

        //    // set converter options
        //    converter.Options.PdfPageSize = pageSize;
        //    converter.Options.PdfPageOrientation = pdfOrientation;
        //    converter.Options.WebPageWidth = webPageWidth;
        //    converter.Options.WebPageHeight = webPageHeight;
        //    converter.Options.MinPageLoadTime = 2;
        //    converter.Options.MaxPageLoadTime = 60000;



        //    if (isDisplayFooter)
        //    {

        //        // footer settings
        //        converter.Options.DisplayFooter = true;
        //        converter.Footer.DisplayOnFirstPage = true;
        //        converter.Footer.DisplayOnOddPages = true;
        //        converter.Footer.DisplayOnEvenPages = true;
        //        //converter.Footer.Height = model.FooterHeight;


        //        //if (!string.IsNullOrEmpty(model.FooterUrl))
        //        //{
        //        //    PdfHtmlSection footerHtml = new PdfHtmlSection(new HttpHelper<string>(baseUrl, "application/html").GetHTML(model.FooterUrl), baseUrl);
        //        //    footerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
        //        //    converter.Footer.Add(footerHtml);
        //        //}

        //        if (!string.IsNullOrEmpty(footerHtml))
        //        {
        //            PdfHtmlSection _footer = new PdfHtmlSection(footerHtml, null); //, baseUrl
        //            _footer.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
        //            converter.Footer.Add(_footer);
        //        }
        //    }



        //    // create a new pdf document converting an url
        //    PdfDocument doc = converter.ConvertHtmlString(htmlString);  //baseUrl
        //                                                                //PdfDocument doc = converter.ConvertUrl(baseUrl + "" + url);// converter.ConvertHtmlString(htmlString, Helper.GetDomainName());

        //    // save pdf document
        //    //  doc.Save(System.Web.HttpContext.Current.Response, false, "Sample.pdf");

        //    //   doc.Save(System.Web.HttpContext.Current.Server.MapPath("~"), false, "Sample.pdf");
        //    // var path2 = System.Web.HttpContext.Current.Server.MapPath("~/content");
        //    if (!Directory.Exists(savePath))
        //    {
        //        Directory.CreateDirectory(savePath);
        //    }

        //    string pdfFullPath = Path.Combine(savePath, fileName);
        //    doc.Save(pdfFullPath);

        //    // close pdf document
        //    doc.Close();

        //    return pdfFullPath;
        //}

        public string ImagePathToBase64(string path)
        {
            try
            {
                using (var image = System.Drawing.Image.FromFile(path))
                {
                    using (MemoryStream m = new MemoryStream())
                    {
                        image.Save(m, image.RawFormat);
                        byte[] imageBytes = m.ToArray();

                        // Convert byte[] to Base64 String
                        string base64String = Convert.ToBase64String(imageBytes);
                        return base64String;
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
            return null;
        }
         
        public string GeneratePDFByItext()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<h1>INVOICE</h1>");
            sb.Append("<div id='company' class='clearfix'>");
            sb.Append("<div>Company Name</div>");
            sb.Append("<div>455 John Tower,<br /> AZ 85004, US</div>");
            sb.Append("<div>(602) 519-0450</div>");
            sb.Append("<div><a href='mailto:company@example.com'>company@example.com</a></div>");
            sb.Append("</div>");
            sb.Append("<div id='project'>");
            sb.Append("<div><span>PROJECT</span> Website development</div>");
            sb.Append("<div><span>CLIENT</span> John Doe</div>");
            sb.Append("<div><span>ADDRESS</span> 796 Silver Harbour, TX 79273, US</div>");
            sb.Append("<div><span>EMAIL</span> <a href='mailto:john@example.com'>john@example.com</a></div>");
            sb.Append("<div><span>DATE</span> April 13, 2016</div>");
            sb.Append("<div><span>DUE DATE</span> May 13, 2016</div>");
            sb.Append("</div>");
            sb.Append("</header>");
            sb.Append("<main>");
            sb.Append("<table>");
            sb.Append("<thead>");
            sb.Append("<tr>");


            // System.IO.File htmlSource = new System.IO.File("input.html");
            // System.IO.File pdfDest = new System.IO.File("output.pdf");
            // // pdfHTML specific code
            // ConverterProperties converterProperties = new ConverterProperties();
            // HtmlConverter.ConvertToPdf(new FileInputStream(htmlSource),
            //new FileOutputStream(pdfDest), converterProperties);


            string path = System.Web.HttpContext.Current.Server.MapPath("~/Files/");

            //HTML String
            String htmlFile = sb.ToString();
            //Setting destination
            //FileOutputStream fileOutputStream = new FileOutputStream(new File(dirPath + "/USER-16-PF-Report.pdf"));

            PdfWriter pdfWriter = new PdfWriter(path + DateTime.Now.Ticks + "fileName.pdf");
            ConverterProperties converterProperties = new ConverterProperties();
            PdfDocument pdfDocument = new PdfDocument(pdfWriter);

            //For setting the PAGE SIZE
            pdfDocument.SetDefaultPageSize(iText.Kernel.Geom.PageSize.A4);// (new PageSize(PageSize.A3));

            var document = HtmlConverter.ConvertToDocument(htmlFile, pdfDocument, converterProperties);
            document.Close();



            StringReader sr = new StringReader(sb.ToString());

            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
            pdfDoc.AddHeader("HEADER1", "WELCOME TO HEADER");

            var htmlparser = new iTextSharp.text.html.simpleparser.HTMLWorker(pdfDoc);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(memoryStream);// PdfWriter.GetInstance(pdfDoc, memoryStream);
                pdfDoc.Open();

                htmlparser.Parse(sr);
                pdfDoc.Close();

                using (FileStream fs = new FileStream(System.IO.Path.Combine(path, DateTime.Now.Ticks + "order.pdf"), FileMode.Create, FileAccess.ReadWrite))
                {
                    byte[] bytes = memoryStream.ToArray();
                    fs.Write(bytes, 0, bytes.Length);
                    memoryStream.Close();
                }
            }
            return path;
        }


        //#region fileaccess
        //[HttpGet]
        //[Route("Files/{*filename}")]
        //public HttpResponseMessage Files(string filename)
        //{
        //    try
        //    {
        //        //var stream = new MemoryStream();
        //        //// processing the stream.

        //        //var result = new HttpResponseMessage(HttpStatusCode.OK)
        //        //{
        //        //    Content = new ByteArrayContent(stream.ToArray())
        //        //};
        //        //result.Content.Headers.ContentDisposition =
        //        //    new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
        //        //    {
        //        //        FileName = filename
        //        //    };
        //        //result.Content.Headers.ContentType =
        //        //    new MediaTypeHeaderValue("application/octet-stream");

        //        //return result;

        //        //var path = System.Web.HttpContext.Current.Server.MapPath("~/Files/"+ filename); ;
        //        //HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
        //        //var stream = new FileStream(path, FileMode.Open);
        //        //result.Content = new StreamContent(stream);
        //        //result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("inline");
        //        //result.Content.Headers.ContentDisposition.FileName = Path.GetFileName(path);
        //        //result.Content.Headers.ContentType = new MediaTypeHeaderValue(System.Web.MimeMapping.GetMimeMapping(filename));
        //        //result.Content.Headers.ContentLength = stream.Length;
        //        //return result;

        //        //Create HTTP Response.
        //        HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);

        //        //Set the File Path.
        //        string filePath = HttpContext.Current.Server.MapPath("~/Files/") + filename;

        //        //Check whether File exists.
        //        if (!File.Exists(filePath))
        //        {
        //            //Throw 404 (Not Found) exception if File not found.
        //            response.StatusCode = HttpStatusCode.NotFound;
        //            response.ReasonPhrase = string.Format("File not found: {0} .", filename);
        //            throw new HttpResponseException(response);
        //        }

        //        ////Read the File into a Byte Array.
        //        //byte[] bytes = File.ReadAllBytes(filePath);

        //        ////Set the Response Content.
        //        //response.Content = new ByteArrayContent(bytes);

        //        ////Set the Response Content Length.
        //        //response.Content.Headers.ContentLength = bytes.LongLength;

        //        ////Set the Content Disposition Header Value and FileName.
        //        //response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("inline");
        //        //response.Content.Headers.ContentDisposition.FileName = filename;

        //        ////Set the File Content Type.
        //        //response.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(filename));

        //        FileStream fileStream = new FileStream(filePath, FileMode.Open);
        //        Image image = Image.FromStream(fileStream);
        //        MemoryStream memoryStream = new MemoryStream();
        //        image.Save(memoryStream, ImageFormat.Jpeg);
        //        response.Content = new ByteArrayContent(memoryStream.ToArray());
        //        response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

        //        return response;

        //    }
        //    catch (Exception ex)
        //    {
        //        output.ResponseCode = 500;
        //        output.Message = ex.Message.ToString();
        //        output.Data = null;
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, output);
        //    }
        //}
        //#endregion

        public int GenerateOTP()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }



        #region Locations

        [HttpPost]
        [Route("SaveEmployeeLocation")]
        public HttpResponseMessage SaveEmployeeLocation(List<EmployeeLocation> locations)
        {
            EmployeeLocationBLL objBLL = new EmployeeLocationBLL();
            try
            {
                bool isupdated = objBLL.SaveEmployeeLocation(locations);
                if (isupdated)
                {
                    output.ResponseCode = 100;
                    output.Message = "Success";
                    output.Data = isupdated;
                }
                else
                {
                    output.ResponseCode = 500;
                    output.Message = "Error!!! Please try again.";
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

        //[HttpPost]
        [Route("GetEmployeeLocations")]
        public HttpResponseMessage GetEmployeeLocations(int empid, DateTime date)
        {
            EmployeeLocationBLL objBLL = new EmployeeLocationBLL();
            try
            {
                output.ResponseCode = 100;
                output.Message = "Success";
                output.Data = objBLL.GetEmployeeLocations(empid, date);
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


        [Route("GetVoucherDetail")]
        public HttpResponseMessage GetVoucherDetail(int companyid, string voucherNo, int loadid)
        {
            JobRequestBLL userJobRequestBLL = new JobRequestBLL();
            try
            {
                output.ResponseCode = 100;
                output.Message = "Success";
                if (loadid > 0)
                {
                    output.Data = new LoadJobRequestBLL().GetLoadAndJobByLoadId_BLL(loadid).FirstOrDefault();
                }
                else
                {
                    output.Data = userJobRequestBLL.GetVoucherDetailVM_BLL(companyid, voucherNo);
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

        //[HttpPost]
        [Route("GetUsers")]
        public HttpResponseMessage GetUsers(int roleid, int companyid)
        {
            EmployeeLocationBLL objBLL = new EmployeeLocationBLL();
            try
            {
                output.ResponseCode = 100;
                output.Message = "Success";
                output.Data = objBLL.GetUsers(roleid, companyid);
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

        #endregion




        #region Exception Log
        [Route("LogException")]
        public HttpResponseMessage LogException(LogExceptionVM model)
        {
            EmployeeLocationBLL objBLL = new EmployeeLocationBLL();
            try
            {
                Helper.LogException((model.empid + "_" + model.type + ".txt"), model.error);
                output.ResponseCode = 100;
                output.Message = "Success";
                output.Data = 1;
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


        [Route("ReadFile")]
        public HttpResponseMessage ReadFile(string filePath)
        {
            return Request.CreateResponse(HttpStatusCode.OK, System.IO.File.ReadAllText(Path.Combine(HttpContext.Current.Server.MapPath("~/"), filePath)));
        }

        [Route("ClearFile")]
        public HttpResponseMessage ClearFile(string filePath)
        {
            System.IO.File.WriteAllText(Path.Combine(HttpContext.Current.Server.MapPath("~/"), filePath), "");
            return Request.CreateResponse(HttpStatusCode.OK, "Cleared");
        }
        #endregion



    }

    public class LogExceptionVM
    {
        public int? id { get; set; }
        public int? empid { get; set; }
        public string type { get; set; }
        public string error { get; set; }
    }


}

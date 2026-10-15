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
using System.Data.SqlClient;

namespace PODWebAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/Load")] 
    [APIAuthorizeAttribute]
    public class LoadController : ApiController
    {
        Output output = new Output();
         
        [HttpGet]
        [Route("GetUserjobrequestData")]
        public HttpResponseMessage GetUserjobrequestData(int roleid, int userid, string branchcode, int empid, string type, long tagno)
        {
            LoadJobRequestBLL userJobRequestBLL = new LoadJobRequestBLL();
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


            LoadJobRequestBLL userJobRequestBLL = new LoadJobRequestBLL();
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
                        //            SendVoucherReport(objjobreq.br_compid, objjobreq.loadid ?? 0, objjobreq.loadno, objjobreq.custemail + (string.IsNullOrEmpty(ConfigurationManager.AppSettings["ADMINEMAIL"]) ? "" : "," + ConfigurationManager.AppSettings["ADMINEMAIL"]));
						//
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


        [HttpGet]
        [Route("GetItemsDatabyLoadId")]
        public HttpResponseMessage GetItemsDatabyLoadId(int companyid, int loadid, string itemtype)
        {
            LoadJobRequestBLL loadJobRequestBLL = new LoadJobRequestBLL();
            try
            {
                List<ItemsModule> ItemData = new List<ItemsModule>();
                
                if (itemtype == "h")
                {
                    ItemData = loadJobRequestBLL.GetHandlingUnitByLoadId_BLL(companyid, loadid);
                }
                else
                {
                    ItemData = loadJobRequestBLL.GetItemsDatabyLoadId_BLL(companyid, loadid);
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



        [HttpGet]
        [Route("GetHandlingUnitByLoadId")]
        public HttpResponseMessage GetHandlingUnitByLoadId(int companyid, int loadid)
        {
            LoadJobRequestBLL loadJobRequestBLL = new LoadJobRequestBLL();
            try
            {
                List<ItemsModule> ItemData = loadJobRequestBLL.GetHandlingUnitByLoadId_BLL(companyid, loadid);
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
        [Route("GetVoucherByLoadId")]
        public HttpResponseMessage GetVoucherByLoadId(int loadid)
        {
            LoadJobRequestBLL userJobRequestBLL = new LoadJobRequestBLL();
            try
            {
                List<JobRequest> JobRequestData = userJobRequestBLL.GetVoucherByLoadId_BLL(loadid);
                output.ResponseCode = 100;
                output.Message = "Success";
                output.Data = JobRequestData;
                
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
        public bool SendEmailOnPickVoucher(JobRequest objjobreq, int companyid, String langcode)
        {
            try
            {
                MasterBLL masterBLL = new MasterBLL();
                //try
                //{
                //      companyid = objjobreq.br_compid;
                var mailConfig = masterBLL.getMailConfigBLL(companyid, "LoadPicked", langcode, objjobreq.deliverytypecode);
                var subject = new StringBuilder(mailConfig.Rows[0]["subject"] + "");
                var body = new StringBuilder(mailConfig.Rows[0]["body"] + "");
                var mailids = objjobreq.custemail + (string.IsNullOrEmpty(ConfigurationManager.AppSettings["ADMINEMAIL"]) ? "" : "," + ConfigurationManager.AppSettings["ADMINEMAIL"]);
                var loadid = objjobreq.loadid;
                //var trackingurl = string.Format("{0}{1}/{2}/{3}",
                //    System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority),
                //    voucherno, objjobreq.drivercode, objjobreq.jobscheduletime.ToString("ddMMyy")
                //    );
                var tracking_map_type = masterBLL.GetSysSettings_BLL(companyid, "tracking_map_type").Select(x => x.value).FirstOrDefault();
                 
                var trackingurl = string.Format("{0}/customer/{1}?loadid={2}&companyid={3}", System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority), (tracking_map_type == "google" ? "driverdetail.html" : "driverdetail_leaflet.html"), loadid, companyid);

                body.Replace("@customername", objjobreq.firmname);
                body.Replace("@drivername", objjobreq.drivername);
                body.Replace("@trackingurl", trackingurl);
                body.Replace("@scheduledate", objjobreq.jobscheduletime.ToString("dd-MM-yyyy"));
                body.Replace("@loadno", objjobreq.loadno);
                body.Replace("@loaddate", objjobreq.loaddate?.ToString("dd-MM-yyyy"));


                subject.Replace("@customername", objjobreq.firmname);
                subject.Replace("@drivername", objjobreq.drivername);
                subject.Replace("@trackingurl", trackingurl);
                subject.Replace("@scheduledate", objjobreq.jobscheduletime.ToString("dd-MM-yyyy"));
                subject.Replace("@loadno", objjobreq.loadno);
                subject.Replace("@loaddate", objjobreq.loaddate?.ToString("dd-MM-yyyy"));

                //@customername,@drivername,@trackingurl,@scheduledate,@loadno,@loaddate,@expecteddate

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
        public HttpResponseMessage SendVoucherReport(int companyid, int loadId, string loadno, string mailids, string langcode = "en", string deliverytype = "d")
        {
            JobRequestBLL userJobRequestBLL = new JobRequestBLL();
            MasterBLL masterBLL = new MasterBLL();
            try
            {
                var objjobreq = new JobRequest();
                var path_en = System.Web.HttpContext.Current.Server.MapPath("~/Files/" + companyid + "_" + loadId + "_" + "en" + "_report.pdf");
                var path_id = System.Web.HttpContext.Current.Server.MapPath("~/Files/" + companyid + "_" + loadId + "_" + "id" + "_report.pdf");

                if (!System.IO.File.Exists(path_id))
                    objjobreq = GenerateVoucher(companyid, loadId, "en");

                if (!System.IO.File.Exists(path_id))
                    objjobreq = GenerateVoucher(companyid, loadId, "id");

                if (objjobreq == null)
                {
                    return null;
                }

                var mailConfig = masterBLL.getMailConfigBLL(companyid, "LoadReport", langcode, deliverytype);

                var path = System.Web.HttpContext.Current.Server.MapPath("~/Files/" + companyid + "_" + loadId + "_" + langcode + "_report.pdf");

                var subject = new StringBuilder(mailConfig.Rows[0]["subject"] + "");
                var body = mailConfig.Rows[0]["body"] + "";
                mailids = mailids + (string.IsNullOrEmpty(ConfigurationManager.AppSettings["ADMINEMAIL"]) ? "" : "," + ConfigurationManager.AppSettings["ADMINEMAIL"]);


                body.Replace("@customername", objjobreq.firmname);
                body.Replace("@drivername", objjobreq.drivername);
                //body.Replace("@trackingurl", trackingurl);
                body.Replace("@scheduledate", objjobreq.jobscheduletime.ToString("dd-MM-yyyy"));
                body.Replace("@loadno", objjobreq.loadno);
                body.Replace("@loaddate", objjobreq.loaddate?.ToString("dd-MM-yyyy"));
                body.Replace("@deliverydate", objjobreq.actiontime.ToString("dd-MM-yyyy"));


                subject.Replace("@customername", objjobreq.firmname);
                subject.Replace("@drivername", objjobreq.drivername);
                //subject.Replace("@trackingurl", trackingurl);
                subject.Replace("@scheduledate", objjobreq.jobscheduletime.ToString("dd-MM-yyyy"));
                subject.Replace("@loadno", objjobreq.loadno);
                subject.Replace("@loaddate", objjobreq.loaddate?.ToString("dd-MM-yyyy"));
                subject.Replace("@deliverydate", objjobreq.actiontime.ToString("dd-MM-yyyy"));


                //voucherno
                //  new ; 
                // string reseltsendmail = userJobRequestBLL.SendVoucherReport_BLL(companyid, invoiceno, mailids);
                if (new NotificationBLL().SendEmailWithAttachment(companyid, subject.ToString(), body.Replace("@loadno", loadno), mailids, null, null, path))
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


        [HttpGet]
        [Route("GeneratePDF")]
        public JobRequest GenerateVoucher(int companyid, int loadId, string langcode)
        {
           // Helper.LogMessage("GenerateVoucherCalled");
            var filesPath = System.Web.HttpContext.Current.Server.MapPath("~/Files/");
            var path = System.Web.HttpContext.Current.Server.MapPath("~/Content/");
            var txtFile = new StringBuilder(File.ReadAllText(System.IO.Path.Combine(path, "LoadReportInvoice.html")));
            var footerHtml = new StringBuilder("<div style='text-align:center;width: 100%;'><div style='font-size:16px;'><b>@lbl_footer_line1</b></div> <div style='font-size:14px;'> @lbl_footer_line2 </div> </div>");

            var fileName = companyid + "_" + loadId + "_" + langcode + "_report.pdf";
            var fileWithPath = Path.Combine(filesPath, fileName);
            try
            {
                if (File.Exists(fileWithPath))
                {
                    File.Delete(fileWithPath);
                }
            }
            catch { }
            LoadJobRequestBLL userJobRequestBLL = new LoadJobRequestBLL();

            var row = userJobRequestBLL.GetLoadAndJobByLoadId_BLL(loadId).FirstOrDefault();

            if(row != null)// (voucherDetailTd.Rows.Count > 0)
            {
               // var row = voucherDetailTd.Rows[0];

        //        ItemsBLL objItemsBLL = new ItemsBLL();
                List<ItemsModule> ItemData = userJobRequestBLL.GetDeliveredItemLoad_BLL(row.jobid);


                txtFile.Replace("@companylogo", "data:image/png;base64," + new PODController().ImagePathToBase64(Path.Combine(filesPath, row.comapanyimage+"")));
                txtFile.Replace("@voucherno", row.loadno);
                txtFile.Replace("@orderdate", row.loaddate?.ToString("dd-MMM-yyyy HH:mm"));

                txtFile.Replace("@firmname", row.firmname);
                txtFile.Replace("@contactpersonname", row.custcontperson);
                txtFile.Replace("@contactno", row.custcontactno);
                txtFile.Replace("@address", row.address);

                txtFile.Replace("@empfirstname", row.empfirstname);
                txtFile.Replace("@emplastname", row.emplastname);  

                var ind = 0;
                var sbItems = new StringBuilder();
                if (ItemData.Count > 0)
                {
                    var g_handl_items = ItemData.GroupBy(x => new { x.handlingunitid, x.handlingunittitle });
                    foreach (var g_handl in g_handl_items)
                    {
                        int handl_ind = 0;
                        var g_vh_items = g_handl.GroupBy(x => new { x.voucherno });
                        foreach (var g_vh in g_vh_items)
                        {
                            int vh_ind = 0;
                            foreach (var item in g_vh.ToList())
                            {
                                ind++;
                                if (handl_ind == 0 && vh_ind == 0)
                                {
                                    sbItems.Append(string.Format("<tr><td rowspan='"+ g_handl.Count()+ "' valign='top'><b>{0}</b></td><td rowspan='" + g_vh.Count() + "'>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td><td>{7}</td><td>{8}</td><td>{9}</td></tr>",
                                    g_handl.Key.handlingunittitle, g_vh.Key.voucherno, item.itemcode, item.itemname, item.color, item.size, item.orderqty, item.itemuom, item.rejectedqty == 0 ? null : (decimal?)item.rejectedqty, item.rejectedreason
                                    ));
                                    handl_ind++;
                                    vh_ind++;
                                }
                                else if(vh_ind == 0)
                                {
                                    sbItems.Append(string.Format("<tr><td rowspan='" + g_vh.Count() + "' valign='top'><b>{0}</b></td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td><td>{7}</td><td>{8}</td></tr>",
                                    //g_handl.Key.handlingunittitle,
                                    g_vh.Key.voucherno, item.itemcode, item.itemname, item.color, item.size, item.orderqty, item.itemuom, item.rejectedqty == 0 ? null : (decimal?)item.rejectedqty, item.rejectedreason
                                    ));
                                    vh_ind++;
                                }
                                else
                                {
                                    sbItems.Append(string.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td><td>{7}</td></tr>",
                                    //g_handl.Key.handlingunittitle, g_vh.Key.voucherno, 
                                    item.itemcode, item.itemname, item.color, item.size, item.orderqty, item.itemuom, item.rejectedqty == 0 ? null : (decimal?)item.rejectedqty, item.rejectedreason
                                    ));
                                }

                                
                            }
                        }
                       
                    }
                  
                }
                txtFile.Replace("@items", sbItems.ToString());

                txtFile.Replace("@vsname", row.voucherstatus);// row["vsname"] + "");

                txtFile.Replace("@actiontime", row.actiontime.ToString("dd-MMM-yyyy HH:mm"));// row["actiontime"] + "");

                txtFile.Replace("@vehiclenumber", row.vehiclenumber);// row["vehiclenumber"] + "");

                txtFile.Replace("@approvername", row.approvername);

                txtFile.Replace("@latitude", row.latitude);
                txtFile.Replace("@longitude", row.longitude);


                try
                {
                    int jaid = row.jaid;
                    int jobid = row.jobid;

                    //int.TryParse(row.jaid + "", out jaid);
                    //int.TryParse(row["jobid"] + "", out jobid);

                    List<JobImagesModule> jobimages = new JobRequestBLL().getJobimages_BLL(jaid, jobid);

                    var driver = jobimages.Where(x => x.remark == "Driver").FirstOrDefault()?.image;
                    var receiver = jobimages.Where(x => x.remark == "Receiver").FirstOrDefault()?.image;

                    var others = jobimages.Where(x => x.remark == "" || x.remark == null).ToList();

                    txtFile.Replace("@driversignature", "data:image/png;base64," + new PODController().ImagePathToBase64(Path.Combine(filesPath, (driver + "").Split('/').Last())));
                    txtFile.Replace("@receiversignature", "data:image/png;base64," + new PODController().ImagePathToBase64(Path.Combine(filesPath, (receiver + "").Split('/').Last())));

                    var sbOtherImages = new StringBuilder();
                    foreach (var other in others)
                    {
                        var base64Img = "data:image/png;base64," + new PODController().ImagePathToBase64(Path.Combine(filesPath, (other.image + "").Split('/').Last()));
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


                    var voucherLBLPath = File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath(String.Format("~/content/lang/{0}_lang_load.json", langcode)));
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

                new PODController().HtmlToPDFItext7(txtFile.ToString(), filesPath, fileName, true, footerHtml.ToString());
            }

            return row;
        }



    }

}

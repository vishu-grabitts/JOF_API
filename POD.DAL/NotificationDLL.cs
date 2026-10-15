using POD.Models;
using SendGrid;
using SendGrid.SmtpApi;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;

namespace POD.DAL
{
    public class NotificationDLL : BaseClass
    {
        public async Task<string> PostFormUrlEncoded(string url, List<KeyValuePair<string, string>> postData)
        {
            using (var httpClient = new System.Net.Http.HttpClient())
            {
                using (var content = new System.Net.Http.FormUrlEncodedContent(postData))
                {
                    content.Headers.Clear();
                    content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

                    System.Net.Http.HttpResponseMessage response = await httpClient.PostAsync(url, content);

                    return await response.Content.ReadAsStringAsync();
                }
            }
        }


        protected bool SendnotificationByActionDLL(JobRequest objjobreq, Notification oNotification)
        {
            int i = 0;
            try
            {
                using (SqlConnection Con = new SqlConnection(constring))
                {
                    string Query = string.Empty;
                    DataTable notificationDT = new DataTable();
                    DataTable dt_Mail = new DataTable();
                    DataSet ds = new DataSet();
                    using (SqlCommand cmd = new SqlCommand("getmessagedetailsbyaction", Con))
                    {
                        cmd.Parameters.AddWithValue("@actionid", oNotification.actionid);
                        cmd.Parameters.AddWithValue("@notificationtype", "sms");
                        cmd.Parameters.AddWithValue("@compid", oNotification.companyid);
                        cmd.Parameters.AddWithValue("@deliverytype", objjobreq.deliverytypecode);
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter sd = new SqlDataAdapter(cmd);
                        Con.Open();
                        sd.Fill(ds);
                        Con.Close();
                        if (ds.Tables.Count > 0)
                        {
                            notificationDT = ds.Tables[0];
                            dt_Mail = ds.Tables[1];
                        }
                        if (notificationDT.Rows.Count > 0)
                        {
                            try
                            {
                                MasterDLL masterBLL = new MasterDLL();

                                var tracking_map_type = masterBLL.GetSysSettings_DLL(oNotification.companyid, "tracking_map_type").Select(x => x.value).FirstOrDefault();

                                string trackingurl = "";
                                if (objjobreq.rectype == "l")
                                {
                                    trackingurl = string.Format("{0}/customer/{1}?loadid={2}&companyid={3}", System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority), (tracking_map_type == "google" ? "driverdetail.html" : "driverdetail_leaflet.html"), objjobreq.loadid, oNotification.companyid);
                                }
                                else
                                {
                                    trackingurl = string.Format("{0}/customer/{1}?voucherno={2}&companyid={3}", System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority), (tracking_map_type == "google" ? "driverdetail.html" : "driverdetail_leaflet.html"), objjobreq.voucherno, oNotification.companyid);
                                }

                                string username = notificationDT.Rows[0]["username"].ToString();
                                string password = notificationDT.Rows[0]["password"].ToString();
                                string type = notificationDT.Rows[0]["type"].ToString();
                                string sender = notificationDT.Rows[0]["sender"].ToString();
                                //string mobile = "9540123870";
                                string mobile = objjobreq.custcontactno;// "9113310233";
                                string message = notificationDT.Rows[0]["message"].ToString();
                                string scheduledate = objjobreq.jobscheduletime.ToString("dd-MM-yyyy");
                                string messagefinal = message.Replace("@customername", objjobreq.firmname).Replace("@orderno", objjobreq.orderno).Replace("@loadno", objjobreq.loadno).Replace("@scheduledate", scheduledate).Replace("@trackingurl", trackingurl);

                                ////HttpWebRequest req = (HttpWebRequest)WebRequest.Create(System.Configuration.ConfigurationManager.AppSettings["MaterialReceive"].ToString());
                                //HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://bulksms.analyticsmantra.com/sendsms/sendsms.php?username=" + username + "&password=" + password + "&type=" + type + "&sender=" + sender + "&mobile=" + mobile + "&message=" + messagefinal + "");
                                //req.ContentType = "text/xml;charset=\"utf-8\"";
                                //req.Accept = "text/xml";
                                //req.Method = "POST";

                                //Stream stm = req.GetRequestStream();
                                ////doc.Save(stm);
                                ////stm.Close();

                                //WebResponse resp = req.GetResponse();
                                //stm = resp.GetResponseStream();
                                //StreamReader r = new StreamReader(stm);
                                //string result = r.ReadToEnd();
                                ////SendEmail(objjobreq,dt_Mail);

                                //" + username + "&password=" + password + "&type=" + type + "&sender=" + sender + "&mobile=" + mobile + "&message=" + messagefinal + ""
                                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
                                parameters.Add(new KeyValuePair<string, string>("username", username));
                                parameters.Add(new KeyValuePair<string, string>("password", password));
                                parameters.Add(new KeyValuePair<string, string>("type", type));
                                parameters.Add(new KeyValuePair<string, string>("sender", sender));
                                parameters.Add(new KeyValuePair<string, string>("mobile", mobile));
                                parameters.Add(new KeyValuePair<string, string>("message", messagefinal));

                                Task.Run(async () => { await PostFormUrlEncoded("https://bulksms.analyticsmantra.com/sendsms/sendsms.php", parameters); });

                                i = 1;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            if (i >= 1) return true; else return false;
        }

        //public void SendEmail(JobRequest oJobrequest, DataTable dt_Mail)
        //{
        //    try
        //    {
        //        using (StringWriter sw = new StringWriter())
        //        {
        //            using (HtmlTextWriter hw = new HtmlTextWriter(sw))
        //            {
        //                using (MemoryStream memoryStream = new MemoryStream())
        //                {
        //                    byte[] bytes = memoryStream.ToArray();
        //                    memoryStream.Close();

        //                    if (dt_Mail.Rows.Count > 0)
        //                    {
        //                        MailMessage mail = new MailMessage();
        //                        string Body = string.Empty;
        //                        string Subject = string.Empty;
        //                        string From = string.Empty;
        //                        string Password = string.Empty;
        //                        string To = string.Empty;
        //                        string CC = string.Empty;
        //                        string message = dt_Mail.Rows[0]["body"].ToString().Trim();
        //                        string scheduledate = oJobrequest.jobscheduletime.ToString("dd-MM-yyyy");
        //                        string messagefinal = message.Replace("@customername", oJobrequest.firmname).Replace("@orderno", oJobrequest.orderno).Replace("@scheduledate", scheduledate);
        //                        string smtphost = dt_Mail.Rows[0]["hostname"].ToString().Trim();
        //                        bool smtpenablessl = Convert.ToBoolean(dt_Mail.Rows[0]["enablessl"].ToString());
        //                        string smtpport = dt_Mail.Rows[0]["portnumber"].ToString().Trim();
        //                        Subject = dt_Mail.Rows[0]["subject"].ToString().Trim();
        //                        From = dt_Mail.Rows[0]["frommailid"].ToString().Trim();
        //                        Password = dt_Mail.Rows[0]["password"].ToString().Trim();
        //                        Body = messagefinal;
        //                        To = "sandeep.ng999@gmail.com";
        //                        CC = "sandeep.ng9@gmail.com";
        //                        mail.CC.Add(CC);
        //                        mail.To.Add(To);
        //                        mail.From = new MailAddress(From);
        //                        mail.Body = Body;
        //                        mail.Subject = Subject;
        //                        //mail.Attachments.Add(new Attachment(new MemoryStream(bytes), "MaterialReceived.pdf"));
        //                        //System.Net.Mail.Attachment attachment;
        //                        //attachment = new System.Net.Mail.Attachment(filename); //Attaching File to Mail  
        //                        //mail.Attachments.Add(attachment);
        //                        mail.IsBodyHtml = true;
        //                        SmtpClient smtp = new SmtpClient();
        //                        smtp.Host = smtphost;
        //                        smtp.EnableSsl = smtpenablessl;
        //                        smtp.Port = Convert.ToInt32(smtpport);
        //                        smtp.Credentials = new System.Net.NetworkCredential(From, Password);
        //                        smtp.SendAsync(mail, null);

        //                    }
        //                }
        //            }
        //        }
        //    }catch(Exception ex)
        //    {

        //    }
        //}

        public bool SendEmail(int companyid, string sSubject, string sBody, string sMailTo, string sMailCC, string sMailBCC, string attachment = null)
        {
            bool mailsent = false;
            try
            {
                using (StringWriter sw = new StringWriter())
                {
                    using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                    {
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            byte[] bytes = memoryStream.ToArray();
                            memoryStream.Close();

                            DataTable dt_Mail = new DataTable();
                            using (SqlConnection Con = new SqlConnection(constring))
                            {
                                string Query = string.Empty;
                                Query = @"SELECT [serverid],[companyid],[servername],[hostname],[portnumber],[frommailid],[password],[enablessl],[timeout],[isactive],[isdeleted],[createdby],[createdon],[modifyby],[modifyon] FROM [sys_mailserver] WHERE (companyid = @compid OR companyid IS NULL) and isactive = 1 ORDER BY companyid DESC";
                                using (SqlCommand cmd = new SqlCommand(Query, Con))
                                {
                                    cmd.Parameters.AddWithValue("@compid", companyid);
                                    cmd.CommandType = CommandType.Text;
                                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                                    Con.Open();
                                    sd.Fill(dt_Mail);
                                    Con.Close();
                                }
                            }
                            if (dt_Mail.Rows.Count == 0)
                            {
                                Helper.LogMessage("Email not configured");
                            }

                            if (dt_Mail.Rows.Count > 0 && dt_Mail.Rows[0]["servername"].ToString() == "smtp")
                            {
                                MailMessage mail = new MailMessage();
                                //string Body = string.Empty;
                                //string Subject = string.Empty;
                                string From = string.Empty;
                                string Password = string.Empty;
                                // string To = string.Empty;
                                // string CC = string.Empty;
                                //string message = dt_Mail.Rows[0]["body"].ToString().Trim();
                                string smtphost = dt_Mail.Rows[0]["hostname"].ToString().Trim();
                                bool smtpenablessl = Convert.ToBoolean(dt_Mail.Rows[0]["enablessl"].ToString());
                                string smtpport = dt_Mail.Rows[0]["portnumber"].ToString().Trim();
                                From = dt_Mail.Rows[0]["frommailid"].ToString().Trim();
                                Password = dt_Mail.Rows[0]["password"].ToString().Trim();
                                //To = "sandeep.ng999@gmail.com";
                                //CC = "sandeep.ng9@gmail.com";
                                //if(!String.IsNullOrEmpty(sMailCC))
                                //{
                                //    mail.CC.Add(sMailCC);

                                //}
                                //mail.To.Add(sMailTo);


                                if (!string.IsNullOrEmpty(sMailCC))
                                {
                                    foreach (var emailId in sMailCC.Split(','))
                                    {
                                        mail.CC.Add(emailId.Trim());
                                    }
                                }

                                if (!string.IsNullOrEmpty(sMailTo))
                                {
                                    foreach (var emailId in sMailTo.Split(','))
                                    {
                                        mail.To.Add(emailId);
                                    }
                                }

                                if (!string.IsNullOrEmpty(attachment))
                                {
                                    foreach (var attach in attachment.Split(','))
                                    {
                                        mail.Attachments.Add(new Attachment(new MemoryStream(File.ReadAllBytes(attachment)), "Proof_of_Delivery.pdf"));
                                        //System.Net.Mail.Attachment _attachment = new System.Net.Mail.Attachment(attach);
                                        //    mail.Attachments.Add(_attachment);
                                    }
                                }


                                mail.From = new MailAddress(From);
                                mail.Body = sBody;
                                mail.Subject = sSubject;
                                //mail.Attachments.Add(new Attachment(new MemoryStream(bytes), "MaterialReceived.pdf"));
                                //System.Net.Mail.Attachment attachment;
                                //attachment = new System.Net.Mail.Attachment(filename); //Attaching File to Mail  
                                //mail.Attachments.Add(attachment);
                                mail.IsBodyHtml = true;
                                //if (smtpenablessl)
                                //{
                                //    System.Net.ServicePointManager.SecurityProtocol |= System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;
                                //}

                                SmtpClient smtp = new SmtpClient();
                                smtp.Host = smtphost;
                                smtp.Port = Convert.ToInt32(smtpport);
                                smtp.Timeout = 2000000;
                                //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                                smtp.UseDefaultCredentials = false;
                                smtp.Credentials = new System.Net.NetworkCredential(From, Password);
                                smtp.EnableSsl = smtpenablessl;
                                smtp.Send(mail);
                                //  smtp.SendAsync(mail, null);
                                //   smtp.SendMailAsync(mail);
                                //       smtp.Dispose();
                                mailsent = true;
                            }
                            if (dt_Mail.Rows.Count > 0 && dt_Mail.Rows[0]["servername"].ToString() == "sendgrid")
                            {
                                mailsent = true;
                                SendMailSandGridWebAPI(dt_Mail.Rows[0]["frommailid"].ToString().Trim(), dt_Mail.Rows[0]["frommailid"].ToString().Trim(), dt_Mail.Rows[0]["password"].ToString().Trim(),
                                    sSubject, sBody, sMailTo.Split(',').ToList(), null, null, attachment);
                            }
                        }
                    }
                    return mailsent;
                }
            }
            catch (Exception ex)
            {
                Helper.LogException(ex, "MailBLL");

                return mailsent;
            }
        }

        public bool SendMailSandGridWebAPI(string sendgrid_fromname, string sendgrid_fromemail, string sendgrid_apikey, string sSubject, string sBody, List<string> lstEmails, string sMailCC = null, string sMailBCC = null, string sFileName = null)
        {
            // return true;  //Comment On Production Level
            bool isSuccess = false;
            try
            {
                var header = new Header();
                header.SetTo(lstEmails.Select(x => (x + "").Trim()).ToList());

                // Create the email object first, then add the properties.
                SendGridMessage oSendGridMessage = new SendGridMessage();

                //Add header to send indivisual receipant
                oSendGridMessage.Headers.Add("X-SMTPAPI", header.JsonString());

                for (int i = 0; i < lstEmails.Count; i++)
                {
                    oSendGridMessage.AddTo((lstEmails[i] + "").Trim().ToString());
                }

                if (!string.IsNullOrEmpty(sMailCC))
                {
                    oSendGridMessage.AddCc(sMailCC);
                }

                if (!string.IsNullOrEmpty(sMailBCC))
                {
                    oSendGridMessage.AddBcc(sMailBCC);
                }

                var name = sendgrid_fromname;// System.Configuration.ConfigurationManager.AppSettings["SENDGRID_Name"];
                var email = sendgrid_fromemail;//System.Configuration.ConfigurationManager.AppSettings["SENDGRID_EMAIL"];
                var apikey = sendgrid_apikey;//System.Configuration.ConfigurationManager.AppSettings["SENDGRID_API_KEY"];

                //Replace with sendar email address
                oSendGridMessage.From = new MailAddress(email, name);
                oSendGridMessage.Subject = sSubject;
                oSendGridMessage.Html = sBody;
                //Attachement for single/multiple files
                if (!string.IsNullOrEmpty(sFileName))
                {
                    if (sFileName.Length > 0)
                    {
                        string[] sFiles = sFileName.Split(new char[] { ',' });
                        foreach (string sFile in sFiles)
                        {
                            if (!string.IsNullOrEmpty(sFile))
                            {
                                oSendGridMessage.AddAttachment(sFile);
                            }
                        }
                    }
                }
                var transportWeb = new SendGrid.Web(apikey);
                // Send the email.
                var res = transportWeb.DeliverAsync(oSendGridMessage);

                //Success message
                isSuccess = true;
            }
            catch (Exception ex)
            {
                Helper.LogException(ex, "MAILBLL");
                //throw;
            }

            return isSuccess;
        }
    }
}

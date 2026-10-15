using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POD.Models;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.IO;
using System.Drawing;
using System.Net;
using System.Net.Mail;

namespace POD.DAL
{
    public class JobRequestDLL : BaseClass
    {
        public string SendVoucherReport_DLL(int companyid,string invoiceno, string mailids)
        {
            string Result = string.Empty;
            try
            {
                using (SqlConnection Con = new SqlConnection(constring))
                {
                    string Query = string.Empty;
                    Query = @"SELECT reporturl FROM sys_company WHERE companyid = @compid";
                    DataTable dt = new DataTable();
                    using (SqlCommand cmd = new SqlCommand(Query, Con))
                    {
                        cmd.Parameters.AddWithValue("@compid", companyid);
                        cmd.CommandType = CommandType.Text;
                        SqlDataAdapter sd = new SqlDataAdapter(cmd);
                        Con.Open();
                        sd.Fill(dt);
                        Con.Close();
                    }

                    if(dt.Rows.Count>0 &&  !String.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["reporturl"])))
                    {
                        string html = string.Empty;
                        //string url = @"http://localhost/PODWEB/Pages/VoucherReport?InvoiceNo=" + invoiceno + "&Mail=" + mailids;
                        //string url = @"http://localhost:9088/Reports/Quotation.aspx?CallID=" + CallID + "&Mail=Yes&MailID=" + MailID + "&Attn1=" + att1 + "&Attn2=" + att2 + "&Subject=" + subject + "&QtDate=" + reportdate;
                        string url = dt.Rows[0]["reporturl"].ToString()+"?InvoiceNo=" + invoiceno + "&Mail=" + mailids;
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                        request.AutomaticDecompression = DecompressionMethods.GZip;
                        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                        using (Stream stream = response.GetResponseStream())
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            Result = "Success";
                        }
                        Result = "Success";
                    }

                  
                }
            }
            catch (Exception ex)
            {
                Result = "Error :" + ex.StackTrace.ToString();
            }


            return Result;
        }
        protected List<JobRequest> getJobrequest_DLL(int userid, int branchid, int empcode)
        {

            List<JobRequest> JobRequestList = new List<JobRequest>();
            using (SqlConnection Con = new SqlConnection(constring))
            {
                string Query = string.Empty;
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("getJobrequest", Con))
                {
                    cmd.Parameters.AddWithValue("@userid", userid);
                    //cmd.Parameters.AddWithValue("@branchid", branchid);
                    cmd.Parameters.AddWithValue("@empid", empcode);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    Con.Open();
                    sd.Fill(dt);
                    Con.Close();
                }
                JobRequestList = ConvertDataTable<JobRequest>(dt);
            }
            return JobRequestList;
        }

        protected List<dashboardcounter> GetCounter_DLL(int userid, string branchcode, int empcode, int roleid)
        {

            List<dashboardcounter> objCounter = new List<dashboardcounter>();
            using (SqlConnection Con = new SqlConnection(constring))
            {
                string Query = string.Empty;
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("getcounterdata", Con))
                {
                    cmd.Parameters.AddWithValue("@userid", userid);
                    //cmd.Parameters.AddWithValue("@branchcode", branchcode);
                    cmd.Parameters.AddWithValue("@empid", empcode);
                    cmd.Parameters.AddWithValue("@roleid", roleid);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    Con.Open();
                    sd.Fill(dt);
                    Con.Close();
                }
                objCounter = ConvertDataTable<dashboardcounter>(dt);
            }
            return objCounter;
        }
        protected List<JobRequest> getJobrequestData_DLL(int roleid, int userid, string branchcode, int empcode, string type, long tagno)
        {

            List<JobRequest> JobRequestList = new List<JobRequest>();
            using (SqlConnection Con = new SqlConnection(constring))
            {
                string Query = string.Empty;
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("getJobrequestdata_New", Con))
                {
                    cmd.Parameters.AddWithValue("@roleid", roleid);
                    cmd.Parameters.AddWithValue("@userid", userid);
                    //cmd.Parameters.AddWithValue("@branchcode", branchcode);
                    cmd.Parameters.AddWithValue("@empid", empcode);
                    cmd.Parameters.AddWithValue("@type", type);
                    cmd.Parameters.AddWithValue("@tagno", tagno);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    Con.Open();
                    sd.Fill(dt);
                    Con.Close();
                }
                JobRequestList = ConvertDataTable<JobRequest>(dt);
            }
            return JobRequestList;
        }

        protected List<JobRequest> getUserOfflineData_DLL(int roleid, int userid, string branchcode, int empcode, string type)
        {

            List<JobRequest> JobRequestList = new List<JobRequest>();
            using (SqlConnection Con = new SqlConnection(constring))
            {
                string Query = string.Empty;
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("getuserofflinedata", Con))
                {
                    cmd.Parameters.AddWithValue("@roleid", roleid);
                    cmd.Parameters.AddWithValue("@userid", userid);
                    //cmd.Parameters.AddWithValue("@branchcode", branchcode);
                    cmd.Parameters.AddWithValue("@empid", empcode);
                    cmd.Parameters.AddWithValue("@type", type);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    Con.Open();
                    sd.Fill(dt);
                    Con.Close();
                }
                JobRequestList = ConvertDataTable<JobRequest>(dt);
            }
            return JobRequestList;
        }
        protected List<JobRequestMultiple> SaveJobActionMultiple_DLL(int userid, JobRequestMultiple objjobreq)
        {
            List<JobRequestMultiple> JobRequestList = new List<JobRequestMultiple>();
            using (SqlConnection Con = new SqlConnection(constring))
            {
                string Query = string.Empty;
                DataTable dt = new DataTable();
                foreach (var item in objjobreq.JobRequestModel)
                {


                    using (SqlCommand cmd = new SqlCommand("SaveJobActionNew", Con))
                    {
                        string signatureimagepath = "";
                        if (item.status == 5 || item.status == 6)
                        {
                            if (item.signature.IndexOf("http") < 0)
                            {
                                Image image = null;
                                try
                                {
                                    if (item.signature.IndexOf("base64") > 0)
                                    {
                                        string FileName = "";
                                        string DataUrl = item.signature.Split(',')[1].ToString();
                                        string fileName = FileName;
                                        string path = HttpContext.Current.Server.MapPath("~/Files");
                                        string fileExtension = Path.GetExtension(fileName);
                                        string sfileName = Path.GetFileNameWithoutExtension(fileName) + "_" + item.jaid.ToString() + "_" + System.DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss").Replace(":", "").Replace("/", "") + fileExtension;
                                        string sFilePath = path + "/" + sfileName;
                                        string fileNameLocation = Path.Combine(path, sfileName);
                                        var bytes = Convert.FromBase64String(DataUrl);
                                        using (var imageFile = new MemoryStream(bytes, 0, bytes.Length))
                                        {
                                            image = Image.FromStream(imageFile, true);
                                            image.Save(fileNameLocation + ".png", System.Drawing.Imaging.ImageFormat.Png);
                                            image.Dispose();
                                        }
                                        signatureimagepath = "" + sfileName + ".png";
                                    }
                                }
                                catch { }
                                finally
                                {
                                    if (image != null)
                                    {
                                        image.Dispose();
                                    }
                                }
                            }
                            //if (objjobreq.status == 5)
                            //{
                            //    if ((objjobreq.shplatitude+"").Trim() != "" && (objjobreq.shplongitude+"").Trim() != "")
                            //    {
                            //        bool locationresult = Saveuserlocation_Dll(objjobreq.customercode, objjobreq.shplongitude, objjobreq.shplatitude);
                            //    }
                            //}
                        }
                        else
                        {
                            signatureimagepath = item.signature;
                        }

                        cmd.Parameters.AddWithValue("@userid", userid);
                        cmd.Parameters.AddWithValue("@vhid", item.vhid);
                        cmd.Parameters.AddWithValue("@jobid", item.jobid);
                        cmd.Parameters.AddWithValue("@empcode", item.drivercode);
                        cmd.Parameters.AddWithValue("@actioncode", item.status);
                        cmd.Parameters.AddWithValue("@scheduletime", item.jobscheduletime);
                        cmd.Parameters.AddWithValue("@rejectremark", item.jobrejectremark);
                        cmd.Parameters.AddWithValue("@signature", signatureimagepath);
                        cmd.Parameters.AddWithValue("@collectedby", item.collectedby);
                        cmd.Parameters.AddWithValue("@latitude", item.latitude);
                        cmd.Parameters.AddWithValue("@longitude", item.longitude);
                        cmd.Parameters.AddWithValue("@rrid", item.rrid);
                        cmd.Parameters.AddWithValue("@approvername", item.approvername);
                        cmd.Parameters.AddWithValue("@voucherno", item.voucherno);
                        cmd.Parameters.AddWithValue("@companyid", item.br_compid);

                        cmd.Parameters.AddWithValue("@deliveryaddrtype", item.deliveryaddrtype);
                        cmd.Parameters.AddWithValue("@actiontime", item.actiontime);


                        var addr = new GeoLocationServices().GetAddress(item.latitude, item.longitude);

                        cmd.Parameters.AddWithValue("@zipcode", (addr == null || string.IsNullOrEmpty(addr.zipcode)) ? " " : addr.zipcode);
                        cmd.Parameters.AddWithValue("@city", (addr == null || string.IsNullOrEmpty(addr.city)) ? " " : addr.city);
                        cmd.Parameters.AddWithValue("@state", (addr == null || string.IsNullOrEmpty(addr.state)) ? " " : addr.state);
                        cmd.Parameters.AddWithValue("@country", (addr == null || string.IsNullOrEmpty(addr.country)) ? " " : addr.country);
                        cmd.Parameters.AddWithValue("@address", (addr == null || string.IsNullOrEmpty(addr.address)) ? " " : addr.address);

                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter sd = new SqlDataAdapter(cmd);
                        Con.Open();
                        sd.Fill(dt);
                        Con.Close();
                    }
                }
                JobRequestList = ConvertDataTable<JobRequestMultiple>(dt);
                if (dt != null && dt.Rows.Count > 0)
                {

                    JobRequestList[0].jobid = Convert.IsDBNull(dt.Rows[0]["jobid"]) ? default(int) : Convert.ToInt32(dt.Rows[0]["jobid"]);
                    JobRequestList[0].jobactionid = Convert.IsDBNull(dt.Rows[0]["jobactionid"]) ? default(int) : Convert.ToInt32(dt.Rows[0]["jobactionid"]);

                }
            }
            return JobRequestList;
        }
        //SaveJobAction
        protected List<JobRequest> SaveJobAction_DLL(int userid, JobRequest objjobreq)
        {
            List<JobRequest> JobRequestList = new List<JobRequest>();
            using (SqlConnection Con = new SqlConnection(constring))
            {
                string Query = string.Empty;
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("SaveJobAction", Con))
                {
                    string signatureimagepath = "";
                    if (objjobreq.status == 5 || objjobreq.status == 6)
                    {
                        if (objjobreq.signature.IndexOf("http") < 0)
                        {
                            if (objjobreq.signature.IndexOf("base64") > 0)
                            {
                                string FileName = "";
                                string DataUrl = objjobreq.signature.Split(',')[1].ToString();
                                string fileName = FileName;
                                string path = HttpContext.Current.Server.MapPath("~/Files");
                                string fileExtension = Path.GetExtension(fileName);
                                string sfileName = Path.GetFileNameWithoutExtension(fileName) + "_" + objjobreq.jaid.ToString() + "_" + System.DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss").Replace(":", "").Replace("/", "") + fileExtension;
                                string sFilePath = path + "/" + sfileName;
                                string fileNameLocation = Path.Combine(path, sfileName);
                                var bytes = Convert.FromBase64String(DataUrl);
                                using (var imageFile = new MemoryStream(bytes, 0, bytes.Length))
                                {
                                    Image image = Image.FromStream(imageFile, true);
                                    image.Save(fileNameLocation + ".png", System.Drawing.Imaging.ImageFormat.Png);
                                }
                                signatureimagepath = "" + sfileName + ".png";
                            }
                        }
                        if (objjobreq.status == 5)
                        {
                            if (objjobreq.longitude.Trim() != "" && objjobreq.latitude.Trim() != "")
                            {
                                bool locationresult = Saveuserlocation_Dll(objjobreq.customercode, objjobreq.longitude, objjobreq.latitude);
                            }
                        }
                    }
                    else
                    {
                        signatureimagepath = objjobreq.signature;
                    }

                    cmd.Parameters.AddWithValue("@userid", userid);
                    cmd.Parameters.AddWithValue("@vhid", objjobreq.vhid);
                    cmd.Parameters.AddWithValue("@jobid", objjobreq.jobid);
                    cmd.Parameters.AddWithValue("@empcode", objjobreq.drivercode);
                    cmd.Parameters.AddWithValue("@actioncode", objjobreq.status);
                    cmd.Parameters.AddWithValue("@scheduletime", objjobreq.jobscheduletime);
                    cmd.Parameters.AddWithValue("@rejectremark", objjobreq.jobrejectremark);
                    cmd.Parameters.AddWithValue("@signature", signatureimagepath);
                    cmd.Parameters.AddWithValue("@collectedby", objjobreq.collectedby);
                    cmd.Parameters.AddWithValue("@latitude", objjobreq.latitude);
                    cmd.Parameters.AddWithValue("@longitude", objjobreq.longitude);
                    cmd.Parameters.AddWithValue("@rrid", objjobreq.rrid);
                    cmd.Parameters.AddWithValue("@approvername", objjobreq.approvername);

                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    Con.Open();
                    sd.Fill(dt);
                    Con.Close();
                }
                JobRequestList = ConvertDataTable<JobRequest>(dt);
                if (dt != null && dt.Rows.Count > 0)
                {
                    JobRequestList[0].jobid = Convert.IsDBNull(dt.Rows[0]["jobid"]) ? default(int) : Convert.ToInt32(dt.Rows[0]["jobid"]);
                    JobRequestList[0].jobactionid = Convert.IsDBNull(dt.Rows[0]["jobactionid"]) ? default(int) : Convert.ToInt32(dt.Rows[0]["jobactionid"]);
                }
            }
            return JobRequestList;
        }

        protected List<JobRequest> SaveJobActionNew_DLL(int userid, JobRequest objjobreq)
        {
            List<JobRequest> JobRequestList = new List<JobRequest>();
            using (SqlConnection Con = new SqlConnection(constring))
            {
                string Query = string.Empty;
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("SaveJobActionNew", Con))
                {
                    string signatureimagepath = "";
                    if (objjobreq.status == 5 || objjobreq.status == 6)
                    {
                        if (objjobreq.signature.IndexOf("http") < 0)
                        {
                            Image image = null;
                            try
                            {
                                if (objjobreq.signature.IndexOf("base64") > 0)
                                {
                                    string FileName = "";
                                    string DataUrl = objjobreq.signature.Split(',')[1].ToString();
                                    string fileName = FileName;
                                    string path = HttpContext.Current.Server.MapPath("~/Files");
                                    string fileExtension = Path.GetExtension(fileName);
                                    string sfileName = Path.GetFileNameWithoutExtension(fileName) + "_" + objjobreq.jaid.ToString() + "_" + System.DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss").Replace(":", "").Replace("/", "") + fileExtension;
                                    string sFilePath = path + "/" + sfileName;
                                    string fileNameLocation = Path.Combine(path, sfileName);
                                    var bytes = Convert.FromBase64String(DataUrl);
                                    using (var imageFile = new MemoryStream(bytes, 0, bytes.Length))
                                    {
                                        image = Image.FromStream(imageFile, true);
                                        image.Save(fileNameLocation + ".png", System.Drawing.Imaging.ImageFormat.Png);
                                        image.Dispose();
                                    }
                                    signatureimagepath = "" + sfileName + ".png";
                                }
                            }
                            catch { }
                            finally
                            {
                               if (image != null)
                               {
                                    image.Dispose();
                               }
                            }
                        }
                        //if (objjobreq.status == 5)
                        //{
                        //    if ((objjobreq.shplatitude+"").Trim() != "" && (objjobreq.shplongitude+"").Trim() != "")
                        //    {
                        //        bool locationresult = Saveuserlocation_Dll(objjobreq.customercode, objjobreq.shplongitude, objjobreq.shplatitude);
                        //    }
                        //}
                    }
                    else
                    {
                        signatureimagepath = objjobreq.signature;
                    }

                    cmd.Parameters.AddWithValue("@userid", userid);
                    cmd.Parameters.AddWithValue("@vhid", objjobreq.vhid);
                    cmd.Parameters.AddWithValue("@jobid", objjobreq.jobid);
                    cmd.Parameters.AddWithValue("@empcode", objjobreq.drivercode);
                    cmd.Parameters.AddWithValue("@actioncode", objjobreq.status);
                    cmd.Parameters.AddWithValue("@scheduletime", objjobreq.jobscheduletime);
                    cmd.Parameters.AddWithValue("@rejectremark", objjobreq.jobrejectremark);
                    cmd.Parameters.AddWithValue("@signature", signatureimagepath);
                    cmd.Parameters.AddWithValue("@collectedby", objjobreq.collectedby);
                    cmd.Parameters.AddWithValue("@latitude", objjobreq.latitude);
                    cmd.Parameters.AddWithValue("@longitude", objjobreq.longitude);
                    cmd.Parameters.AddWithValue("@rrid", objjobreq.rrid);
                    cmd.Parameters.AddWithValue("@approvername", objjobreq.approvername);
                    cmd.Parameters.AddWithValue("@voucherno", objjobreq.voucherno);
                    //cmd.Parameters.AddWithValue("@seqno", objjobreq.seqno);
                    cmd.Parameters.AddWithValue("@companyid", objjobreq.br_compid);

                    cmd.Parameters.AddWithValue("@deliveryaddrtype", objjobreq.deliveryaddrtype);
                    cmd.Parameters.AddWithValue("@actiontime", objjobreq.actiontime);
                    

                    var addr = new GeoLocationServices().GetAddress(objjobreq.latitude, objjobreq.longitude);

                    cmd.Parameters.AddWithValue("@zipcode", (addr == null || string.IsNullOrEmpty(addr.zipcode)) ? " " : addr.zipcode);
                    cmd.Parameters.AddWithValue("@city", (addr == null || string.IsNullOrEmpty(addr.city)) ? " " : addr.city);
                    cmd.Parameters.AddWithValue("@state", (addr == null || string.IsNullOrEmpty(addr.state)) ? " " : addr.state);
                    cmd.Parameters.AddWithValue("@country", (addr == null || string.IsNullOrEmpty(addr.country)) ? " ": addr.country);
                    cmd.Parameters.AddWithValue("@address", (addr == null || string.IsNullOrEmpty(addr.address)) ? " " : addr.address);

                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    Con.Open();
                    sd.Fill(dt);
                    Con.Close();
                }
                JobRequestList = ConvertDataTable<JobRequest>(dt);
                if (dt != null && dt.Rows.Count > 0)
                {
                    JobRequestList[0].jobid = Convert.IsDBNull(dt.Rows[0]["jobid"]) ? default(int) : Convert.ToInt32(dt.Rows[0]["jobid"]);
                    JobRequestList[0].jobactionid = Convert.IsDBNull(dt.Rows[0]["jobactionid"]) ? default(int) : Convert.ToInt32(dt.Rows[0]["jobactionid"]);
                    JobRequestList[0].jaid = JobRequestList[0].jobactionid;
                }
            }
            return JobRequestList;
        }

        protected bool Saveuserlocation_Dll(string customercode, string longitude, string latitude)
        {
            int i = 0;
            using (SqlConnection Con = new SqlConnection(constring))
            {
                string Query = string.Empty;
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("updatecustomerlonlat", Con))
                {
                    cmd.Parameters.AddWithValue("@customercode", customercode);
                    cmd.Parameters.AddWithValue("@latitude", latitude);
                    cmd.Parameters.AddWithValue("@longitude", longitude);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    Con.Open();
                    i = cmd.ExecuteNonQuery();
                    Con.Close();
                }
            }
            if (i >= 1) return true; else return false;
        }

        protected string Savesignatureimages_DLL(JobImagesModule objjobimage)
        {
            string signatureimagepath = "";
            using (SqlConnection Con = new SqlConnection(constring))
            {
                string FileName = "";
                string DataUrl = objjobimage.signature.Split(',')[1].ToString();
                string fileName = FileName;
                string path = HttpContext.Current.Server.MapPath("~/Files");
                string fileExtension = Path.GetExtension(fileName);
                string sfileName = Path.GetFileNameWithoutExtension(fileName) + "_" + objjobimage.jaid.ToString() + "_" + System.DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss").Replace(":", "").Replace("/", "") + fileExtension;
                string sFilePath = path + "/" + sfileName;
                string fileNameLocation = Path.Combine(path, sfileName);
                var bytes = Convert.FromBase64String(DataUrl);
                using (var imageFile = new MemoryStream(bytes, 0, bytes.Length))
                {
                    Image image = Image.FromStream(imageFile, true);
                    image.Save(fileNameLocation + ".png", System.Drawing.Imaging.ImageFormat.Png);
                }
                signatureimagepath = "" + sfileName + ".png";

                //fileNameLocation
                string Query = string.Empty;
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("Updatesignature", Con))
                {
                    cmd.Parameters.AddWithValue("@jaid", objjobimage.jaid);
                    cmd.Parameters.AddWithValue("@signature", signatureimagepath);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    Con.Open();
                    sd.Fill(dt);
                    Con.Close();
                }
            }

            return signatureimagepath;
        }

        protected string Savejobimages_DLL(int userid, JobImagesModule objimage)
        {
            string signatureimagepath = "";
            try
            {
                using (SqlConnection Con = new SqlConnection(constring))
                {
                    string FileName = "jobimage";
                    string DataUrl = objimage.image.Split(',')[1].ToString();
                    string fileName = FileName;
                    string path = HttpContext.Current.Server.MapPath("~/Files");
                    string fileExtension = Path.GetExtension(fileName);
                    string sfileName = Path.GetFileNameWithoutExtension(fileName) + "_" + objimage.jaid.ToString() + "_" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("0#") + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + fileExtension;
                    string sFilePath = path + "/" + sfileName;
                    string fileNameLocation = Path.Combine(path, sfileName);
                    var bytes = Convert.FromBase64String(DataUrl);
                    using (var imageFile = new MemoryStream(bytes, 0, bytes.Length))
                    {

                        Helper.LogMessage("image saved");
                        Image image = Image.FromStream(imageFile, true);
                        image.Save(fileNameLocation + ".png", System.Drawing.Imaging.ImageFormat.Png);
                    }
                    signatureimagepath = "" + sfileName + ".png";

                    //fileNameLocation
                    string Query = string.Empty;
                    DataTable dt = new DataTable();

                    using (SqlCommand cmd = new SqlCommand("savejobImages", Con))
                    {
                        cmd.Parameters.AddWithValue("@userid", userid);
                        cmd.Parameters.AddWithValue("@jobid", objimage.jobid);
                        cmd.Parameters.AddWithValue("@jaid", objimage.jaid);
                        cmd.Parameters.AddWithValue("@orderdate", (objimage.orderdate.Year > 1971?objimage.orderdate:DateTime.Now));
                        cmd.Parameters.AddWithValue("@image", signatureimagepath);
                        cmd.Parameters.AddWithValue("@remark", objimage.remark);
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter sd = new SqlDataAdapter(cmd);
                        Con.Open();
                        sd.Fill(dt);
                        Con.Close();
                        Helper.LogMessage("DB Called" + signatureimagepath);
                    }

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        signatureimagepath = signatureimagepath + "$" + dt.Rows[0]["jiid"].ToString();

                    }
                }
            }catch(Exception ex)
            {
                Helper.LogException(ex);
            }
            return signatureimagepath;
        }

        protected bool Deletejobimages_DLL(JobImagesModule objjobimage)
        {
            int i = 0;
            using (SqlConnection Con = new SqlConnection(constring))
            {
                string Query = string.Empty;
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("delete_jobImage", Con))
                {
                    cmd.Parameters.AddWithValue("@jiid", objjobimage.jiid);
                    cmd.Parameters.AddWithValue("@jobid", objjobimage.jobid);
                    cmd.Parameters.AddWithValue("@jaid", objjobimage.jaid);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    Con.Open();
                    i = cmd.ExecuteNonQuery();
                    Con.Close();
                }
            }
            if (i >= 1) return true; else return false;
        }

        protected List<JobImagesModule> getJobimages_DLL(int jaid, int jobid)
        {

            List<JobImagesModule> JobImagesModuleList = new List<JobImagesModule>();
            using (SqlConnection Con = new SqlConnection(constring))
            {
                string Query = string.Empty;
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("Getjobimages", Con))
                {
                    cmd.Parameters.AddWithValue("@jaid", jaid);
                    cmd.Parameters.AddWithValue("@jobid", jobid);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    Con.Open();
                    sd.Fill(dt);
                    Con.Close();
                }
                JobImagesModuleList = ConvertDataTable<JobImagesModule>(dt);
            }
            return JobImagesModuleList;
        }

        protected List<JobHistoryModule> getJobHistory_DLL(int jobid)//vhid
        {

            List<JobHistoryModule> JobHistoryModuleList = new List<JobHistoryModule>();
            using (SqlConnection Con = new SqlConnection(constring))
            {
                string Query = string.Empty;
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("jobhistory", Con))
                {
                    cmd.Parameters.AddWithValue("@jobid", jobid);  //@vhid
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    Con.Open();
                    sd.Fill(dt);
                    Con.Close();
                }
                JobHistoryModuleList = ConvertDataTable<JobHistoryModule>(dt);
            }
            return JobHistoryModuleList;
        }
         
        protected DataTable GetVoucherDetails_DLL(int companyid, string voucherNo)
        {
           //List <JobRequest> JobRequestList = new List<JobRequest>();
            using (SqlConnection Con = new SqlConnection(constring))
            {
                string Query = string.Empty;
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("getVoucherDetails", Con))
                {
                    cmd.Parameters.AddWithValue("@voucherNo", voucherNo);
                    cmd.Parameters.AddWithValue("@companyid", companyid);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    Con.Open();
                    sd.Fill(dt);
                    Con.Close();
                }
                // JobRequestList = ConvertDataTable<JobRequest>(dt);
                return dt;
            }
          //  return JobRequestList;
        }
         
        protected VoucherDetail GetVoucherDetailVM_DLL(int companyid, string voucherNo)
        { 
            List<VoucherDetail> obj = new List<VoucherDetail>();
            using (SqlConnection Con = new SqlConnection(constring))
            {
                string Query = string.Empty;
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("getVoucherDetails", Con))
                {
                    cmd.Parameters.AddWithValue("@voucherNo", voucherNo);
                    cmd.Parameters.AddWithValue("@companyid", companyid);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    Con.Open();
                    sd.Fill(dt);
                    Con.Close();
                }
                obj = ConvertDataTable<VoucherDetail>(dt);
               // return dt;
            }
            return obj.FirstOrDefault();
        }









    }
}

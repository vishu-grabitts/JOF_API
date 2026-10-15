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
    public class LoadJobRequestDLL : BaseClass
    { 
        protected List<JobRequest> getJobrequestData_DLL(int roleid, int userid, string branchcode, int empcode, string type, long tagno)
        {

            List<JobRequest> JobRequestList = new List<JobRequest>();
            using (SqlConnection Con = new SqlConnection(constring))
            {
                string Query = string.Empty;
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("getJobrequestdata_New_Load", Con))
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

        protected List<JobRequest> SaveJobActionNew_DLL(int userid, JobRequest objjobreq)
        {
            List<JobRequest> JobRequestList = new List<JobRequest>();
            using (SqlConnection Con = new SqlConnection(constring))
            {
                string Query = string.Empty;
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("SaveJobActionNewLoad", Con))
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
                    //cmd.Parameters.AddWithValue("@vhid", objjobreq.vhid);
                    cmd.Parameters.AddWithValue("@loadid", objjobreq.loadid);
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
                    cmd.Parameters.AddWithValue("@country", (addr == null || string.IsNullOrEmpty(addr.country)) ? " " : addr.country);
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


        protected List<JobRequest> GetVoucherByLoadId_DLL(int loadid)
        {
            List<JobRequest> JobRequestList = new List<JobRequest>();
            using (SqlConnection Con = new SqlConnection(constring))
            {
                string Query = string.Empty;
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("getVoucherByLoadId", Con))
                {
                    cmd.Parameters.AddWithValue("@loadid", loadid);
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
        

        protected List<JobRequest> GetLoadAndJobByLoadId_DLL(int loadid)
        {
            List<JobRequest> JobRequestList = new List<JobRequest>();
            using (SqlConnection Con = new SqlConnection(constring))
            {
                string Query = string.Empty;
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("getLoadAndJobByLoadId", Con))
                {
                    cmd.Parameters.AddWithValue("@loadid", loadid);
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

          
        protected List<ItemsModule> GetDeliveredItemLoad_DLL(int jobid)
        {

            List<ItemsModule> JobRequestList = new List<ItemsModule>();
            using (SqlConnection Con = new SqlConnection(constring))
            {
                string Query = string.Empty;
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("getdelivereditemload", Con))
                {
                    cmd.Parameters.AddWithValue("@jobid", jobid);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    Con.Open();
                    sd.Fill(dt);
                    Con.Close();
                }
                JobRequestList = ConvertDataTable<ItemsModule>(dt);
            }
            return JobRequestList;
        }


        protected List<ItemsModule> GetItemsDatabyLoadId_DLL(int companyid, int loadid)
        {
            List<ItemsModule> JobRequestList = new List<ItemsModule>();
            using (SqlConnection Con = new SqlConnection(constring))
            {
                string Query = string.Empty;
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("getitembyvouchernoload", Con))
                {
                    cmd.Parameters.AddWithValue("@loadid", loadid);
                    cmd.Parameters.AddWithValue("@companyid", companyid);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    Con.Open();
                    sd.Fill(dt);
                    Con.Close();
                }
                JobRequestList = ConvertDataTable<ItemsModule>(dt);
            }
            return JobRequestList;
        }


        protected List<ItemsModule> GetHandlingUnitByLoadId_DLL(int companyid, int loadid)
        {
            List<ItemsModule> JobRequestList = new List<ItemsModule>();
            using (SqlConnection Con = new SqlConnection(constring))
            {
                string Query = string.Empty;
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("gethandlingunitbyload", Con))
                {
                    cmd.Parameters.AddWithValue("@loadid", loadid);
                    cmd.Parameters.AddWithValue("@companyid", companyid);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    Con.Open();
                    sd.Fill(dt);
                    Con.Close();
                }
                JobRequestList = ConvertDataTable<ItemsModule>(dt);
            }
            return JobRequestList;
        }


    }
}

using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using POD.Models;
using System.Configuration;
using System.Web;
using System.IO;
using System.Drawing;
using System;
using System.Web;
using POD.Models.Customer;

namespace POD.DAL
{
    public class CustomerDAL : BaseClass
    {
        protected List<CompanyDetailVM> GetCompanyDetail(string companyseries)
        {
            var dataList = new List<CompanyDetailVM>();
            using (SqlConnection Con = new SqlConnection(constring))
            {
                string Query = string.Empty;
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("GetCompanyDetail", Con))
                {
                    cmd.Parameters.AddWithValue("@companyseries", companyseries);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    Con.Open();
                    sd.Fill(dt);
                    Con.Close();
                }
                dataList = ConvertDataTable<CompanyDetailVM>(dt);
                if(dataList.Count > 0)
                {
                    dataList[0].companysettingvm = GetSetting(dataList[0].companyid);
                }
            }
            return dataList;
        }

        protected List<CompanySettingVM> GetSetting(int companyid)
        {
            var dataList = new List<CompanySettingVM>();
            using (SqlConnection Con = new SqlConnection(constring))
            {
                string Query = string.Empty;
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("GetSetting", Con))
                {
                    cmd.Parameters.AddWithValue("@companyid", companyid);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    Con.Open();
                    sd.Fill(dt);
                    Con.Close();
                }
                dataList = ConvertDataTable<CompanySettingVM>(dt);
            }
            return dataList;
        }
        
        protected int RegisterCustomer(RegisterCustomerVM model)
        {
            int customerid = 0;
            using (SqlConnection Con = new SqlConnection(constring))
            {
                string Query = string.Empty;
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("RegisterCustomer", Con))
                {
                    cmd.Parameters.AddWithValue("@companyid", model.companyid);
                    cmd.Parameters.AddWithValue("@customername", model.customername);
                    cmd.Parameters.AddWithValue("@contactperson", model.contactperson);
                    cmd.Parameters.AddWithValue("@email", model.email);
                    cmd.Parameters.AddWithValue("@mobile", model.mobile);
                    cmd.Parameters.AddWithValue("@isemailverified", model.isemailverified);
                    cmd.Parameters.AddWithValue("@ismobileverified", model.ismobileverified);
                    cmd.Parameters.AddWithValue("@password", Crypto.EncryptData(model.password));

                    cmd.Parameters.Add("@returnValue", SqlDbType.Int);
                    cmd.Parameters["@returnValue"].Direction = ParameterDirection.Output;

                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    Con.Open();
                    cmd.ExecuteNonQuery();
                    int.TryParse(cmd.Parameters["@returnValue"].Value + "", out customerid);
                    Con.Close();
                }
            }
            return customerid;
        }

        protected List<CompanyDetailVM> GetCompaniesByUsername(string username)
        {
            var dataList = new List<CompanyDetailVM>();
            using (SqlConnection Con = new SqlConnection(constring))
            {
                string Query = string.Empty;
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("GetCompaniesByUsername", Con))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    Con.Open();
                    sd.Fill(dt);
                    Con.Close();
                }
                dataList = ConvertDataTable<CompanyDetailVM>(dt);
                if (dataList.Count > 0)
                {
                    dataList[0].companysettingvm = GetSetting(dataList[0].companyid);
                }
            }
            return dataList;
        }

        protected List<CustomerLoginResponseVM> CustomerLogin(CustomerLoginVM model)
        {
            var dataList = new List<CustomerLoginResponseVM>();

            if (string.IsNullOrEmpty(model.companyseries))
            {
                var companies = GetCompaniesByUsername(model.username);
                if(companies.Count == 1)
                {
                    model.companyseries = companies[0].companyseries;
                }
                else if (companies.Count > 1)
                {
                    dataList.Add(new CustomerLoginResponseVM { companydetailvm = companies, status = -1, statusmessage="More than one company found!" });
                    return dataList;
                }
                else
                {
                    dataList.Add(new CustomerLoginResponseVM { companydetailvm = companies, status = 0, statusmessage="Username does not exist!" });
                    return dataList;
                }
            }

            using (SqlConnection Con = new SqlConnection(constring))
            {
                string Query = string.Empty;
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("CustomerLogin", Con))
                {
                    cmd.Parameters.AddWithValue("@username", model.username);
                    cmd.Parameters.AddWithValue("@password", Crypto.EncryptData(model.password));
                    cmd.Parameters.AddWithValue("@companyseries", model.companyseries);
                    cmd.Parameters.AddWithValue("@logintype", model.logintype);

                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    Con.Open();
                    sd.Fill(dt);
                    Con.Close();
                }
                dataList = ConvertDataTable<CustomerLoginResponseVM>(dt);
                if(dataList.Count > 0)
                {
                    dataList = dataList.Select(x => { x.status = 1; x.statusmessage = "Login success!"; return x; }).ToList();
                }
            }
            return dataList;
        }

        protected List<CustomerLoginResponseVM> GetUserByMobile(string mobile, int companyid)
        {
            var dataList = new List<CustomerLoginResponseVM>();
            using (SqlConnection Con = new SqlConnection(constring))
            {
                string Query = string.Empty;
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("GetUserByMobile", Con))
                {
                    cmd.Parameters.AddWithValue("@contactno", mobile);
                    cmd.Parameters.AddWithValue("@companyid", companyid);

                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    Con.Open();
                    sd.Fill(dt);
                    Con.Close();
                }
                dataList = ConvertDataTable<CustomerLoginResponseVM>(dt);
            }
            return dataList;
        }

        protected List<CustomerLoginResponseVM> GetUserByEmail(string email, int companyid)
        {
            var dataList = new List<CustomerLoginResponseVM>();
            using (SqlConnection Con = new SqlConnection(constring))
            {
                string Query = string.Empty;
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("GetUserByEmail", Con))
                {
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@companyid", companyid);

                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    Con.Open();
                    sd.Fill(dt);
                    Con.Close();
                }
                dataList = ConvertDataTable<CustomerLoginResponseVM>(dt);
            }
            return dataList;
        }


        protected int SendEmailForVerifyEmailId(string email, bool isCheckDuplicate, int companyid)
        { 
            if (isCheckDuplicate)
            {
               var obj = GetUserByEmail(email, companyid);
                if(obj.Count > 0)
                {
                    return -1;
                }
            }

            var otp = GenerateOTP();

            var dtMailConfig = new MasterDLL().getMailConfig(companyid, "verifyemail", "en", null);
            string message = dtMailConfig.Rows[0]["body"].ToString().Trim();
            string body = message.Replace("@otp", otp.ToString());
            string subject = dtMailConfig.Rows[0]["subject"].ToString();
            
            var isEmailSend = new NotificationDLL().SendEmail(companyid, subject, body, email, null, null);
            return otp;
        }
          
        protected int SendSMSForVerifyMobile(string mobile, bool isCheckDuplicate, int companyid)
        {
            if (isCheckDuplicate)
            {
                var obj = GetUserByMobile(mobile, companyid);
                if (obj.Count > 0)
                {
                    return -1;
                }
            }

            var otp = GenerateOTP();

            //var dtMailConfig = new MasterDLL().getMailConfig(companyid, "verifyemail", "en", null);
            //string message = dtMailConfig.Rows[0]["body"].ToString().Trim();
            //string body = message.Replace("@otp", otp.ToString());
            //string subject = dtMailConfig.Rows[0]["subject"].ToString();

            //var isEmailSend = new NotificationDLL().SendEmail(companyid, subject, body, email, null, null);
            return 1234;
        }




        public int GenerateOTP()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }
        //[GetUserByMobile] '0987654321', 1
        //[GetUserByEmail] 'giri0001@gmail.com', 1



        //NotificationDLL
    }
}


using POD.Models;
using System;
using System;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IdentityModel.Tokens;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;


namespace POD.DAL
{
    public class UserAccountDAL : BaseClass
    {
       

        protected int ForgotPassword_DLL(string loginId)
        {
            int statusReponse = 0;
            string token = Guid.NewGuid().ToString();
            string name = "";
            string email = "";
            using (SqlConnection Con = new SqlConnection(constring))
            {
                string CRMIP11 = ConfigurationManager.ConnectionStrings["PODConf"].ConnectionString;

                using (SqlCommand cmd = new SqlCommand("ForgotPassword", Con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@loginid", loginId);
                    cmd.Parameters.AddWithValue("@token", token);

                    //cmd.Parameters.AddWithValue("@password", objUA.password);
                    cmd.Parameters.Add("@status", SqlDbType.Int);
                    cmd.Parameters["@status"].Direction = ParameterDirection.Output;

                    cmd.Parameters.Add("@name", SqlDbType.VarChar, 100);
                    cmd.Parameters["@name"].Direction = ParameterDirection.Output;

                    cmd.Parameters.Add("@email", SqlDbType.VarChar, 100);
                    cmd.Parameters["@email"].Direction = ParameterDirection.Output;

                    Con.Open();
                    cmd.ExecuteNonQuery();
                    int.TryParse(cmd.Parameters["@status"].Value + "", out statusReponse);
                    name = cmd.Parameters["@name"].Value + "";
                    email = cmd.Parameters["@email"].Value + "";
                    Con.Close();
                }
            }

            if(statusReponse > 0)
            {
                var objDLL = new NotificationDLL();
                string url = ConfigurationManager.AppSettings["FORGOT_PASSWORD_URL"] + token;
                var sb = new StringBuilder(System.IO.File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("~/content/templates/email/reset-password.html")));
                sb.Replace("@name", name);
                sb.Replace("@verifyurl", url);
                return objDLL.SendEmail(0, "Reset Password", sb.ToString(), email, null, null)?1:0;
            }

            return statusReponse;
        }

        //protected List<UserData> GetUserLoginData(UserData objUA)
        //{
        //    List<UserData> UserAccountList = new List<UserData>();
        //    using (SqlConnection Con = new SqlConnection(constring))
        //    {
        //        string Query = string.Empty;
        //        DataTable dt = new DataTable();
        //        string CRMIP11 = ConfigurationManager.ConnectionStrings["PODConf"].ConnectionString;


        //        using (SqlCommand cmd = new SqlCommand("UserLogin", Con))
        //        {
        //            cmd.Parameters.AddWithValue("@loginid", objUA.loginid);
        //            cmd.Parameters.AddWithValue("@password", Crypto.EncryptData(objUA.password));
        //            //cmd.Parameters.AddWithValue("@password", objUA.password);
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            SqlDataAdapter sd = new SqlDataAdapter(cmd);
        //            Con.Open();
        //            sd.Fill(dt);
        //            Con.Close();
        //        }
        //        if (dt.Rows.Count > 0)
        //        {
        //            UserAccountList = ConvertDataTable<UserData>(dt);
        //            UserAccountList[0].appversion = objUA.appversion;
        //        }
        //    }
        //    return UserAccountList;
        //}

        protected List<UserData> GetUserLoginDataV2(UserData objUA)
        {
            List<UserData> UserAccountList = new List<UserData>();

            using (SqlConnection Con = new SqlConnection(constring))
            {
                DataTable dt = new DataTable();

                string CRMIP11 = ConfigurationManager
                    .ConnectionStrings["PODConf"]
                    .ConnectionString;

                using (SqlCommand cmd = new SqlCommand("UserLogin", Con))
                {
                    cmd.Parameters.AddWithValue("@loginid", objUA.loginid);
                    cmd.Parameters.AddWithValue(
                        "@password",
                        Crypto.EncryptData(objUA.password)
                    );

                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter sd = new SqlDataAdapter(cmd);

                    Con.Open();
                    sd.Fill(dt);
                    Con.Close();
                }

                if (dt.Rows.Count > 0)
                {
                    UserAccountList = ConvertDataTable<UserData>(dt);

                    if (UserAccountList.Count > 0)
                    {
                        UserAccountList[0].appversion = objUA.appversion;

                        // Get JWT Secret from Web.config
                        string jwtSecret = ConfigurationManager.AppSettings["JWTSecretKey"];

                        if (string.IsNullOrEmpty(jwtSecret))
                        {
                            throw new Exception("JWTSecretKey not found in Web.config");
                        }

                        UserData user = UserAccountList[0];

                        // Create security key
                        var key = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtSecret)
                        );

                        // Create signing credentials
                        var credentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(
                            key,
                            Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256
                        );

                        // Create claims
                        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.userid.ToString()),
            new Claim(ClaimTypes.Name, user.loginid ?? ""),
            new Claim(ClaimTypes.Email, user.emailid ?? ""),
            new Claim("empcode", user.empcode ?? ""),
            new Claim("roleid", user.roleid.ToString()),
            new Claim("rolename", user.rolename ?? "")
        };

                        // Create JWT
                        var token = new JwtSecurityToken(
                            claims: claims,
                            expires: DateTime.UtcNow.AddHours(8),
                            signingCredentials: credentials
                        );

                        // Convert JWT to string
                        string jwtToken = new JwtSecurityTokenHandler()
                            .WriteToken(token);

                        // Put token into UserAccountList
                        UserAccountList[0].jwttoken = jwtToken;

                        ////                        SaveUserLoginHistory_DLL(
                        ////    new UserLoginHistorySaveParameters
                        ////    {
                        ////        UserId = user.userid,
                        ////        UserToken = jwtToken,
                        ////        TokenExpireOn = DateTime.UtcNow.AddHours(8),
                        ////        DeviceName = "",
                        ////        IPAddress = "",
                        ////        RememberMe = false,
                        ////        IsLoggedIn = true
                        ////    }
                        ////);

                    }
                }
            }
            return UserAccountList;
           
        }

        protected List<UserData> GetUserLoginData(UserData objUA)

        {

            List<UserData> UserAccountList = new List<UserData>();

            using (SqlConnection Con = new SqlConnection(constring))

            {

                string Query = string.Empty;

                DataTable dt = new DataTable();

                string CRMIP11 = ConfigurationManager.ConnectionStrings["PODConf"].ConnectionString;





                using (SqlCommand cmd = new SqlCommand("UserLogin", Con))

                {

                    cmd.Parameters.AddWithValue("@loginid", objUA.loginid);

                    cmd.Parameters.AddWithValue("@password", Crypto.EncryptData(objUA.password));

                    //cmd.Parameters.AddWithValue("@password", objUA.password);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter sd = new SqlDataAdapter(cmd);

                    Con.Open();

                    sd.Fill(dt);

                    Con.Close();

                }

                if (dt.Rows.Count > 0)

                {

                    UserAccountList = ConvertDataTable<UserData>(dt);

                    UserAccountList[0].appversion = objUA.appversion;

                }

            }

            return UserAccountList;

        }
        protected bool SaveUserLoginHistory_DLL(
    UserLoginHistorySaveParameters parameters)
        {
            int result = 0;

            using (SqlConnection Con = new SqlConnection(constring))
            {
                using (SqlCommand cmd = new SqlCommand(
                    "SaveUserLoginHistory", Con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue(
                        "@UserId",
                        parameters.UserId);

                    cmd.Parameters.AddWithValue(
                        "@UserToken",
                        parameters.UserToken ?? "");

                    cmd.Parameters.AddWithValue(
                        "@TokenExpireOn",
                        parameters.TokenExpireOn);

                    cmd.Parameters.AddWithValue(
                        "@DeviceName",
                        parameters.DeviceName ?? "");

                    cmd.Parameters.AddWithValue(
                        "@IPAddress",
                        parameters.IPAddress ?? "");

                    cmd.Parameters.AddWithValue(
                        "@RememberMe",
                        parameters.RememberMe);

                    cmd.Parameters.AddWithValue(
                        "@IsLoggedIn",
                        parameters.IsLoggedIn);

                    Con.Open();

                    result = cmd.ExecuteNonQuery();

                    Con.Close();
                }
            }

            return result > 0;
        }

        protected List<BranchData> GetUserBranchData(int userid)
        {
            List<BranchData> BranchDataList = new List<BranchData>();
            using (SqlConnection Con = new SqlConnection(constring))
            {
                string Query = string.Empty;
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("GetUserBranch", Con))
                {
                    cmd.Parameters.AddWithValue("@userid", userid);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    Con.Open();
                    sd.Fill(dt);
                    Con.Close();
                }
                BranchDataList = ConvertDataTable<BranchData>(dt);
            }
            return BranchDataList;
        }

        protected List<BranchData> GetUserBranches_DLL(int roleid, int userid)
        {
            List<BranchData> BranchDataList = new List<BranchData>();
            using (SqlConnection Con = new SqlConnection(constring))
            {
                string Query = string.Empty;
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("GetUserBranchData", Con))
                {
                    cmd.Parameters.AddWithValue("@userid", userid);
                    cmd.Parameters.AddWithValue("@roleid", roleid);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    Con.Open();
                    sd.Fill(dt);
                    Con.Close();
                }
                BranchDataList = ConvertDataTable<BranchData>(dt);
            }
            return BranchDataList;
        }

        protected bool updateloginhistory_DLL(UserData objUA)
        {
            int i = 0;
            using (SqlConnection Con = new SqlConnection(constring))
            {
                string Query = string.Empty;
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("updateloginhistory", Con))
                {
                    cmd.Parameters.AddWithValue("@userid", objUA.userid);
                    cmd.Parameters.AddWithValue("@loginid", objUA.loginid);
                    cmd.Parameters.AddWithValue("@logintype", objUA.rolename);
                    cmd.Parameters.AddWithValue("@mobileos", objUA.mobileos);
                    cmd.Parameters.AddWithValue("@macid", objUA.macid);
                    cmd.Parameters.AddWithValue("@appversion", objUA.appversion);
                    cmd.Parameters.AddWithValue("@latitude", "");
                    cmd.Parameters.AddWithValue("@longitude", "");
                    cmd.Parameters.AddWithValue("@companyid", objUA.companyid);
                    cmd.Parameters.AddWithValue("@tokennumber", objUA.tokennumber);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    Con.Open();
                    i = cmd.ExecuteNonQuery();
                    Con.Close();
                }
            }
            if (i >= 1) return true; else return false;
        }
        protected bool updatelogouthistory_DLL(UserData objUA)
        {
            int i = 0;
            using (SqlConnection Con = new SqlConnection(constring))
            {
                string Query = string.Empty;
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("updatelogouthistory", Con))
                {
                    cmd.Parameters.AddWithValue("@userid", objUA.userid);
                    cmd.Parameters.AddWithValue("@loginid", objUA.loginid);
                    cmd.Parameters.AddWithValue("@logintype", string.IsNullOrEmpty(objUA.rolename)?"": objUA.rolename);                    
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    Con.Open();
                    i = cmd.ExecuteNonQuery();
                    Con.Close();
                }
            }
            if (i >= 1) return true; else return false;
        }

        protected bool updateuser_DLL(UserData objUA)
        {
            int i = 0;
            using (SqlConnection Con = new SqlConnection(constring))
            {
                string Query = string.Empty;
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("updateUser", Con))
                {
                    cmd.Parameters.AddWithValue("@userid", objUA.userid);
                    cmd.Parameters.AddWithValue("@loginid", objUA.loginid);
                    cmd.Parameters.AddWithValue("@logintype", objUA.rolename);
                    cmd.Parameters.AddWithValue("@empcode", objUA.empcode);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    Con.Open();
                    i = cmd.ExecuteNonQuery();
                    Con.Close();
                }
            }
            if (i >= 1) return true; else return false;
        }


        protected bool updateemail_DLL(UserData objUA)
        {
            int i = 0;
            using (SqlConnection Con = new SqlConnection(constring))
            {
                string Query = string.Empty;
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("updateEmail", Con))
                {
                    cmd.Parameters.AddWithValue("@userid", objUA.userid);
                    cmd.Parameters.AddWithValue("@loginid", objUA.loginid);
                    cmd.Parameters.AddWithValue("@empcode", objUA.empcode);
                    cmd.Parameters.AddWithValue("@emailid", objUA.emailid);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    Con.Open();
                    i = cmd.ExecuteNonQuery();
                    Con.Close();
                }
            }
            if (i >= 1) return true; else return false;
        }

        protected bool updateabout_DLL(UserData objUA)
        {
            int i = 0;
            using (SqlConnection Con = new SqlConnection(constring))
            {
                string Query = string.Empty;
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("updateAbout", Con))
                {
                    cmd.Parameters.AddWithValue("@userid", objUA.userid);
                    cmd.Parameters.AddWithValue("@loginid", objUA.loginid);
                    cmd.Parameters.AddWithValue("@empcode", objUA.empcode);
                    cmd.Parameters.AddWithValue("@about", objUA.about);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    Con.Open();
                    i = cmd.ExecuteNonQuery();
                    Con.Close();
                }
            }
            if (i >= 1) return true; else return false;
        }


        protected bool updatecontact_DLL(UserData objUA)
        {
            int i = 0;
            using (SqlConnection Con = new SqlConnection(constring))
            {
                string Query = string.Empty;
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("updateContactNo", Con))
                {
                    cmd.Parameters.AddWithValue("@userid", objUA.userid);
                    cmd.Parameters.AddWithValue("@loginid", objUA.loginid);
                    cmd.Parameters.AddWithValue("@empcode", objUA.empcode);
                    cmd.Parameters.AddWithValue("@contactno", objUA.contactno);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    Con.Open();
                    i = cmd.ExecuteNonQuery();
                    Con.Close();
                }
            }
            if (i >= 1) return true; else return false;
        }


        protected bool updateaddress_DLL(EmployeeAddress objUA)
        {
            int i = 0;
            using (SqlConnection Con = new SqlConnection(constring))
            {
                string Query = string.Empty;
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("updateAddress", Con))
                {
                    try
                    {
                        var addr = objUA.landmark + " " + objUA.houseno + " " + objUA.city + " " + objUA.state + " " + objUA.country + " " + objUA.pincode;
                       var obj =  new GeoLocationServices().GetAddress(null, null, addr, false);
                        if(obj != null)
                        {
                            objUA.latitude = obj.lat;
                            objUA.longitude = obj.lng;
                        }
                    }
                    catch{}

                  //  cmd.Parameters.AddWithValue("@userid", objUA.userid);
                  //  cmd.Parameters.AddWithValue("@loginid", objUA.loginid);
                    cmd.Parameters.AddWithValue("@empcode", objUA.empcode);
                    cmd.Parameters.AddWithValue("@pincode", objUA.pincode);
                    cmd.Parameters.AddWithValue("@latitude", objUA.latitude);
                    cmd.Parameters.AddWithValue("@longitude", objUA.longitude);
                    cmd.Parameters.AddWithValue("@addresstype", objUA.addresstype);
                    cmd.Parameters.AddWithValue("@houseno", objUA.houseno);
                    cmd.Parameters.AddWithValue("@landmark", objUA.landmark);
                    cmd.Parameters.AddWithValue("@city", objUA.city);
                    cmd.Parameters.AddWithValue("@state", objUA.state);
                    cmd.Parameters.AddWithValue("@country", objUA.country);
                    


                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    Con.Open();
                    i = cmd.ExecuteNonQuery();
                    Con.Close();
                }
            }
            if (i >= 1) return true; else return false;
        }

        protected List<EmployeeAddress> GetEmployeeAddress_DLL(string empcode)
        {

            List<EmployeeAddress> obj = new List<EmployeeAddress>();
            using (SqlConnection con = new SqlConnection(constring))
            {
                string Query = string.Empty;
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("GetEmployeeAddress", con))
                {
                    cmd.Parameters.AddWithValue("@empid", empcode);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    con.Open();
                    sd.Fill(dt);
                    con.Close();
                }
                obj = ConvertDataTable<EmployeeAddress>(dt);
            }
            return obj;
        }

        protected bool DeleteEmployeeAddress_DLL(int empAddressId)
        {
            int i = 0;
            using (SqlConnection Con = new SqlConnection(constring))
            {
                string Query = string.Empty;
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("Delete_EmployeeAddress", Con))
                {
                    cmd.Parameters.AddWithValue("@id", empAddressId);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    Con.Open();
                    i = cmd.ExecuteNonQuery();
                    Con.Close();
                }
            }
            if (i >= 1) return true; else return false;
        }

        protected string SaveIdProof_DLL(IdProof objUA)
        {
            string signatureimagepath = "";
            using (SqlConnection Con = new SqlConnection(constring))
            {
                string FileName = "";
                string DataUrl = objUA.docfile.Split(',')[1].ToString();
                string fileName = FileName;
                string path = HttpContext.Current.Server.MapPath("~/Files");
                string fileExtension = Path.GetExtension(fileName);
                string sfileName = Path.GetFileNameWithoutExtension(fileName) + "_" + System.DateTime.Now.ToString("dd/MM/yyyyhh:mm:ss").Replace(":", "").Replace("/", "") + fileExtension;
                string sFilePath = path + "/" + sfileName;
                string fileNameLocation = Path.Combine(path, sfileName);
                var bytes = Convert.FromBase64String(DataUrl);
                using (var imageFile = new MemoryStream(bytes, 0, bytes.Length))
                {
                    Image image = Image.FromStream(imageFile, true);
                    image.Save(fileNameLocation + ".jpeg", System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                signatureimagepath = "" + sfileName + ".jpeg";

                //fileNameLocation
                string Query = string.Empty;
                DataTable dt = new DataTable();

                using (SqlCommand cmd = new SqlCommand("saveidproof", Con))
                {
                    //cmd.Parameters.AddWithValue("@userid", objUA.userid);
                    cmd.Parameters.AddWithValue("@empid", objUA.empcode);
                    cmd.Parameters.AddWithValue("@docfile", signatureimagepath);
                    cmd.Parameters.AddWithValue("@doctype", objUA.doctype);
                    cmd.Parameters.AddWithValue("@docno", objUA.docno);
                    cmd.Parameters.AddWithValue("@expirydate", objUA.expirydate);

                    
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    Con.Open();
                    cmd.ExecuteNonQuery();
                    Con.Close();
                }

              
            }


            return signatureimagepath;
        }

        protected List<IdProof> getIdProof_DLL(string empcode)
        {

            List<IdProof> lstIdProof = new List<IdProof>();
            using (SqlConnection Con = new SqlConnection(constring))
            {
                string Query = string.Empty;
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("Getidproof", Con))
                {
                    cmd.Parameters.AddWithValue("@empid", empcode);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    Con.Open();
                    sd.Fill(dt);
                    Con.Close();
                }
                lstIdProof = ConvertDataTable<IdProof>(dt);
            }
            return lstIdProof;
        }


        protected bool Deleteidproof_DLL(IdProof idProof)
        {
            int i = 0;
            using (SqlConnection Con = new SqlConnection(constring))
            {
                string Query = string.Empty;
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("delete_idproof", Con))
                {
                    cmd.Parameters.AddWithValue("@idproofid", idProof.idproofid);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    Con.Open();
                    i = cmd.ExecuteNonQuery();
                    Con.Close();
                }
            }
            if (i >= 1) return true; else return false;
        }


        protected string Updatepic_DLL(UserData objUA)
        {
            string signatureimagepath = "";
            using (SqlConnection Con = new SqlConnection(constring))
            {
                string FileName = "";
                string DataUrl = objUA.pic.Split(',')[1].ToString();
                string fileName = FileName;
                string path = HttpContext.Current.Server.MapPath("~/Files");
                string fileExtension = Path.GetExtension(fileName);
                string sfileName = Path.GetFileNameWithoutExtension(fileName) + "_" + objUA.empcode.ToString() + "_" + System.DateTime.Now.ToString("dd/MM/yyyyhh:mm:ss").Replace(":", "").Replace("/", "") + fileExtension;
                string sFilePath = path + "/" + sfileName;
                string fileNameLocation = Path.Combine(path, sfileName);
                var bytes = Convert.FromBase64String(DataUrl);
                using (var imageFile = new MemoryStream(bytes, 0, bytes.Length))
                {
                    Image image = Image.FromStream(imageFile, true);
                    image.Save(fileNameLocation + ".jpeg", System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                signatureimagepath = "" + sfileName + ".jpeg";

                //fileNameLocation
                string Query = string.Empty;
                DataTable dt = new DataTable();

                using (SqlCommand cmd = new SqlCommand("updatepic", Con))
                {
                    cmd.Parameters.AddWithValue("@userid", objUA.userid);
                    cmd.Parameters.AddWithValue("@empcode", objUA.empcode);
                    cmd.Parameters.AddWithValue("@pic", signatureimagepath);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    Con.Open();
                    cmd.ExecuteNonQuery();
                    Con.Close();
                }


            }


            return signatureimagepath;
        }

         


    }

}


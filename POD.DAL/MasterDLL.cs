using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POD.Models;
using System.Data;
using System.Data.SqlClient;

namespace POD.DAL
{
    public class MasterDLL : BaseClass
    { 
        protected List<SysUserNotification> getUserNotifications_DLL(int empid)
        {

            List<SysUserNotification> MasterDataList = new List<SysUserNotification>();
            using (SqlConnection Con = new SqlConnection(constring))
            {
                string Query = string.Empty;
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("GetUserNotifications", Con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@empid", empid);
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    Con.Open();
                    sd.Fill(dt);
                    Con.Close();
                }
                MasterDataList = ConvertDataTable<SysUserNotification>(dt);
            }
            return MasterDataList;
        }


        protected bool UpdateUserNotificationRead_DLL(int empid)
        {
            using (SqlConnection Con = new SqlConnection(constring))
            {
                string Query = string.Empty;
                using (SqlCommand cmd = new SqlCommand("UPDATE sys_user_notifications SET isread=1 WHERE empid = @empid", Con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@empid", empid);
                    Con.Open();
                    cmd.ExecuteNonQuery();
                    Con.Close();
                }
            }
            return true;
        }

        protected int GetUserNotificationCount_DLL(int empid)
        {
            List<SysUserNotification> MasterDataList = new List<SysUserNotification>();
            using (SqlConnection Con = new SqlConnection(constring))
            {
                string Query = string.Empty;
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) AS TotalCount FROM sys_user_notifications WHERE empid = @empid AND isread = 0", Con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@empid", empid);
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    Con.Open();
                    sd.Fill(dt);
                    Con.Close();
                    if(dt.Rows.Count > 0)
                    {
                       return dt.Rows[0].Field<int>("TotalCount");
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            return 0;
        }


        protected List<ActionMaster> getActionMaster_DLL()
        {

            List<ActionMaster> MasterDataList = new List<ActionMaster>();
            using (SqlConnection Con = new SqlConnection(constring))
            {
                string Query = string.Empty;
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("getActionMaster", Con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    Con.Open();
                    sd.Fill(dt);
                    Con.Close();
                }
                MasterDataList = ConvertDataTable<ActionMaster>(dt);
            }
            return MasterDataList;
        }
        protected List<ReasonMaster> getReasonMaster_DLL(int companyid)
        {

            List<ReasonMaster> ReasonMasterList = new List<ReasonMaster>();
            using (SqlConnection Con = new SqlConnection(constring))
            {
                string Query = string.Empty;
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("getRejectreason", Con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@companyid", companyid);
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    Con.Open();
                    sd.Fill(dt);
                    Con.Close();
                }
                ReasonMasterList = ConvertDataTable<ReasonMaster>(dt);
            }
            return ReasonMasterList;
        }

        public DataTable getMailConfig(int companyid, string type, string langcode, string deliverytype)
        {

            using (SqlConnection Con = new SqlConnection(constring))
            {
                string Query = string.Empty;
                Query = @"SELECT smc.mcid, smc.companyid, smc.actionid, smc.[type], smc.[subject], smc.body, smc.mailto, smc.mailcc
                        FROM sys_mailconfig smc WHERE smc.isactive=1 and smc.isdeleted=0 and (companyid = @compid or companyid IS NULL) and type = @type and langcode = @langcode and (deliverytype IS NULL OR deliverytype = @deliverytype)";
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand(Query, Con))
                {
                    cmd.Parameters.AddWithValue("@compid", companyid);
                    cmd.Parameters.AddWithValue("@type", type);
                    cmd.Parameters.AddWithValue("@langcode", langcode);
                    cmd.Parameters.AddWithValue("@deliverytype", string.IsNullOrEmpty(deliverytype)?(object)DBNull.Value : deliverytype); 
                    cmd.CommandType = CommandType.Text;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    Con.Open();
                    sd.Fill(dt);
                    Con.Close();
                }
                return dt;
            }
        }

        public List<SysSettingVM> GetSysSettings_DLL(int companyid, string key)
        {
            List<SysSettingVM> list = new List<SysSettingVM>();
            using (SqlConnection Con = new SqlConnection(constring))
            {
                string Query = string.Empty;
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("sp_GetSysSettings", Con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@key", string.IsNullOrEmpty(key)?"":key);
                    cmd.Parameters.AddWithValue("@companyid", companyid);
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    Con.Open();
                    sd.Fill(dt);
                    Con.Close();
                }
                list = ConvertDataTable<SysSettingVM>(dt);
            }
            return list;
        }
         
    }
}

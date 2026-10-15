using POD.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POD.DAL
{ 
    public class EmployeeLocationDLL : BaseClass
    {  
        protected bool SaveEmployeeLocation_DLL(List<EmployeeLocation> locations)
        {
            var table = new DataTable();
            table.Columns.Add("id", typeof(string));
            table.Columns.Add("empid", typeof(int));
            table.Columns.Add("latitude", typeof(string));
            table.Columns.Add("longitude", typeof(string));
            table.Columns.Add("isactive", typeof(bool));
            table.Columns.Add("createdon", typeof(DateTime));
            table.Columns.Add("type", typeof(string));
            table.Columns.Add("batterylevel", typeof(string));

            
            for (var i = 0; i < locations.Count; i++)
            {
                var location = locations[i];

                var row = table.NewRow();
                row["id"] = 0;
                row["empid"] = location.empid;
                row["latitude"] = location.latitude;
                row["longitude"] = location.longitude;
                row["isactive"] = location.isactive;
                row["createdon"] = location.createdon;
                row["type"] = location.type;
                row["batterylevel"] = location.batterylevel;

                if((location.type+"").ToLower() == "start")
                {
                    UpdateUserLocation_DLL(location.empid, "start", location.createdon);
                }
                else if ((location.type + "").ToLower() == "stop")
                {
                    UpdateUserLocation_DLL(location.empid, "stop", location.createdon);
                }

                table.Rows.Add(row);
            }
             
            using (SqlConnection conn = new SqlConnection(constring))
            {
                conn.Open();
                using (var bulk = new SqlBulkCopy(conn))
                {
                    bulk.DestinationTableName = "EmployeeLocations";
                    bulk.WriteToServer(table);
                }
                conn.Close();
            }
            return true; 
        }

         
        protected bool UpdateUserLocation_DLL(int empid, string status, DateTime statusOn)
        {
            int i = 0;
            using (SqlConnection Con = new SqlConnection(constring))
            {
                string Query = string.Empty;
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("UpdateUserLocation", Con))
                {
                    cmd.Parameters.AddWithValue("@empid", empid);
                    cmd.Parameters.AddWithValue("@status", status);
                    cmd.Parameters.AddWithValue("@statusOn", statusOn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    Con.Open();
                    i = cmd.ExecuteNonQuery();
                    Con.Close();
                }
            }
            if (i >= 1) return true; else return false;
        }


        protected List<EmployeeLocationVM> GetEmployeeLocations_DLL(int empid, DateTime date) 
        {
            List<EmployeeLocationVM> model = new List<EmployeeLocationVM>();
            using (SqlConnection conn = new SqlConnection(constring))
            {
                string Query = string.Empty;
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("GetEmployeeLocations", conn))
                {
                    cmd.Parameters.AddWithValue("@empid", empid);
                    cmd.Parameters.AddWithValue("@date", date.Date);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    conn.Open();
                    sd.Fill(dt);
                    conn.Close();
                }
                model = ConvertDataTable<EmployeeLocationVM>(dt);
            }
          return model;
        }
         
        protected List<UserVM> GetUsers_DLL(int roleid, int companyid)
        {
            List<UserVM> model = new List<UserVM>();
            using (SqlConnection conn = new SqlConnection(constring))
            {
                string Query = string.Empty;
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("GetUsers", conn))
                {
                    cmd.Parameters.AddWithValue("@roleid", roleid);
                    cmd.Parameters.AddWithValue("@companyid", companyid);

                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    conn.Open();
                    sd.Fill(dt);
                    conn.Close();
                }
                model = ConvertDataTable<UserVM>(dt);
            }
            return model;
        }  
    }
}

using POD.Models;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Reflection;

namespace POD.DAL
{
    public class LoginDAL
    {
        private readonly string connectionString;

        public LoginDAL()
        {
            connectionString =
                ConfigurationManager
                .ConnectionStrings["PODConf"]
                .ConnectionString;
        }

        public LoginUserModel ValidateUser(string username,string password)
        {
            LoginUserModel user = null;
            var encryptedPassword = Crypto.EncryptData(password);
            using (SqlConnection con =
           new SqlConnection(connectionString))
            {
                string query = @"SELECT TOP 1 UserId,loginid,emailid,u.roleid,r.rolename,u.IsActive
                    FROM sys_user u left outer join sys_userrole r on u.roleid=r.roleid
                    WHERE erploginid = @Username or emailid=@Username
                      AND Password = @Password
                      AND u.IsActive = 1";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Username", username);

                    cmd.Parameters.AddWithValue("@Password", encryptedPassword);
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new LoginUserModel
                            {
                                UserId = Convert.ToInt32(reader["UserId"]),

                                Username = reader["loginid"].ToString(),

                                Email = reader["emailid"].ToString(),

                                Role = reader["rolename"].ToString(),

                                IsActive = Convert.ToBoolean(reader["IsActive"])
                            };
                        }
                    }
                }
            }

            return user;
        }
    }
}
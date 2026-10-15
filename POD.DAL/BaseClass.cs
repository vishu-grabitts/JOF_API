using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace POD.DAL
{
    [Serializable]
    public class BaseClass
    {
        protected static string constring;
        protected static string conCRMstring;
        protected static string CRMConnectionString;

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            this.disposed = true;
        }

        public BaseClass()
        {
            //string s = "data source=192.168.11.34;initial catalog=infor;user id=webuser;password=web@web#";            
            CRMConnectionString = ConfigurationManager.ConnectionStrings["PODConf"].ToString();
            constring = ConfigurationManager.ConnectionStrings["PODConf"].ToString();
        }

        public void Dispose()
        {
            Dispose(disposed);
            GC.SuppressFinalize(this);
        }
        //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["HumrahiLNContext"].ToString());

        #region Convert Datatable to List
        protected List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName && obj != null)
                    {
                        if(!(dr[column.ColumnName] == null || dr[column.ColumnName] == DBNull.Value))
                        {
                            pro.SetValue(obj,  dr[column.ColumnName], null);
                        }
                    }
                    //
                   // pro.SetValue(obj, dr[column.ColumnName] == null || dr[column.ColumnName] == DBNull.Value ? string.Empty : dr[column.ColumnName], null);

                    //if(pro.PropertyType.isfl)

                        //pro.SetValue(obj, dr[column.ColumnName] , null);
                    else
                        continue;
                }
            }
            return obj;
        }
        #endregion


    }
}

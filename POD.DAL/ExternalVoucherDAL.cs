using Newtonsoft.Json;
using POD.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace POD.DAL
{
    public class ExternalVoucherDAL
    {
        private readonly string _connectionString;

        public ExternalVoucherDAL()
        {
            _connectionString = DbConnection.ConnectionString;
        }
       public bool InsertSalesOrder(SalesOrderRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (string.IsNullOrWhiteSpace(request.VoucherNo))
            {
                throw new Exception("VoucherNo is required.");
            }

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();

                string voucherLinejson = JsonConvert.SerializeObject(request.VoucherLines);
                string SaleOrderline = ConvertJsonToxml(voucherLinejson);

                using (SqlCommand cmd = new SqlCommand("dbo.SP_VoucherInsert", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter p = cmd.Parameters.Add("@TagNo", SqlDbType.VarChar, 10);
                    p.Value = "0";

                    cmd.Parameters.Add("@vhid", SqlDbType.VarChar, 50).Value =
                        request.VoucherNo ?? (object)DBNull.Value;

                    cmd.Parameters.Add("@VoucherDate", SqlDbType.DateTime).Value =
                        request.VoucherDate;

                    cmd.Parameters.Add("@status", SqlDbType.VarChar, 50).Value =
                        request.Status ?? (object)DBNull.Value;

                    cmd.Parameters.Add("@CustomerCode", SqlDbType.VarChar, 50).Value =
                        request.CustomerCode ?? (object)DBNull.Value;

                    cmd.Parameters.Add("@createdby", SqlDbType.VarChar, 50).Value = "2022";

                    cmd.Parameters.Add("@handlingunit", SqlDbType.VarChar, 50).Value =
                        request.HandlingUnit ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@bcode", SqlDbType.VarChar, 50).Value =
                        request.BranchCode ?? (object)DBNull.Value;

                    cmd.Parameters.Add("@voucherLine", SqlDbType.Xml).Value =
                        string.IsNullOrEmpty(SaleOrderline)
                            ? (object)DBNull.Value
                            : SaleOrderline;

                    cmd.ExecuteNonQuery();
                    return true;
                }
            }

        }
          public static string ConvertJsonToxml(string json)

            {

                XmlDocument doc = new XmlDocument();

                XDocument xmlDoc = new XDocument(new XDeclaration("1.0", "utf-8", ""));

                XElement root = new XElement("Root");

                root.Name = "DocumentElement";

                var dataTable = JsonConvert.DeserializeObject<DataTable>(json);

                root.Add(

                from row in dataTable.AsEnumerable()

                select new XElement("Structure",

                from column in dataTable.Columns.Cast<DataColumn>()

                select new XElement(column.ColumnName, row[column])

                  )

                  );

                xmlDoc.Add(root);

                return xmlDoc.ToString();

            }

        }
    
}
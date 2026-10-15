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
    public class ItemsDLL : BaseClass
    {
        protected List<ItemsModule> getItems_DLL(int branchid, int empcode)
        {

            List<ItemsModule> JobRequestList = new List<ItemsModule>();
            using (SqlConnection Con = new SqlConnection(constring))
            {
                string Query = string.Empty;
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("getvoucherItems", Con))
                {
                    //cmd.Parameters.AddWithValue("@branchid", branchid);
                    cmd.Parameters.AddWithValue("@empid", empcode);
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
        protected List<ItemsModule> getofflineitem_DLL(string branchcode, int userid, string itemtype)
        {

            List<ItemsModule> JobRequestList = new List<ItemsModule>();
            using (SqlConnection Con = new SqlConnection(constring))
            {
                string Query = string.Empty;
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("getofflineitem", Con))
                {
                    //cmd.Parameters.AddWithValue("@branchcode", branchcode);
                    cmd.Parameters.AddWithValue("@userid", userid);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    Con.Open();
                    sd.Fill(dt);
                    Con.Close();
                }
                JobRequestList.AddRange(ConvertDataTable<ItemsModule>(dt));
            }

            if (itemtype == "h")
            {
                using (SqlConnection Con = new SqlConnection(constring))
                {
                    string Query = string.Empty;
                    DataTable dt = new DataTable();
                    using (SqlCommand cmd = new SqlCommand("getofflinehandlingunit_load", Con))
                    {
                        //cmd.Parameters.AddWithValue("@branchcode", branchcode);
                        cmd.Parameters.AddWithValue("@userid", userid);
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter sd = new SqlDataAdapter(cmd);
                        Con.Open();
                        sd.Fill(dt);
                        Con.Close();
                    }
                    JobRequestList.AddRange(ConvertDataTable<ItemsModule>(dt));
                }
            }
            else
            {

                using (SqlConnection Con = new SqlConnection(constring))
                {
                    string Query = string.Empty;
                    DataTable dt = new DataTable();
                    using (SqlCommand cmd = new SqlCommand("getofflineitem_load", Con))
                    {
                        //cmd.Parameters.AddWithValue("@branchcode", branchcode);
                        cmd.Parameters.AddWithValue("@userid", userid);
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter sd = new SqlDataAdapter(cmd);
                        Con.Open();
                        sd.Fill(dt);
                        Con.Close();
                    }
                    JobRequestList.AddRange(ConvertDataTable<ItemsModule>(dt));
                }

            }


            return JobRequestList;
        }
        protected List<ItemsModule> getItemsData_DLL(int roleid, string branchcode, int empcode)
        {

            List<ItemsModule> JobRequestList = new List<ItemsModule>();
            using (SqlConnection Con = new SqlConnection(constring))
            {
                string Query = string.Empty;
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("getvoucherItemsdata", Con))
                {
                    //cmd.Parameters.AddWithValue("@branchcode", branchcode);
                    cmd.Parameters.AddWithValue("@empid", empcode);
                    cmd.Parameters.AddWithValue("@roleid", roleid);
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

        protected List<ItemsModule> getItemsDatabyvhid_DLL(int companyid, string voucherno)
        {

            List<ItemsModule> JobRequestList = new List<ItemsModule>();
            using (SqlConnection Con = new SqlConnection(constring))
            {
                string Query = string.Empty;
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("getitembyvoucherno", Con))
                {
                    cmd.Parameters.AddWithValue("@voucherno", voucherno);
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

        protected List<ItemsModule> getDeliveredItemsDatabyjobid_DLL(int jobid)
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
         

        protected List<ItemsModule> getDeliveredHandlingUnitLoad_DLL(int jobid)
        {

            List<ItemsModule> JobRequestList = new List<ItemsModule>();
            using (SqlConnection Con = new SqlConnection(constring))
            {
                string Query = string.Empty;
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("getdeliveredhandlingunitload", Con))
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

        protected List<ItemsModule> getofflineItems_DLL(int userid, string branchcode)
        {

            List<ItemsModule> JobRequestList = new List<ItemsModule>();
            using (SqlConnection Con = new SqlConnection(constring))
            {
                string Query = string.Empty;
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("getofflineitem", Con))
                {
                    //cmd.Parameters.AddWithValue("@branchcode", branchcode);
                    cmd.Parameters.AddWithValue("@userid", userid);
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
        #region Old saveItemData
        //protected bool saveItemData_DLL(int jobid, string drivercode, List<ItemsModule> lstitemsModules)
        //{
        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        dt.Columns.Add(new DataColumn("jobid", typeof(int)));
        //        dt.Columns.Add(new DataColumn("voucherno", typeof(string)));
        //        dt.Columns.Add(new DataColumn("ItemCode", typeof(int)));
        //        dt.Columns.Add(new DataColumn("ItemName", typeof(string)));
        //        dt.Columns.Add(new DataColumn("itemtype", typeof(int)));
        //        dt.Columns.Add(new DataColumn("itemuom", typeof(string)));
        //        dt.Columns.Add(new DataColumn("OrderQty", typeof(float)));
        //        dt.Columns.Add(new DataColumn("VoucherQty", typeof(float)));
        //        dt.Columns.Add(new DataColumn("Price", typeof(float)));
        //        dt.Columns.Add(new DataColumn("GrossAmount", typeof(float)));
        //        dt.Columns.Add(new DataColumn("Discount", typeof(float)));
        //        dt.Columns.Add(new DataColumn("tax", typeof(float)));
        //        dt.Columns.Add(new DataColumn("FinalAmount", typeof(float)));
        //        dt.Columns.Add(new DataColumn("syncdatetime", typeof(DateTime)));
        //        dt.Columns.Add(new DataColumn("isactive", typeof(bool)));
        //        dt.Columns.Add(new DataColumn("isdeleted", typeof(bool)));
        //        dt.Columns.Add(new DataColumn("createdby", typeof(string)));
        //        dt.Columns.Add(new DataColumn("createdon", typeof(DateTime)));
        //        dt.Columns.Add(new DataColumn("modifyby", typeof(int)));
        //        dt.Columns.Add(new DataColumn("modifyon", typeof(DateTime)));
        //        dt.Columns.Add(new DataColumn("rejectedqty", typeof(float)));
        //        dt.Columns.Add(new DataColumn("rejectedreason", typeof(string)));
        //        dt.Columns.Add(new DataColumn("rejectedremark", typeof(string)));
        //        dt.Columns.Add(new DataColumn("lotno", typeof(string)));
        //        dt.Columns.Add(new DataColumn("expirydate", typeof(DateTime)));
        //        dt.Columns.Add(new DataColumn("orderno", typeof(string)));
        //        dt.Columns.Add(new DataColumn("companyid", typeof(int)));

        //        foreach (var item in lstitemsModules)
        //        {
        //            DataRow dr = dt.NewRow();


        //            dr["jobid"] = jobid;//, typeof(int)));
        //            dr["voucherno"] = item.voucherno;//, typeof(string)));
        //            dr["ItemCode"] = string.IsNullOrEmpty(item.itemcode) ? DBNull.Value: (object)int.Parse(item.itemcode);//, typeof(int)));
        //            dr["ItemName"] = item.itemname;//, typeof(string)));
        //            dr["itemtype"] = string.IsNullOrEmpty(item.itemtype) ? DBNull.Value : (object)int.Parse(item.itemtype);//, typeof(int)));
        //            dr["itemuom"] = item.itemuom;//, typeof(string)));
        //            dr["OrderQty"] = item.orderqty;//, typeof(float)));
        //            dr["VoucherQty"] = item.voucherqty;//, typeof(float)));
        //            dr["Price"] = item.price;//, typeof(float)));
        //            dr["GrossAmount"] = item.grossamount;//, typeof(float)));
        //            dr["Discount"] = item.discount;//, typeof(float)));
        //            dr["tax"] = item.tax;//, typeof(float)));
        //            dr["FinalAmount"] = item.finalamount;//, typeof(float)));
        //            dr["syncdatetime"] = item.syncdatetime;//, typeof(DateTime)));
        //            dr["isactive"] = true;//, typeof(bool)));
        //            dr["isdeleted"] = false;//, typeof(bool)));
        //            dr["createdby"] = drivercode;//, typeof(string)));
        //            dr["createdon"] = DateTime.Now;//, typeof(DateTime)));
        //            dr["modifyby"] = DBNull.Value;//, typeof(int)));
        //            dr["modifyon"] = DBNull.Value;//, typeof(DateTime)));
        //            dr["rejectedqty"] = item.rejectedqty;//, typeof(float)));
        //            dr["rejectedreason"] = item.rejectedreason;//, typeof(string)));
        //            dr["rejectedremark"] = item.rejectedremark;//, typeof(string)));
        //            dr["lotno"] = item.lotno;//, typeof(string)));
        //            dr["expirydate"] = (item.expirydate==null || item.expirydate.Value.Year < 2000)?DBNull.Value:(object)item.expirydate;//, typeof(DateTime)));
        //            dr["orderno"] = item.orderno;//, typeof(string)));
        //            dr["companyid"] = item.companyid;//, typeof(int)));


        //            //dr["ID"] = i;
        //            //dr["isDeleted"] = DBNull.Value;
        //            //dr["Manual"] = DBNull.Value;
        //            //dr["source"] = "Test";

        //            dt.Rows.Add(dr);
        //        }


        //        using (SqlConnection con = new SqlConnection(constring))
        //        {
        //            SqlBulkCopy objbulk = new SqlBulkCopy(con);

        //            //assign Destination table name  
        //            objbulk.DestinationTableName = "JobDeliveryLines";

        //            //objbulk.ColumnMappings.Add("ID", "ID");
        //            //objbulk.ColumnMappings.Add("isDeleted", "isDeleted");
        //            //objbulk.ColumnMappings.Add("Manual", "Manual");
        //            //objbulk.ColumnMappings.Add("source", "source");

        //            objbulk.ColumnMappings.Add("jobid", "jobid");// typeof(int)));
        //            objbulk.ColumnMappings.Add("voucherno", "voucherno");//typeof(string)));
        //            objbulk.ColumnMappings.Add("ItemCode", "ItemCode");//typeof(int)));
        //            objbulk.ColumnMappings.Add("ItemName", "ItemName");//typeof(string)));
        //            objbulk.ColumnMappings.Add("itemtype", "itemtype");//typeof(int)));
        //            objbulk.ColumnMappings.Add("itemuom", "itemuom");//typeof(string)));
        //            objbulk.ColumnMappings.Add("OrderQty", "OrderQty");//typeof(float)));
        //            objbulk.ColumnMappings.Add("VoucherQty", "VoucherQty");//typeof(float)));
        //            objbulk.ColumnMappings.Add("Price", "Price");//typeof(float)));
        //            objbulk.ColumnMappings.Add("GrossAmount", "GrossAmount");//typeof(float)));
        //            objbulk.ColumnMappings.Add("Discount", "Discount");//typeof(float)));
        //            objbulk.ColumnMappings.Add("tax", "tax");//typeof(float)));
        //            objbulk.ColumnMappings.Add("FinalAmount", "FinalAmount");//typeof(float)));
        //            objbulk.ColumnMappings.Add("syncdatetime", "syncdatetime");//typeof(DateTime)));
        //            objbulk.ColumnMappings.Add("isactive", "isactive");//typeof(bool)));
        //            objbulk.ColumnMappings.Add("isdeleted", "isdeleted");//typeof(bool)));
        //            objbulk.ColumnMappings.Add("createdby", "createdby");//typeof(string)));
        //            objbulk.ColumnMappings.Add("createdon", "createdon");//typeof(DateTime)));
        //            objbulk.ColumnMappings.Add("modifyby", "modifyby");//typeof(int)));
        //            objbulk.ColumnMappings.Add("modifyon", "modifyon");//typeof(DateTime)));
        //            objbulk.ColumnMappings.Add("rejectedqty", "rejectedqty");//typeof(float)));
        //            objbulk.ColumnMappings.Add("rejectedreason", "rejectedreason");//typeof(string)));
        //            objbulk.ColumnMappings.Add("rejectedremark", "rejectedremark");//typeof(string)));
        //            objbulk.ColumnMappings.Add("lotno", "lotno");//typeof(string)));
        //            objbulk.ColumnMappings.Add("expirydate", "expirydate");//typeof(DateTime)));
        //            objbulk.ColumnMappings.Add("orderno", "orderno");//typeof(string)));
        //            objbulk.ColumnMappings.Add("companyid", "companyid");//typeof(int)));



        //            con.Open();
        //            //insert bulk Records into DataBase.  
        //            objbulk.WriteToServer(dt);
        //            con.Close();
        //        }
        //        return true;
        //    }catch (Exception ex)
        //    {
        //        Helper.LogException(ex, ex.InnerException?.Message);
        //        throw;
        //        return false;
        //    }


        //    //int i = 0;
        //    //using (SqlConnection Con = new SqlConnection(constring))
        //    //{
        //    //    string Query = string.Empty;
        //    //    DataTable dt = new DataTable();
        //    //    foreach (var item in lstitemsModules)
        //    //    {
        //    //        if(item.expirydate.HasValue && item.expirydate.Value.Year < 2000)
        //    //        {
        //    //            item.expirydate = null;
        //    //        }

        //    //        Query += "INSERT INTO [dbo].[JobDeliveryLines]([jobid],[voucherno],[ItemCode],[ItemName],[itemtype],[itemuom],[OrderQty],[VoucherQty],[Price],[GrossAmount],[Discount],[tax],[FinalAmount],[syncdatetime],[isactive],[isdeleted],[createdby],[createdon], [rejectedqty], [rejectedreason], [rejectedremark], [lotno], [expirydate], [orderno], [companyid])";
        //    //        Query += "Values("+ jobid + ", '"+ ReplaceSQLError(item.voucherno)+"', '"+ ReplaceSQLError(item.itemcode)+"', '"+ ReplaceSQLError(item.itemname)+ "', '" + item.itemtype + "', '" + item.itemuom + "', '" + item.orderqty + "', '" + item.voucherqty + "', '" + item.price + "', '" + item.grossamount + "', '" + item.discount + "', '" + item.tax + "', '" + item.finalamount + "', '" + item.syncdatetime + "', 1,0, " + drivercode + ", getdate(), '" + item.rejectedqty + "', '" + ReplaceSQLError(item.rejectedreason) + "', '" + ReplaceSQLError(item.rejectedremark) + "', '" + ReplaceSQLError(item.lotno) + "', '" + item.expirydate + "', '" + ReplaceSQLError(item.orderno) + "', '" + item.companyid + "' );";
        //    //    }
        //    //    using (SqlCommand cmd = new SqlCommand(Query, Con))
        //    //    {
        //    //        cmd.CommandType = CommandType.Text;
        //    //        Con.Open();
        //    //        i = cmd.ExecuteNonQuery();
        //    //        Con.Close();
        //    //    }
        //    //}
        //    //if (i >= 1) return true; else return false;
        //}
        #endregion

        protected bool saveItemData_DLL(int jobid, string drivercode, List<ItemsModule> lstitemsModules)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("jobid", typeof(int)));
                dt.Columns.Add(new DataColumn("voucherno", typeof(string)));
                dt.Columns.Add(new DataColumn("ItemCode", typeof(int)));
                dt.Columns.Add(new DataColumn("ItemName", typeof(string)));
                dt.Columns.Add(new DataColumn("itemtype", typeof(int)));
                dt.Columns.Add(new DataColumn("itemuom", typeof(string)));
                dt.Columns.Add(new DataColumn("OrderQty", typeof(float)));
                dt.Columns.Add(new DataColumn("VoucherQty", typeof(float)));
                dt.Columns.Add(new DataColumn("Price", typeof(float)));
                dt.Columns.Add(new DataColumn("GrossAmount", typeof(float)));
                dt.Columns.Add(new DataColumn("Discount", typeof(float)));
                dt.Columns.Add(new DataColumn("tax", typeof(float)));
                dt.Columns.Add(new DataColumn("FinalAmount", typeof(float)));
                dt.Columns.Add(new DataColumn("syncdatetime", typeof(DateTime)));
                dt.Columns.Add(new DataColumn("isactive", typeof(bool)));
                dt.Columns.Add(new DataColumn("isdeleted", typeof(bool)));
                dt.Columns.Add(new DataColumn("createdby", typeof(string)));
                dt.Columns.Add(new DataColumn("createdon", typeof(DateTime)));
                dt.Columns.Add(new DataColumn("modifyby", typeof(int)));
                dt.Columns.Add(new DataColumn("modifyon", typeof(DateTime)));
                dt.Columns.Add(new DataColumn("rejectedqty", typeof(float)));
                dt.Columns.Add(new DataColumn("rejectedreason", typeof(string)));
                dt.Columns.Add(new DataColumn("rejectedremark", typeof(string)));
                dt.Columns.Add(new DataColumn("lotno", typeof(string)));
                dt.Columns.Add(new DataColumn("expirydate", typeof(DateTime)));
                dt.Columns.Add(new DataColumn("orderno", typeof(string)));
                dt.Columns.Add(new DataColumn("companyid", typeof(int)));

                // Row-level validation & addition
                foreach (var item in lstitemsModules)
                {
                    try
                    {
                        DataRow dr = dt.NewRow();

                        dr["jobid"] = jobid;
                        dr["voucherno"] = item.voucherno;
                        dr["ItemCode"] = string.IsNullOrEmpty(item.itemcode) ? DBNull.Value : (object)int.Parse(item.itemcode);
                        dr["ItemName"] = item.itemname;
                        dr["itemtype"] = string.IsNullOrEmpty(item.itemtype) ? DBNull.Value : (object)int.Parse(item.itemtype);
                        dr["itemuom"] = item.itemuom;
                        dr["OrderQty"] = item.orderqty;
                        dr["VoucherQty"] = item.voucherqty;
                        dr["Price"] = item.price;
                        dr["GrossAmount"] = item.grossamount;
                        dr["Discount"] = item.discount;
                        dr["tax"] = item.tax;
                        dr["FinalAmount"] = item.finalamount;
                        dr["syncdatetime"] = item.syncdatetime;
                        dr["isactive"] = true;
                        dr["isdeleted"] = false;
                        dr["createdby"] = drivercode;
                        dr["createdon"] = DateTime.Now;
                        dr["modifyby"] = DBNull.Value;
                        dr["modifyon"] = DBNull.Value;
                        dr["rejectedqty"] = item.rejectedqty;
                        dr["rejectedreason"] = item.rejectedreason;
                        dr["rejectedremark"] = item.rejectedremark;
                        dr["lotno"] = item.lotno;
                        dr["expirydate"] = (item.expirydate == null || item.expirydate.Value.Year < 2000) ? DBNull.Value : (object)item.expirydate;
                        dr["orderno"] = item.orderno;
                        dr["companyid"] = item.companyid;

                        dt.Rows.Add(dr);
                    }
                    catch (Exception rowEx)
                    {
                        
                        Helper.LogException(rowEx, $"Error adding row for VoucherNo {item.voucherno}: {rowEx.InnerException?.Message}");
                        throw; 
                    }
                }

                // Bulk insert with transaction
                using (SqlConnection con = new SqlConnection(constring))
                {
                    con.Open();
                    using (SqlTransaction tran = con.BeginTransaction())
                    {
                        try
                        {
                            using (SqlBulkCopy objbulk = new SqlBulkCopy(con, SqlBulkCopyOptions.Default, tran))
                            {
                                objbulk.BulkCopyTimeout = 0;
                                objbulk.DestinationTableName = "JobDeliveryLines";
                                objbulk.ColumnMappings.Add("jobid", "jobid");
                                objbulk.ColumnMappings.Add("voucherno", "voucherno");
                                objbulk.ColumnMappings.Add("ItemCode", "ItemCode");
                                objbulk.ColumnMappings.Add("ItemName", "ItemName");
                                objbulk.ColumnMappings.Add("itemtype", "itemtype");
                                objbulk.ColumnMappings.Add("itemuom", "itemuom");
                                objbulk.ColumnMappings.Add("OrderQty", "OrderQty");
                                objbulk.ColumnMappings.Add("VoucherQty", "VoucherQty");
                                objbulk.ColumnMappings.Add("Price", "Price");
                                objbulk.ColumnMappings.Add("GrossAmount", "GrossAmount");
                                objbulk.ColumnMappings.Add("Discount", "Discount");
                                objbulk.ColumnMappings.Add("tax", "tax");
                                objbulk.ColumnMappings.Add("FinalAmount", "FinalAmount");
                                objbulk.ColumnMappings.Add("syncdatetime", "syncdatetime");
                                objbulk.ColumnMappings.Add("isactive", "isactive");
                                objbulk.ColumnMappings.Add("isdeleted", "isdeleted");
                                objbulk.ColumnMappings.Add("createdby", "createdby");
                                objbulk.ColumnMappings.Add("createdon", "createdon");
                                objbulk.ColumnMappings.Add("modifyby", "modifyby");
                                objbulk.ColumnMappings.Add("modifyon", "modifyon");
                                objbulk.ColumnMappings.Add("rejectedqty", "rejectedqty");
                                objbulk.ColumnMappings.Add("rejectedreason", "rejectedreason");
                                objbulk.ColumnMappings.Add("rejectedremark", "rejectedremark");
                                objbulk.ColumnMappings.Add("lotno", "lotno");
                                objbulk.ColumnMappings.Add("expirydate", "expirydate");
                                objbulk.ColumnMappings.Add("orderno", "orderno");
                                objbulk.ColumnMappings.Add("companyid", "companyid");
                                objbulk.WriteToServer(dt);
                            }

                            tran.Commit();
                            return true;
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                            Helper.LogException(ex, ex.InnerException?.Message);
                            throw;
                        }
                        finally
                        {
                            con.Close();
                        }
                    }
                }
            }
            catch (Exception exOuter)
            {
                Helper.LogException(exOuter, exOuter.InnerException?.Message);
                return false;
            }
        }

        public string ReplaceSQLError(string val)
        {
            return (val + "").Replace("'", "''");
        }


    }

}
    

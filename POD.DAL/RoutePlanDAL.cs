using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using POD.Models.RoutePlan;

namespace POD.DAL
{
    public class RoutePlanDAL : BaseClass
    {
        public List<CustomerDetailVM> GetCustomerDetails()
        {
            List<CustomerDetailVM> lstCustomer = new List<CustomerDetailVM>();

            string sql = @"
                SELECT
                    customercode,
                    firmname,
                    contactpersonname,
                    emailid,
                    contactno,
                    pincode,
                    address,
                    address1,
                    address2,
                    address3,
                    address4,
                    address5,
                    Isactive
                FROM dbo.sys_customer
                WHERE ISNULL(Isdeleted, 0) = 0
                ORDER BY customerid;";

            using (SqlConnection con = new SqlConnection(constring))
            {
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string[] addressParts =
                            {
                                GetString(reader, "address"),
                                GetString(reader, "address1"),
                                GetString(reader, "address2"),
                                GetString(reader, "address3"),
                                GetString(reader, "address4"),
                                GetString(reader, "address5")
                            };

                            List<string> validAddressParts = new List<string>();

                            foreach (string part in addressParts)
                            {
                                if (!string.IsNullOrWhiteSpace(part))
                                {
                                    validAddressParts.Add(part.Trim());
                                }
                            }

                            CustomerDetailVM objCustomer = new CustomerDetailVM();

                            objCustomer.CustomerCode =
                                GetString(reader, "customercode");

                            objCustomer.CustomerName =
                                GetString(reader, "firmname");

                            objCustomer.ContactPersonName =
                                GetString(reader, "contactpersonname");

                            objCustomer.Email =
                                GetString(reader, "emailid");

                            objCustomer.ContactNo =
                                GetString(reader, "contactno");

                            objCustomer.PinCode =
                                GetString(reader, "pincode");

                            objCustomer.Address =
                                string.Join(", ", validAddressParts);

                            objCustomer.CusStatus =
                                GetCustomerStatus(reader, "Isactive");

                            // These fields require verified lookup columns.
                            objCustomer.AddressType = null;
                            objCustomer.City = null;
                            objCustomer.State = null;
                            objCustomer.Country = null;
                            objCustomer.Latitude = null;
                            objCustomer.Longitude = null;

                            lstCustomer.Add(objCustomer);
                        }
                    }
                }
            }

            return lstCustomer;
        }

        private static string GetString(SqlDataReader reader,string columnName)
        {
            int index = reader.GetOrdinal(columnName);

            if (reader.IsDBNull(index))
            {
                return null;
            }

            return Convert.ToString(reader.GetValue(index));
        }

        private static string GetCustomerStatus( SqlDataReader reader,string columnName)
        {
            int index = reader.GetOrdinal(columnName);

            if (reader.IsDBNull(index))
            {
                return "Inactive";
            }

            object value = reader.GetValue(index);

            if (value is bool)
            {
                return (bool)value ? "Active" : "Inactive";
            }

            string status = Convert.ToString(value);

            if (status == "1" ||
                string.Equals(status, "true",
                    StringComparison.OrdinalIgnoreCase) ||
                string.Equals(status, "Active",
                    StringComparison.OrdinalIgnoreCase) ||
                string.Equals(status, "Y",
                    StringComparison.OrdinalIgnoreCase))
            {
                return "Active";
            }

            return "Inactive";
        }



        public string SyncCustomer(CustomerDetailVM customer, int companyId)
        {
            if (customer == null)
            {
                throw new ArgumentException("Customer data is required.");
            }

            if (string.IsNullOrWhiteSpace(customer.CustomerCode))
            {
                throw new ArgumentException("CustomerCode is required.");
            }

            string sql = @"
        IF EXISTS
        (
            SELECT 1
            FROM dbo.sys_customer
            WHERE customercode = @CustomerCode
              AND companyid = @CompanyId
              AND ISNULL(Isdeleted, 0) = 0
        )
        BEGIN
            UPDATE dbo.sys_customer
            SET
                firmname = @CustomerName,
                contactpersonname = @ContactPersonName,
                emailid = @Email,
                contactno = @ContactNo,
                pincode = @PinCode,
                address = @Address,
                Isactive = @IsActive,
                modifyon = GETDATE()
            WHERE customercode = @CustomerCode
              AND companyid = @CompanyId
              AND ISNULL(Isdeleted, 0) = 0;

            SELECT 'UPDATE';
        END
        ELSE
        BEGIN
            INSERT INTO dbo.sys_customer
            (
                customercode,
                firmname,
                contactpersonname,
                emailid,
                contactno,
                pincode,
                address,
                companyid,
                Isactive,
                Isdeleted,
                createdon
            )
            VALUES
            (
                @CustomerCode,
                @CustomerName,
                @ContactPersonName,
                @Email,
                @ContactNo,
                @PinCode,
                @Address,
                @CompanyId,
                @IsActive,
                0,
                GETDATE()
            );

            SELECT 'INSERT';
        END";

            using (SqlConnection con = new SqlConnection(constring))
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue(
                    "@CustomerCode", customer.CustomerCode.Trim());

                cmd.Parameters.AddWithValue("@CompanyId", companyId);

                cmd.Parameters.AddWithValue(
                    "@CustomerName",
                    (object)customer.CustomerName ?? DBNull.Value);

                cmd.Parameters.AddWithValue(
                    "@ContactPersonName",
                    (object)customer.ContactPersonName ?? DBNull.Value);

                cmd.Parameters.AddWithValue(
                    "@Email",
                    (object)customer.Email ?? DBNull.Value);

                cmd.Parameters.AddWithValue(
                    "@ContactNo",
                    (object)customer.ContactNo ?? DBNull.Value);

                cmd.Parameters.AddWithValue(
                    "@PinCode",
                    (object)customer.PinCode ?? DBNull.Value);

                cmd.Parameters.AddWithValue(
                    "@Address",
                    (object)customer.Address ?? DBNull.Value);

                bool isActive = string.Equals(
                    customer.CusStatus,
                    "Active",
                    StringComparison.OrdinalIgnoreCase);

                cmd.Parameters.AddWithValue("@IsActive", isActive);

                con.Open();

                string action = Convert.ToString(cmd.ExecuteScalar());

                if (action == "INSERT")
                {
                    return "Customer inserted successfully.";
                }

                if (action == "UPDATE")
                {
                    return "Customer updated successfully.";
                }

                throw new Exception("Customer operation could not be identified.");
            }
        }

    }
}
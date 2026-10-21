
using POD.DAL;
using POD.Models;
using POD.Models.RoutePlan;
using System;
using System.Collections.Generic;

namespace POD.Logics
{
    public class RoutePlanBLL : RoutePlanDAL
    {
        public List<CustomerDetailVM> GetCustomerDetails_BLL()
        {
            return GetCustomerDetails();
        }



public object SyncCustomers_BLL(SyncCustomerRequestVM request)
    {
        if (request == null)
        {
            throw new ArgumentException("Request data is required.");
        }

        if (request.CompanyId <= 0)
        {
            throw new ArgumentException("Valid CompanyId is required.");
        }

        if (request.Customers == null || request.Customers.Count == 0)
        {
            throw new ArgumentException("At least one customer is required.");
        }

        int insertedCount = 0;
        int updatedCount = 0;

        foreach (CustomerDetailVM customer in request.Customers)
        {
            string result = SyncCustomer(customer, request.CompanyId);

            if (result == "Customer inserted successfully.")
            {
                insertedCount++;
            }
            else if (result == "Customer updated successfully.")
            {
                updatedCount++;
            }
        }

        string statusMessage;

        if (insertedCount > 0 && updatedCount > 0)
        {
            statusMessage = "Customers inserted and updated successfully.";
        }
        else if (insertedCount > 0)
        {
            statusMessage = "Customer inserted successfully.";
        }
        else if (updatedCount > 0)
        {
            statusMessage = "Customer updated successfully.";
        }
        else
        {
            statusMessage = "No customers were processed.";
        }

        return new
        {
            InsertedCount = insertedCount,
            UpdatedCount = updatedCount,
            ProcessedCount = insertedCount + updatedCount,
            StatusMessage = statusMessage
        };
    }

}
}

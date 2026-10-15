using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POD.DAL;
using POD.Models;
using POD.Models.Customer;

namespace POD.Logics
{
    public class CustomerBLL : CustomerDAL
    {
        public List<CompanyDetailVM> GetCompanyDetail_BLL(string companyseries)
        {
            return GetCompanyDetail(companyseries);
        }
         
        public int RegisterCustomer_BLL(RegisterCustomerVM model)
        {
            return RegisterCustomer(model);
        }

        public List<CustomerLoginResponseVM> CustomerLogin_BLL(CustomerLoginVM model)
        {
            return CustomerLogin(model);
        }

        public int SendEmailForVerifyEmailId_BLL(string email, bool isCheckDuplicate, int companyid)
        {
            return SendEmailForVerifyEmailId( email,  isCheckDuplicate,  companyid);
        }

        public int SendSMSForVerifyMobile_BLL(string mobile, bool isCheckDuplicate, int companyid)
        {
            return SendSMSForVerifyMobile( mobile,  isCheckDuplicate,  companyid);
        }


    }
}

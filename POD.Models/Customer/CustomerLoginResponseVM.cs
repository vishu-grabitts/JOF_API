using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POD.Models.Customer
{
    public class CustomerLoginResponseVM
    {

        public int companyid { get; set; }
        public string companycode { get; set; }
        public string companyname { get; set; }
        public string companyseries { get; set; }


        public string empname { get; set; }
        public string emailid { get; set; }
        public string contactno { get; set; }
        public string pic { get; set; }
        public string country { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public string pincode { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string comapanyaddress { get; set; }


        public int status { get; set; }
        public string statusmessage { get; set; }
        public List<CompanyDetailVM> companydetailvm { get; set; }
        public CustomerLoginResponseVM()
        {
            companydetailvm = new List<CompanyDetailVM>();
        }

    }
}

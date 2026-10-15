using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POD.Models.Customer
{
    public class RegisterCustomerVM
    {
        public int companyid { get; set; }
        public string customername { get; set; }
        public string contactperson { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }
        public bool isemailverified { get; set; }
        public bool ismobileverified { get; set; }
        public string password { get; set; }
    }
}

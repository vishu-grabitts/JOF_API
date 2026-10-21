using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POD.Models
{
    public class CustomerSaveVM
    {
        public int CustomerId { get; set; }
        public int CompanyId { get; set; }
        public int UserId { get; set; }

        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string ContactPersonName { get; set; }
        public string Email { get; set; }
        public string ContactNo { get; set; }
        public string PinCode { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }
    }
}

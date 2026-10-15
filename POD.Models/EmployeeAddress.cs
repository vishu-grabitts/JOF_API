using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POD.Models
{ 
   public class EmployeeAddress
    { 
        public int id { get; set; }
        public int empid { get; set; }
        public string empcode { get; set; }

        public string pincode { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string addresstype { get; set; }
        public string houseno { get; set; }
        public string landmark { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
    }
     
}



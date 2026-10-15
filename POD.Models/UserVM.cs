using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POD.Models
{
    public class UserVM
    {
         public int userid { get; set; }
         public int empid { get; set; }
        public int companyid { get; set; }

        public string empcode { get; set; }
        public string empname { get; set; }
    }
      
}

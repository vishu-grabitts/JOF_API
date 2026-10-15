using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POD.Models
{ 
   public class EmployeeLocation
    {
        public int id { get; set; }
        public int empid { get; set; }
        //public string empcode { get; set; }

        public string latitude { get; set; }
        public string longitude { get; set; }

        public bool isactive { get; set; }
        public DateTime createdon { get; set; }
        public string type { get; set; }
        public string batterylevel { get; set; }
        
    }
     
}



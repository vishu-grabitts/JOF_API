using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POD.Models
{
    public class EmployeeLocationVM
    {
        public double lat { get; set; }
        public double lng { get; set; }
        public DateTime? createdon { get; set; }
        public string batterylevel { get; set; }
    }
}

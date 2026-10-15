using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POD.Models
{
    public class JobImagesModule
    {
        public int jiid { get; set; } 
        public int jobid { get; set; }
        public int jaid { get; set; }
        public DateTime orderdate { get; set; }
        public string image { get; set; }
        public string remark { get; set; }
        public string signature { get; set; }
    }

    public class JobSignatureModule
    {
        public int jiid { get; set; }
        public int jobid { get; set; }
        public int jaid { get; set; }
        public DateTime orderdate { get; set; }
        public string image { get; set; }
        public string remark { get; set; }
        public string signature { get; set; }
    }
}

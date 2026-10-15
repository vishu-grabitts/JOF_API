using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POD.Models
{
    public class JobHistoryModule
    {
        public int jaid { get; set; }
        public int jobid { get; set; }
        public int actioncode { get; set; }
        public string actionname { get; set; }
        public DateTime actiontime { get; set; }
        public DateTime scheduletime { get; set; }
        public string rejectremark { get; set; }
        public string signature { get; set; }
        public string collectedby { get; set; }
        public string longitude { get; set; }
        public string latitude { get; set; }
        public string attemptedby { get; set; }
    }
}

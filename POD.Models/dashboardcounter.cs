using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POD.Models
{
   public class dashboardcounter
    {
        public int pending { get; set; }
        public int inprocess { get; set; }
        public int onhold { get; set; }
        public int completed { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POD.Models
{
    public class Notification
    {
        public int nmid { get; set; }
        public int companyid { get; set; }
        public int actionid { get; set; }
        public string notificationtype { get; set; }
        public string triggertype { get; set; }
        public string message { get; set; }
        public string title { get; set; }
        public string image { get; set; }
    }
}

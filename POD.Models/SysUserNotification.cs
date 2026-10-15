using System;

namespace POD.Models
{
    public class SysUserNotification
    {
        public int empid { get; set; }
        public string title { get; set; }
        public string message { get; set; }
        public string data { get; set; }
        public DateTime createdon { get; set; }
        public bool isread { get; set; }
    }
}

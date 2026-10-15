using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POD.Models
{
    public class UserLoginHistorySaveParameters
    {
        public int UserId { get; set; }

        public string UserToken { get; set; }

        public DateTime TokenExpireOn { get; set; }

        public string DeviceName { get; set; }

        public string IPAddress { get; set; }

        public bool RememberMe { get; set; }

        public bool IsLoggedIn { get; set; }
    }
}

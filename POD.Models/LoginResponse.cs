using System;

namespace POD.Models
{
    public class LoginResponse
    {
        public string Token { get; set; }

         public string Userid { get; set; }

        public string Role { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }


        public DateTime TokenExpireOn { get; set; }

        public LoginUserModel User { get; set; }
    }
}
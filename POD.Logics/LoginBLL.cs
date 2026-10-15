using POD.DAL;
using POD.Models;
using System;

namespace POD.Logics
{
    public class LoginBLL
    {
        private readonly LoginDAL _loginDAL;

        public LoginBLL()
        {
            _loginDAL = new LoginDAL();
        }

        public LoginUserModel ValidateUser(
            string username,
            string password)
        {
            try
            {
                var user = _loginDAL.ValidateUser(
                    username,
                    password
                );

                return user;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
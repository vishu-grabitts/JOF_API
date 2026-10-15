using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POD.DAL;
using POD.Models;

namespace POD.Logics
{
    public class UserAccountBLL : UserAccountDAL
    {
        public int ForgotPassword_BLL(string loginId)
        {
            return ForgotPassword_DLL(loginId);
        }
        public List<UserData> GetUserLoginData_BLL(UserData objUA)
        {
            return GetUserLoginData(objUA);
        }
        public List<BranchData> GetUserBranchData_BLL(int userid)
        {
            return GetUserBranchData(userid);
        }
        public List<BranchData> GetUserBranches_BLL(int roleid, int userid)
        {
            return GetUserBranches_DLL(roleid, userid);
        }
        public bool updateloginhistory_BLL(UserData objUA)
        {
            return updateloginhistory_DLL(objUA);
        }
        public bool updatelogouthistory_BLL(UserData objUA)
        {
            return updatelogouthistory_DLL(objUA);
        }

        public bool updateuser_BLL(UserData objUA)
        {
            return updateuser_DLL(objUA);
        }

        public bool updateemail_BLL(UserData objUA)
        {
            return updateemail_DLL(objUA);
        }

        public bool updateabout_BLL(UserData objUA)
        {
            return updateabout_DLL(objUA);
        }

        public bool updatecontact_BLL(UserData objUA)
        {
            return updatecontact_DLL(objUA);
        }

        public bool updateaddress_BLL(EmployeeAddress objUA)
        {
            return updateaddress_DLL(objUA);
        }

        public string updatepic_BLL(UserData objUA)
        {
            return Updatepic_DLL(objUA);
        }

        public string saveidproof_BLL(IdProof objUA)
        {
            return SaveIdProof_DLL(objUA);
        }



        public List<IdProof> getidproof_BLL(string empcode)
        {
            return getIdProof_DLL(empcode);
        }


        public bool deleteidproof_BLL(IdProof objUA)
        {
            return Deleteidproof_DLL(objUA);
        }

        public List<EmployeeAddress> GetEmployeeAddress_BLL(string empcode)
        {
            return GetEmployeeAddress_DLL(empcode);
        }
        public bool DeleteEmployeeAddress_BLL(int empAddressId)
        {
            return DeleteEmployeeAddress_DLL(empAddressId);
        }

    }
}

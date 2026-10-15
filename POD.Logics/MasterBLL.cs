using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POD.DAL;
using POD.Models;

namespace POD.Logics 
{
    public class MasterBLL : MasterDLL
    {
        public List<SysSettingVM> GetSysSettings_BLL(int companyid, string key)
        { 
            return GetSysSettings_DLL(companyid, key);
        }
        public List<SysUserNotification> getUserNotifications_BLL(int empid)
        {
            return getUserNotifications_DLL(empid);
        }

        public bool UpdateUserNotificationRead_BLL(int empid)
        {
            return UpdateUserNotificationRead_DLL(empid);
        }
        public int GetUserNotificationCount_BLL(int empid)
        {
            return GetUserNotificationCount_DLL(empid);
        }


        public List<ActionMaster> getActionMaster_BLL()
        {
            return getActionMaster_DLL();
        }
        public List<ReasonMaster> getReasonMaster_BLL(int companyid)
        {
            return getReasonMaster_DLL(companyid);
        }

        public DataTable getMailConfigBLL(int companyid, string type, string langcode, string deliverytype)
        {
            return getMailConfig(companyid, type, langcode, deliverytype);
        }
    }
}

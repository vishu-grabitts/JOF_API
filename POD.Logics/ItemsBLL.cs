using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POD.DAL;
using POD.Models;

namespace POD.Logics
{
    public class ItemsBLL : ItemsDLL
    {
        public List<ItemsModule> getItems_BLL(int branchid, int empcode)
        {
            return getItems_DLL(branchid, empcode);
        }
        public List<ItemsModule> getofflineitem_BLL(string branchcode, int userid, string itemtype)
        {
            return getofflineitem_DLL(branchcode, userid, itemtype);
        }

        public List<ItemsModule> getItemsData_BLL(int roleid, string branchcode, int empcode)
        {
            return getItemsData_DLL(roleid, branchcode, empcode);
        }
        public List<ItemsModule> getItemsDatabyvhid_BLL(int companyid, string voucherno)
        {
            return getItemsDatabyvhid_DLL(companyid, voucherno);
        }

        public List<ItemsModule> getDeliveredItemsDatabyjobid_BLL(int jobid)
        {
            return getDeliveredItemsDatabyjobid_DLL(jobid);
        }
        public List<ItemsModule> getofflineItems_BLL(int userid, string branchcode)
        {
            return getofflineItems_DLL(userid, branchcode);
        }

        public bool saveItemData_BLL(int jobid, string drivercode, List<ItemsModule> lstitemsModules)
        {
            return saveItemData_DLL(jobid, drivercode, lstitemsModules);
        }

        public List<ItemsModule> getDeliveredHandlingUnitLoad_BLL(int jobid)
        {
            return getDeliveredHandlingUnitLoad_DLL(jobid);
        } 
    }
}

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
    public class LoadJobRequestBLL : LoadJobRequestDLL
    {
        public List<JobRequest> getJobrequestData_BLL(int roleid, int userid, string branchcode, int empid,string type, long tagno)
        {
            return getJobrequestData_DLL(roleid, userid, branchcode, empid,type,tagno);
        }
        public List<JobRequest> SaveJobActionNew(int userid, JobRequest objjobreq)
        {
            return SaveJobActionNew_DLL(userid, objjobreq);
        }
        public List<JobRequest> GetVoucherByLoadId_BLL(int loadid)
        {
            return GetVoucherByLoadId_DLL(loadid);
        }
        public List<JobRequest> GetLoadAndJobByLoadId_BLL(int loadid)
        {
            return GetLoadAndJobByLoadId_DLL(loadid);
        }
         
        public List<ItemsModule> GetDeliveredItemLoad_BLL(int loadid)
        {
            return GetDeliveredItemLoad_DLL(loadid);
        }

        public List<ItemsModule> GetItemsDatabyLoadId_BLL(int companyId, int loadid)
        {
            return GetItemsDatabyLoadId_DLL(companyId, loadid);
        }

        public List<ItemsModule> GetHandlingUnitByLoadId_BLL(int companyId, int loadid)
        {
            return GetHandlingUnitByLoadId_DLL(companyId, loadid);
        }

        

    }
}

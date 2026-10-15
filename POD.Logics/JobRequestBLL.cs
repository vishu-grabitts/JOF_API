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
    public class JobRequestBLL : JobRequestDLL
    {
        public List<JobRequest> getJobrequest_BLL(int userid, int branchid, int empid)
        {
            return getJobrequest_DLL(userid, branchid, empid);
        }
        public List<dashboardcounter> GetCounter_BLL(int userid, string branchcode, int empid, int roleid)
        {
            return GetCounter_DLL(userid, branchcode, empid,roleid);
        }

        public List<JobRequest> getJobrequestData_BLL(int roleid, int userid, string branchcode, int empid,string type, long tagno)
        {
            return getJobrequestData_DLL(roleid, userid, branchcode, empid,type,tagno);
        }
        public List<JobRequest> getUserOfflineData_BLL(int roleid, int userid, string branchcode, int empid, string type)
        {
            return getUserOfflineData_DLL(roleid, userid, branchcode, empid, type);
        }
        public List<JobRequestMultiple> SaveJobActionMultiple(int userid, JobRequestMultiple objjobreq)

        {

            return SaveJobActionMultiple_DLL(userid, objjobreq);

        }
        public List<JobRequest> SaveJobAction(int userid, JobRequest objjobreq)
        {
            return SaveJobAction_DLL(userid, objjobreq);
        }
        public List<JobRequest> SaveJobActionNew(int userid, JobRequest objjobreq)
        {
            return SaveJobActionNew_DLL(userid, objjobreq);
        }
        public string Savesignatureimages_BLL(JobImagesModule objjobimage)
        {
            return Savesignatureimages_DLL(objjobimage);
        }
        public string SendVoucherReport_BLL(int companyid,string invoiceno, string mailids)
        {
            return SendVoucherReport_DLL(companyid,invoiceno, mailids);
        }
        public string Savejobimages_BLL(int userid, JobImagesModule objimage)
        {
            return Savejobimages_DLL(userid, objimage);
        }
        public bool Deletejobimages_BLL(JobImagesModule objjobimage)
        {
            return Deletejobimages_DLL(objjobimage);
        }
        public List<JobImagesModule> getJobimages_BLL(int jaid, int jobid)
        {
            return getJobimages_DLL(jaid, jobid);
        }
        //getJobHistory_DLL
        public List<JobHistoryModule> getJobHistory_BLL(int jobid)//string vhid)
        {
            return getJobHistory_DLL(jobid);
        }

        public DataTable GetVoucherDetails_BLL(int companyid, string voucherNo)
        {
            return GetVoucherDetails_DLL(companyid, voucherNo);
        }
        public VoucherDetail GetVoucherDetailVM_BLL(int companyid, string voucherNo)
        {
            return GetVoucherDetailVM_DLL(companyid, voucherNo);
        }
    }
}

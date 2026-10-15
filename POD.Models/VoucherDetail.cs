using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POD.Models
{
    public class VoucherDetail
    {
        public int? jobid { get; set; }
        public string voucherno { get; set; }
        public string drivercode { get; set; }
        public int? deliverystatus { get; set; }
        public int actioncode { get; set; }
        public DateTime? actiontime { get; set; }
        public DateTime? scheduletime { get; set; }
        public string rejectremark { get; set; }
        public string signature { get; set; }
        public string collectedby { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public int? rrid { get; set; }
        public string approvername { get; set; }


        public string empfirstname { get; set; }
        public string emplastname { get; set; }


        public string branchname { get; set; }
        public string brchaddress1 { get; set; }
        public string brchaddress2 { get; set; }
        public string brchaddress3 { get; set; }
        public string brchaddress4 { get; set; }
        public string brchaddress5 { get; set; }
        public string brchaddress6 { get; set; }
        public string brchphoneno { get; set; }
        public string brchlatitude { get; set; }
        public string brchlongitude { get; set; }
        //Customer Address
        public string cuaddaddresstype { get; set; }
        public string cuaddcontactpersonname { get; set; }
        public string cuaddcontactpersonmobile { get; set; }
        public string cuaddaddress { get; set; }
        public string cuaddaddress1 { get; set; }
        public string cuaddaddress2 { get; set; }
        public string cuaddaddress3 { get; set; }
        public string cuaddaddress4 { get; set; }
        public string cuaddaddress5 { get; set; }
        //--cadd.cityid,
        //--cadd.stateid,
        //--cadd.countryid,
        //--cadd.areaid,
        public string cuaddpincode { get; set; }
        public string cuaddlatitude { get; set; }
        public string cuaddlongitude { get; set; }


        public int companyid { get; set; }
        public string companycode { get; set; }
        public string companyname { get; set; }
        public string comapanyaddress { get; set; }
        public string image { get; set; }
    }
}

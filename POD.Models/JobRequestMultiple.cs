using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POD.Models
{
    public class JobRequestMultiple
    {
        public List<JobRequestModel> JobRequestModel { get; set; }
        public int? jobid { get; set; } = 0;
        public int? jobactionid { get; set; } = 0;

    }

    public class JobRequestModel
    {
        public int salesmanid { get; set; }
        public string vhid { get; set; }//
        public long tagno { get; set; }
        public int jobid { get; set; }//
        public int status { get; set; }//
        public int seriesno { get; set; }
        public int jobactionid { get; set; }
        public int jaid { get; set; }//
        public int jhstatusid { get; set; }
        public int scityid { get; set; }
        public int sstateid { get; set; }
        public int shippcountryid { get; set; }
        public int cityid { get; set; }
        public int countryid { get; set; }
        public int areaid { get; set; }
        public int priorityid { get; set; }
        public int stateid { get; set; }
        public string voucherno { get; set; }
        public string voucherseriescode { get; set; }
        public string voucherstatus { get; set; }
        public DateTime voucherdate { get; set; }
        public string orderno { get; set; }
        public DateTime orderdate { get; set; }
        public string seriescode { get; set; }
        public string customercode { get; set; }
        public string firmname { get; set; }
        public string parentcustomercode { get; set; }
        public string custcontperson { get; set; }
        public string custemail { get; set; }
        public string custcontactno { get; set; }
        public decimal grossamount { get; set; }
        public decimal saleamount { get; set; }
        public decimal discount { get; set; }
        public decimal tax { get; set; }
        public string batchno { get; set; }
        public string salesmanname { get; set; }

        public string priorityname { get; set; }
        public string area { get; set; }
        public string deliverytypecode { get; set; }
        public double totalitem { get; set; }
        public double totalqty { get; set; }
        public DateTime syncdatetime { get; set; }
        public string drivercode { get; set; }
        public string drivername { get; set; }
        public string customeraddress { get; set; }
        public string country { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public string pincode { get; set; }
        public string address { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string address3 { get; set; }
        public string address4 { get; set; }
        public string address5 { get; set; }
        public string scountry { get; set; }
        public string sstate { get; set; }
        public string scity { get; set; }
        public string spincode { get; set; }
        public string gstno { get; set; }
        public string tinno { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        //new
        public int br_compid { get; set; }
        public int br_cityid { get; set; }
        public int br_stateid { get; set; }
        public int br_countryid { get; set; }
        public int br_zoneid { get; set; }
        public string scontact { get; set; }
        public string smobile { get; set; }
        public string jhstatus { get; set; }
        public string jhstatuscolor { get; set; }
        public string jobdriver { get; set; }
        public string jobactionname { get; set; }
        public DateTime actiontime { get; set; }
        public DateTime jobscheduletime { get; set; }
        public string jobrejectremark { get; set; }
        public string signature { get; set; }
        public string collectedby { get; set; }
        public int rrid { get; set; }
        public string approvername { get; set; }
        public int br_id { get; set; }
        public string br_code { get; set; }
        public string br_name { get; set; }
        public string br_latitude { get; set; }
        public string br_longitude { get; set; }
        public string br_add1 { get; set; }
        public string br_add2 { get; set; }
        public string br_add3 { get; set; }
        public string br_add4 { get; set; }
        public string br_add5 { get; set; }
        public string br_add6 { get; set; }
        public string br_phone { get; set; }
        public string br_country { get; set; }
        public string br_state { get; set; }
        public string br_city { get; set; }
        public string br_zone { get; set; }
        public int jobactioncount { get; set; }
        public string shplatitude { get; set; }
        public string shplongitude { get; set; }
        public string shippingaddid { get; set; }
        public int deliveryaddrtype { get; set; }


        public string rectype { get; set; }
        public int? loadid { get; set; }
        public string loadno { get; set; }
        public int? companyid { get; set; }

        public string comapanyimage { get; set; }

        public string empfirstname { get; set; }
        public string emplastname { get; set; }
        public string vehiclenumber { get; set; }
        public DateTime? loaddate { get; set; }






        #region Voucher Details


        public int? deliverystatus { get; set; }


        public int actioncode { get; set; }
        public DateTime? scheduletime { get; set; }
        public string rejectremark { get; set; }


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
        public string companycode { get; set; }
        public string companyname { get; set; }
        public string comapanyaddress { get; set; }
        public string image { get; set; }
        #endregion Voucher Details

    }
}

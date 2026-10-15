using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POD.Models
{
   public class UserData
    {
        public int userid{ get; set; }
        public string empcode{ get; set; }
        public string empname{ get; set; }
        public int companyid{ get; set; }
        public string emailid { get; set; }
        public string  contactno{ get; set; }
        public string loginid{ get; set; }
        public string password{ get; set; }
        public int roleid{ get; set; }
        public string rolename{ get; set; }
        public string usertype{ get; set; }
        public string erploginid { get; set; }
        public string eroppassword{ get; set; }
        public int attempt{ get; set; }
        public string mobileos{ get; set; }
        public string macid{ get; set; }
        public string appversion{ get; set; }
        public string tokennumber{ get; set; }
        public int vendorid{ get; set; }
        public string firstname{ get; set; }
        public string lastname{ get; set; }
        public DateTime dob{ get; set; }
        public string gender{ get; set; }
        public string department{ get; set; }
        public int reportingmanager{ get; set; }
        public string alternateno{ get; set; }
        public string marriagestatus{ get; set; }
        public string spousename{ get; set; }
        public DateTime spousedob{ get; set; }
        public DateTime anniversarydate{ get; set; }
        public string localaddress{ get; set; }
        public int lcityid{ get; set; }
        public string lcity { get; set; }
        public int lstateid{ get; set; }
        public string lstate { get; set; }
        public int lzoneid{ get; set; }
        public string lzone { get; set; }
        public int lcountryid{ get; set; }
        public string lcountry { get; set; }
        public string permanentaddress{ get; set; }
        public int pcityid{ get; set; }
        public string pcity { get; set; }
        public int pstateid{ get; set; }
        public string pstate { get; set; }
        public int pzoneid{ get; set; }
        public string pzone { get; set; }
        public int pcountryid{ get; set; }
        public string pcountry { get; set; }

        public string ppincode { get; set; }
        public string platitude { get; set; }
        public string plongitude { get; set; }
        public string paddresstype { get; set; }
        public string phouseno { get; set; }
        public string plandmark { get; set; }

        public string prooftype{ get; set; }
        public string proofid{ get; set; }
        public string proofattachment{ get; set; }
        public int paymentTermsid{ get; set; }
        public string employeetype{ get; set; }
        public int branchid{ get; set; }
        public string branchcode { get; set; }
        public string pic{ get; set; }
        public string about { get; set; }
        public string jwttoken { get; set; }
    }

    public class IdProof
    {
        public int idproofid { get; set; }
        public int userid { get; set; }
        public string empcode { get; set; }
        public string doctype { get; set; }
        public string docfile { get; set; }
        public string docno { get; set; }
        public string expirydate { get; set; }
    }
}



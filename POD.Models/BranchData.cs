using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POD.Models
{
    public class BranchData
    {
        public int branchid { get; set; }
        public string branchcode { get; set; }
        public string branchname { get; set; }
        public int companyid { get; set; }
        public int cityid { get; set; }
        public string city { get; set; }
        public int zoneid { get; set; }
        public string zone { get; set; }
        public int stateid { get; set; }
        public string state { get; set; }
        public int countryid { get; set; }
        public string country { get; set; }
        public string address { get; set; }
        public string phoneno { get; set; }
        public string companycode { get; set; }
        public string companyname { get; set; }
    }
}

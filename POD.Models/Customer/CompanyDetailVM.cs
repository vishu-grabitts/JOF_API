using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POD.Models.Customer
{
    public class CompanyDetailVM
    {
         public int companyid { get; set; }
         public string   companycode     { get; set; }
         public string   companyname     { get; set; }
         public string   companyseries   { get; set; }
         public string   companyempname         { get; set; }
         public string   companyemailid         { get; set; }
         public string   companycontactno       { get; set; }
         public string   companypic             { get; set; }
         public string   companycountry         { get; set; }
         public string   companystate           { get; set; }
         public string   companycity            { get; set; }
         public string   companypincode         { get; set; }
         public string   companylatitude        { get; set; }
         public string companylongitude { get; set; }
         public string comapanyaddress { get; set; }

        public List<CompanySettingVM> companysettingvm { get; set; }
        
        public CompanyDetailVM()
        {
            companysettingvm = new List<CompanySettingVM>();
        }
    }
}

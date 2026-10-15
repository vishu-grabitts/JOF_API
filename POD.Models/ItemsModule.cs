using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POD.Models
{
    public class ItemsModule
    { 
        public long vlid { get; set; }
        public string voucherno { get; set; }
        public string itemcode { get; set; }
        public string itemname { get; set; }
        public string itemtype { get; set; }
        public string itemuom { get; set; }
        public decimal orderqty { get; set; }
        public decimal voucherqty { get; set; }
        public decimal price { get; set; }
        public decimal grossamount { get; set; }
        public decimal discount { get; set; }
        public decimal tax { get; set; }
        public decimal finalamount { get; set; }
        public DateTime syncdatetime { get; set; }

        public decimal rejectedqty { get; set; }
        public string rejectedreason { get; set; }
        public string rejectedremark { get; set; }


        public string lotno { get; set; }
        public DateTime? expirydate { get; set; }
        public string orderno { get; set; }
        public int? companyid { get; set; }
        
        public int? loadid { get; set; }
        public int? handlingunitid { get; set; }
        public string handlingunittitle { get; set; }
        public string size { get; set; }
        public string color { get; set; }

    }

    public class JobDeliveryLines
    {
        public long jdlid { get; set; }
        public int jobid { get; set; }
        public string voucherno { get; set; }
        public string itemcode { get; set; }
        public string itemname { get; set; }
        public string itemtype { get; set; }
        public string itemuom { get; set; }
        public decimal orderqty { get; set; }
        public decimal voucherqty { get; set; }
        public decimal price { get; set; }
        public decimal grossamount { get; set; }
        public decimal discount { get; set; }
        public decimal tax { get; set; }
        public decimal finalamount { get; set; }
        public DateTime syncdatetime { get; set; }
    }
}

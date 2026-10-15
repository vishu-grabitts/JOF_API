using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POD.Models
{
    public class VoucherLineModel
    {
        public string Voucherno { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public decimal? OrderQty { get; set; }

        public decimal? VoucherQty { get; set; }

        public decimal? Price { get; set; }

        public decimal? GrossAmount { get; set; }

        public decimal? Discount { get; set; }

        public decimal? Tax { get; set; }

        public decimal? FinalAmount { get; set; }
    }
}
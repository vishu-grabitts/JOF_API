using System;
using System.Collections.Generic;

namespace POD.Models
{
    public class VoucherHeaderModel
    {
        public string Vhid { get; set; }

        public string BranchCode { get; set; }

        public string Voucherno { get; set; }

        public string VoucherSeriesCode { get; set; }

        public DateTime? VoucherDate { get; set; }

        public int? Status { get; set; }

        public string OrderNo { get; set; }

        public DateTime? OrderDate { get; set; }

        public int? SeriesCode { get; set; }

        public int? SeriesNo { get; set; }

        public string CustomerCode { get; set; }

        public decimal? GrossAmount { get; set; }

        public decimal? SaleAmount { get; set; }

        public decimal? Discount { get; set; }

        public decimal? Tax { get; set; }

        // CHILD RECORDS
        public List<VoucherLineModel> VoucherLines { get; set; }
    }
}
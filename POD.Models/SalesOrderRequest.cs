using System;
using System.Collections.Generic;

namespace POD.Models
{
    public class SalesOrderRequest
    {
        public string VoucherNo { get; set; }

        public DateTime VoucherDate { get; set; }

        public string CustomerCode { get; set; }
        public string PriorityId { get; set; }

        public string DeliveryTypeCode { get; set; }

        public int CompanyId { get; set; }

        public string BranchCode { get; set; }
        public string VoucherSeriesCode { get; set; }
        public string OrderNo { get; set; }
        public DateTime? OrderDate { get; set; }

        public int? SeriesCode { get; set; }
        public int? SeriesNo { get; set; }

        public int? AreaId { get; set; }

        public decimal? GrossAmount { get; set; }
        public decimal? SaleAmount { get; set; }
        public decimal? Discount { get; set; }
        public decimal? Tax { get; set; }

        public int? TotalItem { get; set; }
        public int? TotalQty { get; set; }
        public string Status { get; set; }

        public string HandlingUnit { get; set; }
        public int? HandlingUnitId { get; set; }

        public List<VoucherLineRequest> VoucherLines { get; set; }
    }
}
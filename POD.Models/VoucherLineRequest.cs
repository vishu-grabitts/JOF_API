using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POD.Models
{
    public class VoucherLineRequest
    {
        public string VoucherNo { get; set; }

        public string ItemCode { get; set; }

        public string ItemDescription { get; set; }

        public string ItemUom { get; set; }

        public decimal Price { get; set; }

        public string Size { get; set; }

        public string Color { get; set; }

        public int VoucherQty { get; set; }
    }
}

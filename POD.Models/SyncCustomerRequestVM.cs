using Newtonsoft.Json;
using POD.Models.RoutePlan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POD.Models
{
    public class SyncCustomerRequestVM
    {
        [JsonProperty("CompanyId")]
        public int CompanyId { get; set; }

        [JsonProperty("Customers")]
        public List<CustomerDetailVM> Customers { get; set; }
    }
}

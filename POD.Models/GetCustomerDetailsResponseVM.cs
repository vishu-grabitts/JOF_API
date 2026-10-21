using System.Collections.Generic;
using Newtonsoft.Json;

namespace POD.Models.RoutePlan
{
    public class GetCustomerDetailsResponseVM
    {
        [JsonProperty("statusCode")]
        public int StatusCode { get; set; }

        [JsonProperty("statusMessage")]
        public string StatusMessage { get; set; }

        [JsonProperty("responseObject")]
        public List<CustomerDetailVM> ResponseObject { get; set; }
    }
}
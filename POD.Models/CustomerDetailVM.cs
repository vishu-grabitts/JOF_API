using Newtonsoft.Json;

namespace POD.Models.RoutePlan
{
    public class CustomerDetailVM
    {
        [JsonProperty("Customer Code")]
        public string CustomerCode { get; set; }

        [JsonProperty("Customer Name")]
        public string CustomerName { get; set; }

        [JsonProperty("Contact Person Name")]
        public string ContactPersonName { get; set; }

        [JsonProperty("Email")]
        public string Email { get; set; }

        [JsonProperty("ContactNo")]
        public string ContactNo { get; set; }

        [JsonProperty("AddressType")]
        public string AddressType { get; set; }

        [JsonProperty("PinCode")]
        public string PinCode { get; set; }

        [JsonProperty("City")]
        public string City { get; set; }

        [JsonProperty("State")]
        public string State { get; set; }

        [JsonProperty("Country")]
        public string Country { get; set; }

        [JsonProperty("Latitude")]
        public decimal? Latitude { get; set; }

        [JsonProperty("Longitude")]
        public decimal? Longitude { get; set; }

        [JsonProperty("Address")]
        public string Address { get; set; }

        [JsonProperty("CusStatus")]
        public string CusStatus { get; set; }
    }
}
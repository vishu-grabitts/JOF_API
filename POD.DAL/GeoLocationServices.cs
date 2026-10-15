using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace POD.DAL
{
    public class GeoLocationServices 
    {
        public AddressVM GetAddress(string lat, string lng, string searchAddress = null, bool isFindAddress = true)
        {
            try
            {
                var key = ConfigurationManager.AppSettings["GOOGLE_API_KEY"];

                var httpClient = new HttpClient();
                string url = "";

                if (isFindAddress)
                {
                    url = string.Format("https://maps.googleapis.com/maps/api/geocode/json?key={0}&latlng={1},{2}", key, lat, lng);
                }
                else
                {
                    url = string.Format("https://maps.googleapis.com/maps/api/geocode/json?key={0}&address={1}", key, searchAddress);
                }

                var response = httpClient.GetAsync(url).Result;
                if (response != null && response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    var location = JsonConvert.DeserializeObject<GeoLocationServiceVM>(data);
                    if (location != null)
                    {
                        var address = new AddressVM();
                        var result = location.results.Count() > 1 ? location.results[1] : location.results[0];

                        if (result.address_components != null)
                        {
                            var _postal_code = result.address_components.FirstOrDefault(x => x.types.Contains("postal_code"));
                            if (_postal_code != null)
                            {
                                address.zipcode = _postal_code.long_name;
                            }

                            var _locality = result.address_components.FirstOrDefault(x => x.types.Contains("locality"));
                            if (_locality != null)
                            {
                                address.city = _locality.long_name;
                            }

                            var _administrative_area_level_1 = result.address_components.FirstOrDefault(x => x.types.Contains("administrative_area_level_1"));
                            if (_administrative_area_level_1 != null)
                            {
                                address.state = _administrative_area_level_1.long_name;
                            }

                            var _country = result.address_components.FirstOrDefault(x => x.types.Contains("country"));
                            if (_country != null)
                            {
                                address.country = _country.long_name;
                            }


                        }

                        address.address = result.formatted_address;
                        if(result.geometry != null && result.geometry.location != null)
                        {
                            address.lat = result.geometry.location.lat;
                            address.lng = result.geometry.location.lng;
                        }
                        return address;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }
    }

    public class AddressVM
    {
        public string zipcode { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string address { get; set; }


        public string lat { get; set; }
        public string lng { get; set; }
    }

    public class GeoLocationServiceVM
    {
        public List<GeoLocationService_ResultVM> results { get; set; }
        public string status { get; set; }
    }

    public class GeoLocationService_ResultVM
    {
        public GeoLocationService_Address_Components[] address_components { get; set; }
        public string formatted_address { get; set; }
        public Geometry geometry { get; set; }
        public string place_id { get; set; }
        public string[] types { get; set; }
    }

    public class GeoLocationService_Address_Components
    {
        public string long_name { get; set; }
        public string short_name { get; set; }
        public string[] types { get; set; }
    }

    public class Geometry
    {
        public Location location { get; set; }
    }

    public class Location
    {
        public string lat { get; set; }
        public string lng { get; set; }
    }
}

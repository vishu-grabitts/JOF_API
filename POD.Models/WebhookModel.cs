using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POD.Models
{
    public class Webhookmodel
    {
        //public int tenantsId { get; set; }
        public int companyid { get; set; }
       // public string voucherno { get; set; }
        public string eventtype { get; set; }
    }
    public class CommonResponceWebhook
    {
        public string code { get; set; }
        public string message { get; set; }
    }
    public class VoucherJobDetail
    {
        public string voucher { get; set; }
        public int jaid { get; set; }
    }
    public class SalesWebhook
    {
        public string url { get; set; }
        public string method { get; set; }
        public string Code { get; set; }
        public string AuthType { get; set; }
        public string Tocken { get; set; }

    }
    public class WebhookHttpResponse
    {
        public string Url { get; set; }
        public int StatusCode { get; set; }
        public string ReasonPhrase { get; set; }
        public bool IsSuccess { get; set; }
        public string ResponseBody { get; set; }
    }
}

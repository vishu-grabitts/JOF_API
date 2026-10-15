using POD.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace POD.DAL
{
    public class WebhookDLL : BaseClass
    {
        private readonly HttpClient _httpClient;

        public WebhookDLL()
        {
            _httpClient = new HttpClient();
        }
        protected async Task<List<CommonResponceWebhook>> PostWebhook_DLL(Webhookmodel model)
        {
            List<CommonResponceWebhook> responseModel =
                new List<CommonResponceWebhook>();

            try
            {
                List<SalesWebhook> webhookConfigurations =
                    GetSalesWebhook_DLL(model);

                if (webhookConfigurations == null ||
                    webhookConfigurations.Count == 0)
                {
                    responseModel.Add(new CommonResponceWebhook
                    {
                        code = "failed",
                        message = "No webhook configuration found."
                    });

                    return responseModel;
                }

                // SP should return only records which are not successfully sent
                var voucherJobDetails = GetVoucherJobDetails();

                if (voucherJobDetails == null ||
                    voucherJobDetails.Count == 0)
                {
                    responseModel.Add(new CommonResponceWebhook
                    {
                        code = "Success",
                        message = "No pending webhook records found."
                    });

                    return responseModel;
                }

                foreach (var voucherJob in voucherJobDetails)
                {
                    try
                    {
                        string voucherNo = voucherJob.voucher;
                        int jaid = voucherJob.jaid;

                        var payload =
                            GetDeliveryLocationWebhookDetails(
                                voucherNo,
                                jaid);

                        if (payload == null || payload.Count == 0)
                        {
                            responseModel.Add(new CommonResponceWebhook
                            {
                                code = "failed",
                                message =
                                    "Webhook payload not found." +
                                    " VoucherNo: " + voucherNo +
                                    " | Jaid: " + jaid
                            });

                            continue;
                        }

                        string jsonPayload =
                            System.Text.Json.JsonSerializer.Serialize(
                                payload[0]);

                        List<Task<WebhookHttpResponse>> webhookTasks =
                            new List<Task<WebhookHttpResponse>>();

                        List<SalesWebhook> requestConfigurations =
                            new List<SalesWebhook>();

                        foreach (var webhookConfig in webhookConfigurations)
                        {
                            string url = webhookConfig.url;
                            string method =
                                webhookConfig.method?.ToUpperInvariant();

                            string authType = webhookConfig.AuthType;
                            string token = webhookConfig.Tocken;

                            if (string.IsNullOrWhiteSpace(url))
                                continue;

                            if (!url.StartsWith("http://",
                                    StringComparison.OrdinalIgnoreCase) &&
                                !url.StartsWith("https://",
                                    StringComparison.OrdinalIgnoreCase))
                            {
                                continue;
                            }

                            HttpMethod httpMethod;

                            switch (method)
                            {
                                case "GET":
                                    httpMethod = HttpMethod.Get;
                                    break;

                                case "POST":
                                    httpMethod = HttpMethod.Post;
                                    break;

                                case "PUT":
                                    httpMethod = HttpMethod.Put;
                                    break;

                                case "DELETE":
                                    httpMethod = HttpMethod.Delete;
                                    break;

                                case "HEAD":
                                    httpMethod = HttpMethod.Head;
                                    break;

                                case "OPTIONS":
                                    httpMethod = HttpMethod.Options;
                                    break;

                                default:
                                    continue;
                            }

                            webhookTasks.Add(
                                SendWebhookAsync(
                                    httpMethod,
                                    url,
                                    jsonPayload,
                                    authType,
                                    token));

                            requestConfigurations.Add(webhookConfig);
                        }

                        if (webhookTasks.Count == 0)
                        {
                            responseModel.Add(new CommonResponceWebhook
                            {
                                code = "failed",
                                message =
                                    "No valid webhook configuration found." +
                                    " VoucherNo: " + voucherNo +
                                    " | Jaid: " + jaid
                            });

                            continue;
                        }

                        WebhookHttpResponse[] responses =
                            await Task.WhenAll(webhookTasks);

                        bool anyFailed = false;

                        List<string> webhookResponses =
                            new List<string>();

                        for (int i = 0; i < responses.Length; i++)
                        {
                            WebhookHttpResponse response = responses[i];

                            SalesWebhook webhookConfig =
                                requestConfigurations[i];

                            string method =
                                webhookConfig.method?.ToUpperInvariant();

                            // Save request + response
                            SaveWebhookLog(
                                voucherNo,
                                jaid,
                                response.Url,
                                method,
                                jsonPayload,
                                response);

                            Helper.LogException(
                                new Exception(
                                    "Webhook Response" +
                                    " | VoucherNo: " + voucherNo +
                                    " | Jaid: " + jaid +
                                    " | URL: " + response.Url +
                                    " | StatusCode: " +
                                        response.StatusCode +
                                    " | Success: " +
                                        response.IsSuccess +
                                    " | Response: " +
                                        response.ResponseBody));

                            if (!response.IsSuccess)
                                anyFailed = true;

                            webhookResponses.Add(
                                "URL: " + response.Url +
                                " | StatusCode: " +
                                    response.StatusCode +
                                " | Response: " +
                                    response.ResponseBody);
                        }

                        responseModel.Add(
                            new CommonResponceWebhook
                            {
                                code = anyFailed
                                    ? "failed"
                                    : "Success",

                                message =
                                    (anyFailed
                                        ? "At least one webhook call failed."
                                        : "All webhook calls succeeded.") +
                                    " VoucherNo: " + voucherNo +
                                    " | Jaid: " + jaid +
                                    " | " +
                                    string.Join(
                                        " | ",
                                        webhookResponses)
                            });
                    }
                    catch (Exception voucherEx)
                    {
                        Helper.LogException(voucherEx);

                        responseModel.Add(
                            new CommonResponceWebhook
                            {
                                code = "failed",
                                message =
                                    "Voucher processing failed." +
                                    " | VoucherNo: " +
                                    voucherJob.voucher +
                                    " | Jaid: " +
                                    voucherJob.jaid +
                                    " | Error: " +
                                    voucherEx.Message
                            });
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);

                responseModel.Add(
                    new CommonResponceWebhook
                    {
                        code = "failed",
                        message = ex.Message
                    });
            }

            return responseModel;
        }
        private void SaveWebhookLog(string voucherNo,int jaid,string url,string method,string requestJson,WebhookHttpResponse response)
        {
            using (SqlConnection con = new SqlConnection(constring))
            using (SqlCommand cmd = new SqlCommand(
                "SaveWebhookRequestLog", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(
                    "@VoucherNo",
                    SqlDbType.NVarChar,
                    100).Value =
                    (object)voucherNo ?? DBNull.Value;

                cmd.Parameters.Add(
                    "@Jaid",
                    SqlDbType.NVarChar,
                    100).Value =
                    (object)jaid ?? DBNull.Value;

                cmd.Parameters.Add(
                    "@WebhookUrl",
                    SqlDbType.NVarChar,
                    1000).Value =
                    (object)url ?? DBNull.Value;

                cmd.Parameters.Add(
                    "@HttpMethod",
                    SqlDbType.NVarChar,
                    20).Value =
                    (object)method ?? DBNull.Value;

                cmd.Parameters.Add(
                    "@RequestJson",
                    SqlDbType.NVarChar,
                    -1).Value =
                    (object)requestJson ?? DBNull.Value;

                cmd.Parameters.Add(
                    "@StatusCode",
                    SqlDbType.Int).Value =
                    response.StatusCode;

                cmd.Parameters.Add(
                    "@ReasonPhrase",
                    SqlDbType.NVarChar,
                    500).Value =
                    (object)response.ReasonPhrase ?? DBNull.Value;

                cmd.Parameters.Add(
                    "@ResponseBody",
                    SqlDbType.NVarChar,
                    -1).Value =
                    (object)response.ResponseBody ?? DBNull.Value;

                cmd.Parameters.Add(
                    "@IsSuccess",
                    SqlDbType.Bit).Value =
                    response.IsSuccess;

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        private List<VoucherJobDetail> GetVoucherJobDetails()
        {
            List<VoucherJobDetail> result =
                new List<VoucherJobDetail>();

            using (SqlConnection con = new SqlConnection(constring))
            using (SqlCommand cmd = new SqlCommand(
                "GetVoucherJobDetails", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new VoucherJobDetail
                        {
                            voucher = reader["VoucherNo"] == DBNull.Value
                                ? string.Empty
                                : reader["VoucherNo"].ToString(),

                            jaid = reader["Jaid"] == DBNull.Value
                                ? 0
                                : Convert.ToInt32(reader["Jaid"])
                        });
                    }
                }
            }

            return result;
        }

        private async Task<WebhookHttpResponse> SendWebhookAsync( HttpMethod httpMethod,string url,string jsonPayload,string authType,string token)
        {
            try
            {
                using (HttpRequestMessage request =
                       new HttpRequestMessage(httpMethod, url))
                {
                    request.Version = HttpVersion.Version11;

                    request.Content = new StringContent(
                        jsonPayload,
                        Encoding.UTF8,
                        "application/json");

                    if (!string.IsNullOrWhiteSpace(authType) &&
                        !string.IsNullOrWhiteSpace(token))
                    {
                        request.Headers.Authorization =
                            new System.Net.Http.Headers.AuthenticationHeaderValue(
                                authType,
                                token);
                    }

                    using (HttpResponseMessage response =
                           await _httpClient.SendAsync(request))
                    {
                        string responseBody = string.Empty;

                        if (response.Content != null)
                        {
                            responseBody = await response.Content.ReadAsStringAsync();
                        }

                        return new WebhookHttpResponse
                        {
                            Url = url,
                            StatusCode = (int)response.StatusCode,
                            ReasonPhrase = response.ReasonPhrase,
                            IsSuccess = response.IsSuccessStatusCode,
                            ResponseBody = responseBody
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);

                return new WebhookHttpResponse
                {
                    Url = url,
                    StatusCode = 0,
                    IsSuccess = false,
                    ResponseBody =
                        "Error: " + ex.Message +
                        " | Inner Error: " +
                        (ex.InnerException?.Message ?? "None")
                };
            }
        }
        protected List<DeliveryLocationPayloadVM> GetDeliveryLocationWebhookDetails(string voucherno,int jaid)
        {
            List<DeliveryLocationPayloadVM> webhookDetail = new List<DeliveryLocationPayloadVM>();

            using (SqlConnection con = new SqlConnection(constring))
            using (SqlCommand cmd = new SqlCommand("GetDeliveryLocationWebhookDetails", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@VoucherNo", SqlDbType.NVarChar, 100).Value = voucherno ?? string.Empty;
                cmd.Parameters.Add("@Jaid", SqlDbType.Int).Value = jaid ;

                con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    Dictionary<string, DeliveryLocationPayloadVM> payloads =
                        new Dictionary<string, DeliveryLocationPayloadVM>();

                    while (reader.Read())
                    {
                        string voucherNo = reader["VoucherNo"] == DBNull.Value
                            ? null
                            : reader["VoucherNo"].ToString();

                        if (!payloads.ContainsKey(voucherNo))
                        {
                            payloads[voucherNo] = new DeliveryLocationPayloadVM
                            {
                                VoucherNo = voucherNo,

                                VoucherDate = reader["VoucherDate"] == DBNull.Value
                                    ? (DateTime?)null
                                    : Convert.ToDateTime(reader["VoucherDate"]),

                                Name = reader["Name"] == DBNull.Value
                                    ? null
                                    : reader["Name"].ToString(),

                                Stage = reader["Stage"] == DBNull.Value
                                    ? null
                                    : reader["Stage"].ToString(),

                                Signature = reader["Signature"] == DBNull.Value
    ? null
    : GetBase64Image(reader["Signature"].ToString()),

                                ImageFile = reader["ImageFile"] == DBNull.Value
    ? null
    : GetBase64Image(reader["ImageFile"].ToString())

                                //Items = new List<DeliveryLocationItemPayloadVM>()
                            };
                        }

                        //DeliveryLocationItemPayloadVM item =
                        //    new DeliveryLocationItemPayloadVM
                        //    {
                        //        itemcode = reader["itemcode"] == DBNull.Value
                        //            ? null
                        //            : reader["itemcode"].ToString(),

                        //        itemname = reader["itemname"] == DBNull.Value
                        //            ? null
                        //            : reader["itemname"].ToString(),

                        //        itemtype = reader["itemtype"] == DBNull.Value
                        //            ? null
                        //            : reader["itemtype"].ToString(),

                        //        itemuom = reader["itemuom"] == DBNull.Value
                        //            ? null
                        //            : reader["itemuom"].ToString(),

                        //        orderqty = reader["orderqty"] == DBNull.Value
                        //            ? 0
                        //            : Convert.ToDecimal(reader["orderqty"]),

                        //        voucherqty = reader["voucherqty"] == DBNull.Value
                        //            ? 0
                        //            : Convert.ToDecimal(reader["voucherqty"]),

                        //        price = reader["price"] == DBNull.Value
                        //            ? 0
                        //            : Convert.ToDecimal(reader["price"]),

                        //        grossamount = reader["grossamount"] == DBNull.Value
                        //            ? 0
                        //            : Convert.ToDecimal(reader["grossamount"]),

                        //        discount = reader["discount"] == DBNull.Value
                        //            ? 0
                        //            : Convert.ToDecimal(reader["discount"]),

                        //        tax = reader["tax"] == DBNull.Value
                        //            ? 0
                        //            : Convert.ToDecimal(reader["tax"]),

                        //        finalamount = reader["finalamount"] == DBNull.Value
                        //            ? 0
                        //            : Convert.ToDecimal(reader["finalamount"]),

                        //        syncdatetime = reader["syncdatetime"] == DBNull.Value
                        //            ? DateTime.MinValue
                        //            : Convert.ToDateTime(reader["syncdatetime"]),

                        //        rejectedqty = reader["rejectedqty"] == DBNull.Value
                        //            ? 0
                        //            : Convert.ToDecimal(reader["rejectedqty"]),

                        //        rejectedreason = reader["rejectedreason"] == DBNull.Value
                        //            ? null
                        //            : reader["rejectedreason"].ToString(),

                        //        rejectedremark = reader["rejectedremark"] == DBNull.Value
                        //            ? null
                        //            : reader["rejectedremark"].ToString(),

                        //        lotno = reader["lotno"] == DBNull.Value
                        //            ? null
                        //            : reader["lotno"].ToString(),

                        //        expirydate = reader["expirydate"] == DBNull.Value
                        //            ? (DateTime?)null
                        //            : Convert.ToDateTime(reader["expirydate"]),

                        //        orderno = reader["orderno"] == DBNull.Value
                        //            ? null
                        //            : reader["orderno"].ToString(),

                        //        companyid = reader["companyid"] == DBNull.Value
                        //            ? (int?)null
                        //            : Convert.ToInt32(reader["companyid"]),

                        //        loadid = reader["loadid"] == DBNull.Value
                        //            ? (int?)null
                        //            : Convert.ToInt32(reader["loadid"]),

                        //        handlingunitid = reader["handlingunitid"] == DBNull.Value
                        //            ? (int?)null
                        //            : Convert.ToInt32(reader["handlingunitid"]),

                        //        handlingunittitle = reader["handlingunittitle"] == DBNull.Value
                        //            ? null
                        //            : reader["handlingunittitle"].ToString(),

                        //        size = reader["size"] == DBNull.Value
                        //            ? null
                        //            : reader["size"].ToString(),

                        //        color = reader["color"] == DBNull.Value
                        //            ? null
                        //            : reader["color"].ToString()
                        //    };

                        //payloads[voucherNo].Items.Add(item);
                    }

                    webhookDetail = payloads.Values.ToList();
                }
            }

            return webhookDetail;
        }

        protected List<SalesWebhook> GetSalesWebhook_DLL(Webhookmodel model)
        {
            List<SalesWebhook> webhookList = new List<SalesWebhook>();

            using (SqlConnection con = new SqlConnection(constring))
            using (SqlCommand cmd = new SqlCommand("GetSalesWebhook", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@Action", SqlDbType.NVarChar, 100)
                               .Value = (object)model.eventtype ?? DBNull.Value;

                cmd.Parameters.Add("@Comapnyid", SqlDbType.Int)
                               .Value = model.companyid;
                

                con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        SalesWebhook webhook = new SalesWebhook
                        {
                            url = reader["url"] == DBNull.Value
                                ? null
                                : reader["url"].ToString(),

                            method = reader["method"] == DBNull.Value
                                ? null
                                : reader["method"].ToString(),

                            Code = reader["Code"] == DBNull.Value
                                ? null
                                : reader["Code"].ToString(),

                            AuthType = reader["AuthType"] == DBNull.Value
                                ? null
                                : reader["AuthType"].ToString(),

                            Tocken = reader["Tocken"] == DBNull.Value
                                ? null
                                : reader["Tocken"].ToString()
                        };

                        webhookList.Add(webhook);
                    }
                }
            }

            return webhookList;
        }

        private string GetBase64Image(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return string.Empty;

            string folderPath = ConfigurationManager.AppSettings["JobImagePath"];

            if (string.IsNullOrWhiteSpace(folderPath))
                return string.Empty;

            string filePath = Path.Combine(folderPath, fileName);

            if (!File.Exists(filePath))
                return string.Empty;

            byte[] imageBytes = File.ReadAllBytes(filePath);

            string extension = Path.GetExtension(filePath).ToLowerInvariant();

            string contentType;

            switch (extension)
            {
                case ".png":
                    contentType = "image/png";
                    break;

                case ".jpg":
                case ".jpeg":
                    contentType = "image/jpeg";
                    break;

                case ".gif":
                    contentType = "image/gif";
                    break;

                case ".bmp":
                    contentType = "image/bmp";
                    break;

                case ".webp":
                    contentType = "image/webp";
                    break;

                default:
                    contentType = "application/octet-stream";
                    break;
            }

            return "data:" + contentType + ";base64," +
                   Convert.ToBase64String(imageBytes);
        }

    }
}

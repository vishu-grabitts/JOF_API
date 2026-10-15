using POD.DAL;
using POD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace POD.Logics
{
    public class WebhookBLL : WebhookDLL
    {
        public WebhookBLL()
         : base()
        {
        }
        public async Task<List<CommonResponceWebhook>> PostWebhook(Webhookmodel requestModel)
        {
            return await PostWebhook_DLL(requestModel);
        }
    }
}

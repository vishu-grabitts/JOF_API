using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Text;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Collections.Specialized;

namespace POD.Logics
{
    public class AndroidFCMPushNotification
    {
        public AndroidFCMPushNotification()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public string SendNotification(string message, string sGoogleAppId, string sSenderId= "774538595038")
        {
            var value = message;
            WebRequest tRequest;
            tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            tRequest.Method = "post";
            //tRequest.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            tRequest.ContentType = "application/json";
            tRequest.Headers.Add(string.Format("Authorization: key={0}", sGoogleAppId));
            tRequest.Headers.Add(string.Format("Sender: id={0}", sSenderId));


            Byte[] byteArray = Encoding.UTF8.GetBytes(message);
            tRequest.ContentLength = byteArray.Length;

            Stream dataStream = tRequest.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse tResponse = tRequest.GetResponse();
            dataStream = tResponse.GetResponseStream();
            StreamReader tReader = new StreamReader(dataStream);
            String sResponseFromServer = tReader.ReadToEnd();

            tReader.Close();
            dataStream.Close();
            tResponse.Close();
            return sResponseFromServer;
        }
    }
}
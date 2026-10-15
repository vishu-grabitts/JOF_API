using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POD.DAL;
using POD.Logics;
using POD.Models;

namespace POD.Logics
{
    public class NotificationBLL : NotificationDLL
    {
        //getmessagedetailsbyaction
        public bool SendnotificationByAction_BLL(JobRequest objjobreq, Notification objNotification)
        {
            return SendnotificationByActionDLL(objjobreq, objNotification);
        }

        public bool SendEmail(int companyid, string sSubject, string sBody, string sMailTo, string sMailCC, string sMailBCC)
        {
            return base.SendEmail(companyid, sSubject, sBody, sMailTo, sMailCC, sMailBCC);
        }
        public bool SendEmailWithAttachment(int companyid, string sSubject, string sBody, string sMailTo, string sMailCC, string sMailBCC, string attachment = null)
        {
            var res =  base.SendEmail(companyid, sSubject, sBody, sMailTo, sMailCC, sMailBCC, attachment);
            return res;
        }
    } 
}

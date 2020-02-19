using LostAndFound.Models;
using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace LostAndFound.Controllers
{
    public class phoneCallController : Controller
    {
        DbHandle DB = new DbHandle();

        [HttpPost]
        public void MakeCall(string phone, int findId)
        {
            const string accountSid = "ACef949e0b748bc3a584dba2bc18c5bd9a";
            const string authToken = "75a775705f007ce6a41b0fc6468d1ebf";
            TwilioClient.Init(accountSid, authToken);
            string finderPhone = DB.finds.FirstOrDefault(f => f.id == findId).cellphone;
            finderPhone = "+972" + finderPhone.Substring(1);
            phone = "+972" + phone.Substring(1);
            try
            {

                //Pass the messageId as URL parameter

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                                | SecurityProtocolType.Tls11
                                                | SecurityProtocolType.Tls12
                                                | SecurityProtocolType.Ssl3;
                var call = CallResource.Create(
                            method: Twilio.Http.HttpMethod.Get,
                            url: new Uri("http://twimlets.com/forward?PhoneNumber=" + phone),
                            to: new Twilio.Types.PhoneNumber(finderPhone),
                            from: new Twilio.Types.PhoneNumber("+19252392301")
                            );

                //return View("Home/Index");
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
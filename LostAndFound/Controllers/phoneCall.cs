using LostAndFound.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Twilio;
using Twilio.AspNet.Mvc;
using Twilio.Rest.Api.V2010.Account;
using Twilio.TwiML;

namespace LostAndFound.Controllers
{
    public class phoneCallController : Controller
    {
        DbHandle DB = new DbHandle();

        public void MakeCall(string from, int findId)
        {
            const string accountSid = "AC50e5e5806235577d8b66a6a4470962e7";
            const string authToken = "6948f920aa402d892ea3e4aa6ab02d58";
            TwilioClient.Init(accountSid, authToken);
            string finderPhone = DB.finds.FirstOrDefault(f => f.id == findId).cellphone;
            try
            {

                //Pass the messageId as URL parameter.
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                                | SecurityProtocolType.Tls11
                                                | SecurityProtocolType.Tls12
                                                | SecurityProtocolType.Ssl3;
                var call = CallResource.Create(
                            method: Twilio.Http.HttpMethod.Get,
                            url: new Uri("http://twimlets.com/forward?PhoneNumber=" + finderPhone),
                            to: new Twilio.Types.PhoneNumber(from),
                            from: new Twilio.Types.PhoneNumber("+12053460858")
                            );

            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
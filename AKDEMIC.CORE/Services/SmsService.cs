using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Services
{
    public class SmsService : ISmsService
    {
        public async Task<bool> SendSMS(string message, params string[] phoneNumbers)
        {
            try
            {
                var webRequest = (HttpWebRequest)System.Net.WebRequest.Create("https://api.labsmobile.com/json/send");
                var credentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes("akdemic@edu.pe" + ":" + "emrB3xDcpm7QTW6XgYFagEMS7VZ69L2c"));

                webRequest.Method = "POST";
                webRequest.Headers.Add(HttpRequestHeader.CacheControl, "no-cache");
                webRequest.Headers.Add(HttpRequestHeader.Authorization, $"Basic  {credentials}");
                webRequest.Headers.Add(HttpRequestHeader.ContentType, "application/json");

                var model = new MessageViewModel
                {
                    message = message,
                    tpoa = "Sender",
                    recipient = new List<UserMessageViewModel>()
                };

                foreach (var phoneNumber in phoneNumbers)
                {
                    model.recipient.Add(new UserMessageViewModel
                    {
                        msisdn = phoneNumber
                    });
                }

                var parameters = JsonConvert.SerializeObject(model);

                using (var stream = new StreamWriter(webRequest.GetRequestStream()))
                {
                    stream.Write(parameters);
                }

                var response = (HttpWebResponse)(await webRequest.GetResponseAsync());
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #region Models
        public class MessageViewModel
        {
            public string message { get; set; }
            public string tpoa { get; set; }
            public List<UserMessageViewModel> recipient { get; set; }
        }
        public class UserMessageViewModel
        {
            public string msisdn { get; set; }
        }

        #endregion
    }
}

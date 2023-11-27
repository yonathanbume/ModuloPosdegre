using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.VisaNet.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.VisaNet.Templates;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.VisaNet.Implementations
{
    public class VisaNetPaymentRepository : IVisaNetPaymentRepository
    {
        protected readonly AkdemicContext _context;

        public VisaNetPaymentRepository(AkdemicContext context)
        {
            _context = context;
        }

        private string GetIPAddress()
        {
            var result = "";
            var Host = default(IPHostEntry);
            string Hostname = null;
            Hostname = System.Environment.MachineName;
            Host = Dns.GetHostEntry(Hostname);
            foreach (IPAddress IP in Host.AddressList)
            {
                if (IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    result = Convert.ToString(IP);
                }
            }
            return result;
        }

        public async Task<SessionKeyResponseTemplate> GetSecurityToken(string totalAmount, string merchantId, string uriBase, string visaCredentials)
        {
            var serviceResponse = "";

            using (var client = new HttpClient { BaseAddress = new Uri(uriBase) })
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var tokenArray = new UTF8Encoding().GetBytes($"{visaCredentials}");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(tokenArray));

                using (var response = await client.PostAsync("api.security/v1/security", null))
                {
                    serviceResponse = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode) throw new Exception(serviceResponse.StartsWith('"') ? JsonConvert.DeserializeObject<string>(serviceResponse) : serviceResponse);
                }
            }

            using (var client = new HttpClient { BaseAddress = new Uri(uriBase) })
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //TryAddWithoutValidation
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", serviceResponse);

                var serializedEntity = JsonConvert.SerializeObject(new
                {
                    amount = totalAmount,
                    antifraud = new
                    {
                        clientIp = GetIPAddress()
                    },
                    channel = "web",
                    recurrenceMaxAmount = 10000.00M
                });
                var content = new StringContent(serializedEntity, Encoding.UTF8, "application/json");

                using (var response = await client.PostAsync($"api.ecommerce/v2/ecommerce/token/session/{merchantId}", content))
                {
                    var serviceResponse2 = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode) throw new Exception(serviceResponse2.StartsWith('"') ? JsonConvert.DeserializeObject<string>(serviceResponse2) : serviceResponse2);

                    var rrep = JsonConvert.DeserializeObject<SessionKeyResponseTemplate>(serviceResponse2);
                    rrep.MainKey = serviceResponse;
                    return rrep;
                }
            }
        }

        public async Task<VisaApiResponseTemplate> SendVisaBill(string mainToken, string amount, string apiResponseToken, int purchaseNumber, string merchantId, string uriBase)
        {
            using (var client = new HttpClient { BaseAddress = new Uri(uriBase) })
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", mainToken);

                var serializedEntity = JsonConvert.SerializeObject(new
                {
                    captureType = "manual",
                    channel = "web",
                    countable = true,
                    order = new
                    {
                        amount = amount,
                        currency = "PEN",
                        purchaseNumber = purchaseNumber,
                        tokenId = apiResponseToken
                    }
                });
                var content = new StringContent(serializedEntity, Encoding.UTF8, "application/json");

                var visaResponse = new VisaApiResponseTemplate();
                using (var response = await client.PostAsync($"api.authorization/v3/authorization/ecommerce/{merchantId}", content))
                {
                    var serviceResponse = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        visaResponse.Succesful = false;
                    }
                    else
                    {
                        visaResponse.Succesful = true;
                    }
                    visaResponse.Response = serviceResponse;
                }

                return visaResponse;
            }
        }
    }
}

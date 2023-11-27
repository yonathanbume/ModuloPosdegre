using AKDEMIC.CORE.Options;
using Flurl;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Services
{
    public class GoogleReCaptchaService : IGoogleReCaptchaService
    {
        private readonly IOptions<ReCaptchaCredentials> _config;
        private readonly string URL_VERIFY_BASE = "https://www.google.com/recaptcha/api/siteverify";

        public GoogleReCaptchaService(IOptions<ReCaptchaCredentials> config)
        {
            _config = config;
        }

        public class GoogleRecaptchaResponseModel
        {
            [JsonProperty("success")]
            public bool Success { get; set; }
            [JsonProperty("challenge_ts")]
            public string ChallengeTs { get; set; }

            [JsonProperty("apk_package_name")]
            public string ApkPackageName { get; set; }
            [JsonProperty("error-codes")]
            public string[] ErrorCodes { get; set; }

        }

        public async Task<bool> VerifiyToken(string token)
        {
            try
            {
                var uri = $"{URL_VERIFY_BASE}?secret={_config.Value.SecretKey}&response={token}";

                using (var httpClient = new HttpClient())
                {
                    var httpResult = await httpClient.GetAsync(uri);
                    if (httpResult.StatusCode != System.Net.HttpStatusCode.OK)
                        return false;

                    var response = await httpResult.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<GoogleRecaptchaResponseModel>(response);

                    return result.Success;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

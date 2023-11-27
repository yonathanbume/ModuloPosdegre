using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace AKDEMIC.WEBSERVICE.Base.Methods
{
    public class REST : Base
    {
        protected readonly HttpClient _httpClient;

        public REST()
        {
            _httpClient = new HttpClient();
        }

        public REST(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        protected async Task<TOutput> Get<TOutput>(string requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken, Func<StreamReader, TOutput> func)
        {
            var httpResponseMessage = await _httpClient.GetAsync(requestUri, completionOption, cancellationToken);

            httpResponseMessage.EnsureSuccessStatusCode();

            var httpContent = httpResponseMessage.Content;
            var stream = await httpContent.ReadAsStreamAsync();
            TOutput result = default(TOutput);

            using (var reader = new StreamReader(stream))
            {
                result = func(reader);
            }

            return result;
        }

        protected async Task<TOutput> Post<TOutput>(string requestUri, object value, Encoding encoding, CancellationToken cancellationToken, string mediaType, Func<StreamReader, TOutput> func)
        {
            var content = JsonConvert.SerializeObject(value);
            var stringContent = new StringContent(content, encoding, mediaType);
            var httpResponseMessage = await _httpClient.PostAsync(requestUri, stringContent, cancellationToken);

            httpResponseMessage.EnsureSuccessStatusCode();

            var httpContent = httpResponseMessage.Content;
            var stream = await httpContent.ReadAsStreamAsync();
            TOutput result = default(TOutput);

            using (var reader = new StreamReader(stream))
            {
                result = func(reader);
            }

            return result;
        }

        protected async Task<TOutput> GetJson<TOutput>(string requestUri, HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Get(requestUri, completionOption, cancellationToken, (reader) =>
            {
                using (var jsonTextReader = new JsonTextReader(reader))
                {
                    var jsonSerializer = new JsonSerializer();

                    return jsonSerializer.Deserialize<TOutput>(jsonTextReader);
                }
            });
        }

        protected async Task<TOutput> GetXml<TOutput>(string requestUri, HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Get(requestUri, completionOption, cancellationToken, (reader) =>
            {
                var xmlSerializer = new XmlSerializer(typeof(TOutput));

                return (TOutput)xmlSerializer.Deserialize(reader);
            });
        }

        protected async Task<TOutput> PostJson<TOutput>(string requestUri, object value, Encoding encoding = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (encoding == null)
            {
                encoding = Encoding.Default;
            }

            return await Post(requestUri, value, encoding, cancellationToken, "application/json", (reader) =>
            {
                using (var jsonTextReader = new JsonTextReader(reader))
                {
                    var jsonSerializer = new JsonSerializer();

                    return jsonSerializer.Deserialize<TOutput>(jsonTextReader);
                }
            });
        }

        protected async Task<TOutput> PostXml<TOutput>(string requestUri, object value, Encoding encoding = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (encoding == null)
            {
                encoding = Encoding.Default;
            }

            return await Post(requestUri, value, encoding, cancellationToken, "application/xml", (reader) =>
            {
                using (var xmlTextReader = new XmlTextReader(reader))
                {
                    var xmlSerializer = new XmlSerializer(typeof(TOutput));

                    return (TOutput)xmlSerializer.Deserialize(xmlTextReader);
                }
            });
        }
    }
}

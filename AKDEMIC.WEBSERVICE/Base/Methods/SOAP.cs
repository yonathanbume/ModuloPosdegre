using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace AKDEMIC.WEBSERVICE.Base.Methods
{
    public class SOAP : Base
    {
        protected readonly HttpWebRequest _httpWebRequest;

        public SOAP(HttpWebRequest httpWebRequest)
        {
            _httpWebRequest = httpWebRequest;
            _httpWebRequest.Headers.Add(@"SOAP:Action");
            _httpWebRequest.ContentType = "text/xml;charset=\"utf-8\"";
            _httpWebRequest.Accept = "text/xml";
        }

        public SOAP()
        {
            _httpWebRequest = (HttpWebRequest)WebRequest.Create(CORE.Environments.Environment.WebServices.PIDE.Methods.SOAP.WDSL);
        }

        protected async Task<TOutput> Post<TOutput>(string xml, Func<StreamReader, TOutput> func)
        {
            var xmlDocument = new XmlDocument();
            _httpWebRequest.Method = "POST";

            xmlDocument.LoadXml(xml);

            using (var requestStream = await _httpWebRequest.GetRequestStreamAsync())
            {
                xmlDocument.Save(requestStream);
            }

            TOutput result = default(TOutput);

            using (var response = await _httpWebRequest.GetResponseAsync())
            {
                var responseStream = response.GetResponseStream();

                using (var reader = new StreamReader(responseStream))
                {
                    result = func(reader);
                }
            }

            return result;
        }

        protected async Task<TOutput> PostXml<TOutput>(string xml)
        {
            return await Post(xml, (reader) =>
            {
                using (var xmlTextReader = new XmlTextReader(reader))
                {
                    var xmlSerializer = new XmlSerializer(typeof(TOutput));

                    return (TOutput)xmlSerializer.Deserialize(xmlTextReader);
                }
            });
        }

        protected async Task<TOutput> PostXml<TEnvelope, TOutput>(TEnvelope envelope)
        {
            var xml = "";
            var xmlSerializer = new XmlSerializer(envelope.GetType());

            using (var stringWriter = new StringWriter())
            {
                xmlSerializer.Serialize(stringWriter, envelope);
                xml = stringWriter.ToString();
            }

            xml = @"<?xml version='1.0' encoding='utf-16'?><soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:ws='http://ws.wsdni.segdi.gob.pe/'><soapenv:Header/><soapenv:Body><ws:getDatosDni><peticionConsulta><nuDniConsulta>75141180</nuDniConsulta><nuDniUsuario>40529119</nuDniUsuario><nuRucUsuario>20168999926</nuRucUsuario><password>40529119</password></peticionConsulta></ws:getDatosDni></soapenv:Body></soapenv:Envelope>";

            return await Post(xml, (reader) =>
            {
                using (var xmlTextReader = new XmlTextReader(reader))
                {
                    var xmlSerializer2 = new XmlSerializer(typeof(TOutput));

                    return (TOutput)xmlSerializer2.Deserialize(xmlTextReader);
                }
            });
        }
    }
}

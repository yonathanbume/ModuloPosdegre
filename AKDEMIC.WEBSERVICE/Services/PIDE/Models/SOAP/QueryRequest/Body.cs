using System;
using System.Xml.Serialization;

namespace AKDEMIC.WEBSERVICE.Services.PIDE.Models.SOAP.QueryRequest
{
    [Serializable]
    [XmlRoot(ElementName = "soapenv:Body")]
    public class Body
    {
        [XmlElement(ElementName = "ws:getDatosDni")]
        public GetDatosDni GetDatosDni { get; set; }
    }
}

using System;
using System.Xml.Serialization;

namespace AKDEMIC.WEBSERVICE.Services.PIDE.Models.SOAP.QueryResponse
{
    [Serializable]
    [XmlRoot(ElementName = "soapenv:Body")]
    public class Body
    {
        [XmlElement(ElementName = "ns2:getDatosDniResponse")]
        public GetDatosDniResponse GetDatosDniResponse { get; set; }
    }
}

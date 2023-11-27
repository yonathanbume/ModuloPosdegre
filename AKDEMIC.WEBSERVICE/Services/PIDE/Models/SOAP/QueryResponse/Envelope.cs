using System;
using System.Xml.Serialization;

namespace AKDEMIC.WEBSERVICE.Services.PIDE.Models.SOAP.QueryResponse
{
    [Serializable]
    [XmlRoot(ElementName = "soapenv:Envelope", Namespace = "http://ws.wsdni.segdi.gob.pe/")]
    public class Envelope
    {
        [XmlElement(ElementName = "soapenv:Body")]
        public Body Body { get; set; }
    }
}

using System;
using System.Xml.Serialization;

namespace AKDEMIC.WEBSERVICE.Services.PIDE.Models.SOAP.QueryRequest
{
    [Serializable]
    [XmlRoot(ElementName = "soapenv:Envelope", Namespace = "http://ws.wsdni.segdi.gob.pe/")]
    public class Envelope
    {
        [XmlElement(ElementName = "soapenv:Header")]
        public Header Header { get; set; }

        [XmlElement(ElementName = "soapenv:Body")]
        public Body Body { get; set; }
    }
}

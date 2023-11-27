using System;
using System.Xml.Serialization;

namespace AKDEMIC.WEBSERVICE.Services.PIDE.Models.SOAP.QueryResponse
{
    [Serializable]
    [XmlRoot(ElementName = "ns2:getDatosDniResponse", Namespace = "http://ws.wsdni.segdi.gob.pe/")]
    public class GetDatosDniResponse
    {
        [XmlElement(ElementName = "return")]
        public Return Return { get; set; }
    }
}

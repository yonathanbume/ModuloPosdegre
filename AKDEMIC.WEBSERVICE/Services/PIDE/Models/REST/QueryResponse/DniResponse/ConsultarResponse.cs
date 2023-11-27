using System;
using System.Xml.Serialization;

namespace AKDEMIC.WEBSERVICE.Services.PIDE.Models.REST.QueryResponse.DniResponse
{
    [Serializable]
    [XmlRoot(ElementName = "w:consultarResponse", Namespace = "http://ws.reniec.gob.pe/")]
    public class ConsultarResponse
    {
        [XmlElement(ElementName = "return")]
        public Return Return { get; set; }
    }
}

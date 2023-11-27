using System;
using System.Xml.Serialization;

namespace AKDEMIC.WEBSERVICE.Services.PIDE.Models.SOAP.QueryRequest
{
    [Serializable]
    [XmlRoot(ElementName = "peticionConsulta")]
    public class PeticionConsulta
    {
        [XmlElement(ElementName = "nuDniConsulta")]
        public string NuDniConsulta { get; set; }

        [XmlElement(ElementName = "nuDniUsuario")]
        public string NuDniUsuario { get; set; }

        [XmlElement(ElementName = "nuRucUsuario")]
        public string NuRucUsuario { get; set; }

        [XmlElement(ElementName = "password")]
        public string Password { get; set; }
    }
}

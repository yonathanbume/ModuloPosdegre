using System;
using System.Xml.Serialization;

namespace AKDEMIC.WEBSERVICE.Services.PIDE.Models.SOAP.QueryResponse
{
    [Serializable]
    [XmlRoot(ElementName = "return")]
    public class Return
    {
        [XmlElement(ElementName = "coResultado")]
        public string CoResultado { get; set; }

        [XmlElement(ElementName = "deResultado")]
        public string DeResultado { get; set; }

        [XmlElement(ElementName = "datosPersona")]
        public DatosPersona DatosPersona { get; set; }
    }
}

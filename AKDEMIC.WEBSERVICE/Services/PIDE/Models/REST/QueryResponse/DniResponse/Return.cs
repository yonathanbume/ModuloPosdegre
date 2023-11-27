using System;
using System.Xml.Serialization;

namespace AKDEMIC.WEBSERVICE.Services.PIDE.Models.REST.QueryResponse.DniResponse
{
    [Serializable]
    public class Return
    {
        [XmlElement(ElementName = "coResultado")]
        public string CoResultado { get; set; }

        [XmlElement(ElementName = "datosPersona")]
        public DatosPersona DatosPersona { get; set; }

        [XmlElement(ElementName = "deResultado")]
        public string DeResultado { get; set; }
    }
}

using System;
using System.Xml.Serialization;

namespace AKDEMIC.WEBSERVICE.Services.PIDE.Models.REST.QueryResponse.DniResponse
{
    [Serializable]
    public class DatosPersona
    {
        [XmlElement(ElementName = "apPrimer")]
        public string ApPrimer { get; set; }

        [XmlElement(ElementName = "apSegundo")]
        public string ApSegundo { get; set; }

        [XmlElement(ElementName = "direccion")]
        public string Direccion { get; set; }

        [XmlElement(ElementName = "estadoCivil")]
        public string EstadoCivil { get; set; }

        [XmlElement(ElementName = "foto")]
        public string Foto { get; set; }

        [XmlElement(ElementName = "prenombres")]
        public string Prenombres { get; set; }

        [XmlElement(ElementName = "restriccion")]
        public string Restriccion { get; set; }

        [XmlElement(ElementName = "ubigeo")]
        public string Ubigeo { get; set; }
    }
}

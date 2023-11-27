using System;
using System.Xml.Serialization;

namespace AKDEMIC.WEBSERVICE.Services.PIDE.Models.SOAP.QueryRequest
{
    [Serializable]
    [XmlRoot(ElementName = "ws: getDatosDni")]
    public class GetDatosDni
    {
        [XmlElement(ElementName = "peticionConsulta")]
        public PeticionConsulta PeticionConsulta { get; set; }
    }
}

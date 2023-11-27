using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.WEBSERVICE.Services.PIDE.Models.REST.QueryResponse.GradosResponse
{
    public class ConsultaResponse
    {
        public string RequestUri { get; set; }
        public Respuesta Respuesta { get; set; }
        public List<GTPersona> GTPersonas { get; set; }
    }
}

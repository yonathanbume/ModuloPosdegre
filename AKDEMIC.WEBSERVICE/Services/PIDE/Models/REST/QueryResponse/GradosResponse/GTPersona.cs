using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.WEBSERVICE.Services.PIDE.Models.REST.QueryResponse.GradosResponse
{
    public class GTPersona
    {
        public string TipoDocumento     { get; set; }
        public string NroDocumento      { get; set; }
        public string ApellidoPaterno   { get; set; }
        public string ApellidoMaterno   { get; set; }
        public string Nombres           { get; set; }
        public string AbreviaturaTitulo { get; set; }
        public string TituloProfesional { get; set; }
        public string Universidad       { get; set; }
        public string Pais              { get; set; }
        public string TipoInstitucion   { get; set; }
        public string TipoGestion       { get; set; }
        public string FechaEmision      { get; set; }
        public string Resolucion        { get; set; }
        public string FechaResolucion   { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.WEBSERVICE.Services.PIDE.Models.REST.QueryResponse.RucResponse
{
    public class DatosT362Response
    {
        public string DescNumreg   { get; set; } //descripción Oficina RRPP
        public string T362Fecact   { get; set; } //Fecha y Hora de actualización
        public string T362Fecbaj   { get; set; } //Fecha Baja
        public string T362Indice   { get; set; } //Número de índice
        public string T362Nombre   { get; set; } //Nombre de la empresa
        public string T362Numreg   { get; set; } //Número de registro
        public string T362Numruc   { get; set; } //Número de RUC

    }
}

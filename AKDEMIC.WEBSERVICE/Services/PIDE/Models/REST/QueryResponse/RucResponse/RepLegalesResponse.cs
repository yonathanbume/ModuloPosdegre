using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.WEBSERVICE.Services.PIDE.Models.REST.QueryResponse.RucResponse
{
    public class RepLegalesResponse
    {
        public string CodDepar     { get; set; } //Código de departamento de número teléfono
        public string NumOrdSuce   { get; set; } //Número de orden de representación sucesiva
        public string CodCargo     { get; set; } //Código de cargo que ocupe
        public string RsoCargoo    { get; set; } //Cargo
        public string RsoVdesde    { get; set; } //Fecha desde la que ocupa el cargo
        public string RsoDocide    { get; set; } //Tipo de documento de identidad
        public string DescDocide   { get; set; } //Descripción de documento 
        public string RsoNrodoc    { get; set; } //Número de documento
        public string RsoFecact    { get; set; } //Fecha y hora de actualización
        public string RsoFecnac    { get; set; } //Fecha de nacimiento
        public string RsoNombre    { get; set; } //Nombre del representante
        public string RsoNumruc    { get; set; } //Número de RUC

    }
}

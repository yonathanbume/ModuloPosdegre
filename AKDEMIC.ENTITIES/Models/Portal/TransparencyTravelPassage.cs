using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.Portal
{
    public class TransparencyTravelPassage
    {
        public Guid Id { get; set; }
        public DateTime ChargeDate { get; set; }

        //Fuente de Financiamiento
        public string FinancialSource { get; set; }
        //Ruc de la universidad , 11 digitos
        [StringLength(11)]
        public string Ruc { get; set; }
        //Tipo de Viatico   
        public int TravelType { get; set; }
        //Medio de Transporte TRAVELMODALITY constante
        public int TravelModality { get; set; }
        //Año del viaje
        public int Year { get; set; }
        //Mes del viaje , 2 digitos
        [StringLength(2)]
        public string Month { get; set; }
        //Area/Oficina del personal que realiza el viaje
        public string Area { get; set; }
        //Usuario
        public string UserFullName { get; set; }
        //Fecha de Salida
        public DateTime DepartDate { get; set; }
        //Fecha de Retorno
        public DateTime ReturnDate { get; set; }
        //Ruta
        public string Route { get; set; }
        //Area que autorizo el viaje
        public string AreaAuthorization { get; set; }
        //Costo de Pasajes
        public decimal CostPassageAmount { get; set; }
        //Asignacion de Viaticos
        public decimal CostTravelAmount { get; set; }
        //Costo Total
        public decimal TotalCost { get; set; }
        //Resolucion de Autorizacion
        public string ResolutionAuthorization { get; set; }
    }
}

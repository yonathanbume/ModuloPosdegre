using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.Portal
{
    public class TransparencyServiceOrder
    {
        public Guid Id { get; set; }
        public DateTime ChargeDate { get; set; }

        //Tipo de Orden
        public int Type { get; set; }
        //Año de la Orden o Servicio
        public int Year { get; set; }
        //Mes de la Orden o Servicio , 2 digitos
        [StringLength(2)]
        public string Month { get; set; }
        //Ruc de la universidad , 11 digitos
        [StringLength(11)]
        public string Ruc { get; set; }
        //Periodo de la Orden o Servicio
        public string Term { get; set; }
        //Numero de Orden
        public string OrderNumber { get; set; }
        //Numero de SIAF
        public string Siaf { get; set; }
        //Fecha Formato(dd/mm/aaaa)
        public DateTime OrderDate { get; set; }
        //Monto de la Orden o Servicio, utilizar punto decimal
        public decimal OrderAmount { get; set; }
        //Proveeedor de la Orden o Servicio
        public string Supplier { get; set; }
        //Descripcion de la Orden o Servicio
        public string Description { get; set; }
    }
}

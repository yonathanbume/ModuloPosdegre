using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.Portal
{
    public class TransparencyTelephone
    {
        public Guid Id { get; set; }
        public DateTime ChargeDate { get; set; }

        //Ruc de la universidad , 11 digitos
        [StringLength(11)]
        public string Ruc { get; set; }
        //Año de la Orden o Servicio
        public int Year { get; set; }
        //Mes de la Orden o Servicio , 2 digitos
        [StringLength(2)]
        public string Month { get; set; }
        //Tipo de Servicio de Telecomunicacion , 1 Movil , 2 Fijo , 3 Internet
        public int Type { get; set; }
        //Area
        public string Area { get; set; }
        //Asignacion
        public string UserAsigned { get; set; }
        //Proveedor
        public string Supplier { get; set; }
        //Cargo
        public string Charge { get; set; }
        //Monto
        public decimal Amount { get; set; }
    }
}

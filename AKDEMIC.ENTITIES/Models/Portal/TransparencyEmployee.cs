using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.Portal
{
    public class TransparencyEmployee
    {
        public Guid Id { get; set; }
        public DateTime ChargeDate { get; set; }

        //RUC
        [StringLength(11)]
        public string Ruc { get; set; }
        //Año
        public int Year { get; set; }
        //Mes
        public string Month { get; set; }
        //DNI
        [StringLength(8)]
        public string Dni { get; set; }
        //Regimen Laboral
        public string Regime { get; set; }
        //Apellido Paterno
        public string PaternalSurname { get; set; }
        //Apellido Materno
        public string MaternalSurname { get; set; }
        //Apellido Materno
        public string Name { get; set; }
        //Cargo
        public string Charge { get; set; }
        //Dependencia
        public string Dependency { get; set; }
        //Remuneraciones
        public decimal Remuneration { get; set; }
        //Honorarios
        public decimal Honorary { get; set; }
        //Incentivos
        public decimal Incentive { get; set; }
        //Gratificacion
        public decimal Gratification { get; set; }
        //Beneficios
        public decimal Benefit { get; set; }
        //Total
        public decimal Total { get; set; }
        //Observaciones
        public string Observations { get; set; }
    }
}

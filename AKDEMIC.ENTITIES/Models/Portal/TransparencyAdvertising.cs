using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.Portal
{
    public class TransparencyAdvertising
    {
        public Guid Id { get; set; }
        public DateTime ChargeDate { get; set; }

        //Ruc de la universidad , 11 digitos
        [StringLength(11)]
        public string Ruc { get; set; }
        //Fuente de Financiamiento, Constantes
        public string FinancialSource { get; set; }
        //Año
        public int Year { get; set; }
        //Mes
        public string Month { get; set; }
        //Tipo de Gastos 01:Bienes , 02:Servicios , COnstantes
        public string ExpenseType { get; set; }
        //Tipo de Proceso
        public string ProcessType { get; set; }
        //Numero de Contrato
        public string ContractNumber { get; set; }
        //Objeto del Servicio o Bien
        public string Reason { get; set; }
        //Monto valor Referencial
        public decimal Amount { get; set; }
        //Proveedor
        public string Supplier { get; set; }
        //Ruc del Proveeedor
        [StringLength(11)]
        public string SupplierRuc { get; set; }
        //Monto del Contrato
        public decimal ContractAmount { get; set; }
        //Monto de la penalidad
        public decimal PenaltyAmount { get; set; }
        //Monto Final
        public decimal FinalAmount { get; set; }
        //Observaciones
        public string Observation { get; set; }
    }
}

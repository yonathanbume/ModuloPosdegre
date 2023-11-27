using System;

namespace AKDEMIC.ENTITIES.Models.Portal
{
    public class TransparencyAppliedPenalty
    {
        public Guid Id { get; set; }
        public DateTime ChargeDate { get; set; }

        //N° Correlativo
        public string CorrelativeNumber { get; set; }
        //N° de Contrato, Orden de Compra
        public string ContractNumber { get; set; }
        //Nombre de Contratista
        public string ContractorName { get; set; }
        //Razón de la Contratación
        public string ContractReason { get; set; }
        //Registro de SIAF
        public string Siaf { get; set; }
        //Monto de Penalidad Aplicada
        public decimal PenalyAmount { get; set; }
    }
}

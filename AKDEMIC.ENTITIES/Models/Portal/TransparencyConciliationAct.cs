using System;

namespace AKDEMIC.ENTITIES.Models.Portal
{
    public class TransparencyConciliationAct
    {
        public Guid Id { get; set; }
        public DateTime ChargeDate { get; set; }

        //N° Correlativo
        public string CorrelativeNumber { get; set; }
        //Expediente
        public string Record { get; set; }
        //Solicitante
        public string Petitioner { get; set; }
        //Invitado
        public string Guest { get; set; }
        //Centro de Conciliacion
        public string ConciliationCenter { get; set; }
        //Contrato
        public string Contract { get; set; }
        //Materia que se discute
        public string Topic { get; set; }
        //Cuantía
        public decimal ContractAmount { get; set; }
        //Petitorio
        public decimal RequestedAmount { get; set; }
        //Estado Situacional
        public string State { get; set; }
        //Numero de acta de Conciliacion
        public string Number { get; set; }
    }
}

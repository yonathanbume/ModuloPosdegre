using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.Portal
{
    public class TransparencySelectionCommittee
    {
        public Guid Id { get; set; }
        public DateTime ChargeDate { get; set; }

        //N° Correlativo
        public string CorrelativeNumber { get; set; }
        //Año de Emision de Acto Resolutivo
        public int Year { get; set; }
        //Mes de Emision de Acto Resolutivo , 2 digitos
        [StringLength(2)]
        public string Month { get; set; }
        //Area de destino
        public string Area { get; set; }
        //Funcionarios integrantes del Comite
        public string CommitteMembers { get; set; }
        //Numero de Acto Resolutivo
        public string Number { get; set; }
    }
}

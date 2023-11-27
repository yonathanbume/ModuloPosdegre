using System;

namespace AKDEMIC.ENTITIES.Models.Portal
{
    public class TransparencyVisitsRecord
    {
        public Guid Id { get; set; }
        public DateTime ChargeDate { get; set; }

        //Numero
        public int Number { get; set; }
        //Fecha
        public DateTime Date { get; set; }
        //Visitante
        public string Visitor { get; set; }
        //Tipo de Documento
        public string DocumentType { get; set; }
        //Documento
        public string DocumentNumber { get; set; }
        //Entidad
        public string Entity { get; set; }
        //Motivo
        public string Reason { get; set; }
        //Sede
        public string Sede { get; set; }
        //Empleado Publico
        public string PublicEmployee { get; set; }
        //Oficina
        public string Office { get; set; }
        //Lugar de Reunion
        public string MeetingPlace { get; set; }
        //Hora de Ingreso
        public TimeSpan EnterTime { get; set; }
        //Hora de Salida
        public TimeSpan LeaveTime { get; set; }

    }
}

using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class MedicalAppointment : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public string DoctorId { get; set; }
        public string UserId { get; set; } //Paciente
        public DateTime StartTimeMedicalCare { get; set; }
        public DateTime EndTimeMedicalCare { get; set; }
        public DateTime? DateReffered { get; set; }

        [InverseProperty("UserMedicalAppointments")]
        public ApplicationUser User { get; set; }

        [InverseProperty("DoctorMedicalAppointments")]
        public ApplicationUser Doctor { get; set; }
        public bool Attended { get; set; } = false;
        public bool Reffered { get; set; } = false;
        public string Observation { get; set; }

        ///Inasistencia
        public byte? AbsenceJustificationStatus { get; set; }
        public string AbsenceJustificationReason { get; set; }
        ///
        public PsychologicalRecord PsychologicalRecord { get; set; }
        public TopicConsult TopicConsult { get; set; }
        public NutritionalRecord NutritionalRecord { get; set; }
        public ICollection<HistoricalReferredAppointment> HistoricalReferredAppointments { get; set; }
    }
}

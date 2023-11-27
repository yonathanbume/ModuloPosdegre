using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class PsychologicalRecord
    {
        public Guid? Id { get; set; }
        public ICollection<MedicalConsultReason> MedicalConsultReasons { get; set; }
        public ICollection<MedicalPersonalHistory> PersonalHistories { get; set; }
        public ICollection<MedicalObservation> Observations { get; set; }
        public ICollection<MedicalDiagnosticImpression> DiagnosticImpression { get; set; }
        public ICollection<MedicalFamilyHistory> FamilyHistories { get; set; }
        public bool Isrehabilitated { get; set; }
        public DateTime CurrentDate { get; set; }
        public MedicalAppointment MedicalAppointment { get; set; }
        public ICollection<PsychologyTestExam> PsychologyTestExams { get; set; }

    }
}

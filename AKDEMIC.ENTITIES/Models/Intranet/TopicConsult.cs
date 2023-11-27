using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class TopicConsult
    {
        public Guid Id { get; set; }
        public string DoctorId { get; set; }
        public Guid? ClinicHistoryId { get; set; }
        public Guid? MedicalDiagnosisId { get; set; }
        public DateTime CurrentDate { get; set; }
        public DateTime CurrentTime { get; set; }
        public string ConsultReason { get; set; }
        public string SickTime { get; set; }
        public string Appetite { get; set; }
        public string Thirst { get; set; }
        public string Dream { get; set; }
        public string StateOfMind { get; set; }
        public string Urine { get; set; }
        public string Depositions { get; set; }
        public string VitalSigns { get; set; }
        public decimal Temperature { get; set; }
        public decimal BloodPresure { get; set; }
        public decimal HeartRate { get; set; }
        public decimal BreathingFrequency { get; set; }
        public decimal Weight { get; set; }
        public decimal Size { get; set; }
        public decimal IMC { get; set; }
        public string PhysicalExploration { get; set; }
        public string Diagnosis { get; set; }
        public string Treatment { get; set; }
        public string Evacuation { get; set; }
        public DateTime NextAppointmentDate { get; set; }
        public DateTime HistoricalDate { get; set; }
        public MedicalAppointment MedicalAppointment { get; set; }
        public ClinicHistory ClinicHistory { get; set; }
        public ApplicationUser Doctor { get; set; }
        public ICollection<Prescription> Prescriptions { get; set; }
        public MedicalDiagnostic MedicalDiagnosis { get; set; }

    }
}

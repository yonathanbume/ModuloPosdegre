using System;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.InstitutionalWelfareViewModels
{
    public class PsychologicalRecordViewModel
    {        
        public Guid? Id { get; set; }
        public Guid MedicalAppointmet { get; set; }
        public Guid StudentId { get; set; }
        public Guid DoctorId { get; set; }
        public bool IsTestFinished { get; set; }
        public string DoctorView { get; set; } = "false";
        public string FullName { get; set; }
        public string Birthday { get; set; }
        public int Age { get; set; }
        public string Sex { get; set; }
        public string Career { get; set; }
        public string Address { get; set; }
        public string DNI { get; set; }
        public string UserName { get; set; }
        public string ComeFrom { get; set; }
        public string CurrentAddress { get; set; }
        public string FacultyName { get; set; }
        public string Cycle { get; set; }
        public string Phone { get; set; }
        public string listmedicalconsultreason { get; set; }
        public string listmedicalpersonalhistory { get; set; }
        public string listmedicalfamilyhistory { get; set; }
        public string listmedicalobservation { get; set; }
        public string listmedicaldiagnosticimpression { get; set; }
        public bool IsRehabilitaded { get; set; }
        public Guid currenthistoricaldiagnostic { get; set; }


    }

    public class MedicalConsultReasonViewModel
    {
        public Guid? PsychologicalRecordId;
        public string Description;
        public string DateHistorical;
    }
    public class MedicalPersonalHistoryViewModel
    {
        public Guid? PsychologicalRecordId;
        public string Description;
        public string DateHistorical;
    }
    public class MedicalFamilyHistoryViewModel
    {
        public Guid? PsychologicalRecordId;
        public string Description;
        public string DateHistorical;
    }
    public class MedicalObservationViewModel
    {
        public Guid? PsychologicalRecordId;
        public string Description;
        public string DateHistorical;
    }
    public class MedicalDiagnosticImpressionViewModel
    {
        public Guid? PsychologicalRecordId;
        public string Description;
        public string DateHistorical;
    }
}


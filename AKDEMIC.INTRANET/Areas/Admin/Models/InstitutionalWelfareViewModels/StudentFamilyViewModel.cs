using System;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.InstitutionalWelfareViewModels
{
    public class StudentFamilyViewModel
    {
        public Guid StudentId { get; set; }
        public string Name { get; set; }
        public string PaternalName { get; set; }
        public string MaternalName { get; set; }
        public string Birthday { get; set; }
        public string Relationship { get; set; }
        public string CivilStatus { get; set; }
        public string DegreeInstruction { get; set; }
        public string Certificated { get; set; }
        public string Occupation { get; set; }
        public string WorkCenter { get; set; }
        public string Location { get; set; }
        public byte RelationshipInt { get; set; }
        public byte CivilStatusInt { get; set; }
        public byte DegreeInstructionInt { get; set; }
    }

    public class StudentFamilyEditViewModel : StudentFamilyViewModel
    {
        public Guid StudentFamilyId { get; set; }
    }

    public class StudentFamilyHealEditViewModel
    {
        public Guid StudentFamilyId { get; set; }
        public string Name { get; set; }
        public string PaternalName { get; set; }
        public string MaternalName { get; set; }
        public bool IsSick { get; set; }
        public string DiseaseType { get; set; }
        public bool SurgicalIntervention { get; set; }
    }
}

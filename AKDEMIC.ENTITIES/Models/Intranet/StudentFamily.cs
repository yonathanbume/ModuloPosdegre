using System;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class StudentFamily
    {
        public Guid Id { get; set; }
        public Student Student { get; set; }
        public Guid StudentId { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string PaternalName { get; set; }
        public string MaternalName { get; set; }
        public DateTime Birthday { get; set; }
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
        public bool IsSick { get; set; } = false;
        public string DiseaseType { get; set; }
        public bool SurgicalIntervention { get; set; } = false;
        public string Community { get; set; }
        public string Partiality { get; set; }
        public string PopulatedCenter { get; set; }
        public string SecondOccupation { get; set; }
        public bool LivingRuralArea { get; set; }
    }
}

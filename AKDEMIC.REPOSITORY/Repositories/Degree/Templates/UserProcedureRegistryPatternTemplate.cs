using System;

namespace AKDEMIC.REPOSITORY.Repositories.Degree.Templates
{
    public class UserProcedureRegistryPatternTemplate
    {
        public Guid Id { get; set; }
        public string User { get; set; }
        public string DNI { get; set; }
        public string ProcedureName { get; set; }
        public int GenerateId { get; set; }
        public bool StatusCreated { get; set; }
        public int CurrentAcademicYear { get; set; }        
    }
}

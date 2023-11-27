using System;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Templates.TeacherInstruction
{
    public class TeacherInstructionModelBTemplate
    {
        public string TimeFrame { get;  set; }
        public int LevelStudy { get;  set; }
        public bool State { get;  set; }
        public string CountryName { get;  set; }
        public string InstituteName { get;  set; }
        public Guid Id { get;  set; }
    }
}

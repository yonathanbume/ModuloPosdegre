using System;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.Curriculum
{
    public class CurriculumTemplateD
    {
        public Guid Id { get; set; }

        public bool IsActive { get; set; }
        public string Code { get; set; }
        public bool IsNew { get; set; }

        public string Career { get; set; }

        public int Year { get; set; }

        public Guid FacultyId { get; set; }
        public Guid CareerId { get; set; }
        public Guid? AcademicProgramId { get; set; }
        public string CurriculumName { get; set; }

        public string CreationResolution { get; set; }

        public string CreationResolutionDate { get; set; }

        public string AprobeResolution { get; set; }

        public string AprobeResolutionDate { get; set; }
        public byte StudyRegime { get; set; }

        public string ValidityStart { get; set; }

        public string ValidityEnd { get; set; }

        public int ElectiveCredits { get; set; }

        public int ExtracurricularCredits { get; set; }

        public int ProfessionalPracticeHours { get; set; }
        public string AcademicDegreeBachelor { get; set; }
        public string AcademicDegreeProfessionalTitle { get; set; }
        public int RequiredCredits { get; set; }
        public bool HasCompetencies { get; set; }
        public string UniversityAssemblyResolutionFile { get; set; }
        public string UniversityAssemblyResolutionDate { get; set; }
        public string UniversityAssemblyResolution { get; set; }
        public string CurricularDesingFileUrl { get; set; }
    }
}
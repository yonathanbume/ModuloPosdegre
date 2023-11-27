using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class Curriculum : Entity, IKeyNumber, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid? AcademicProgramId { get; set; }
        public Guid CareerId { get; set; }

        [StringLength(255)]
        public string AcademicDegreeBachelor { get; set; }
        [StringLength(255)]
        public string AcademicDegreeProfessionalTitle { get; set; }

        [StringLength(255)]
        public string Code { get; set; }
        public int ElectiveCredits { get; set; } = 0;
        public int ExtracurricularCredits { get; set; } = 0;
        public int RequiredCredits { get; set; } = 0;
        //public string GeneralSkills { get; set; }
        public bool IsActive { get; set; } = false;
        public bool IsNew { get; set; } = true;
        public int ProfessionalPracticeHours { get; set; } = 0;
        public byte RegimeType { get; set; } = ConstantHelpers.CURRICULUM.REGIME_TYPE.UNSPECIFIED;


        //public string SpecificSkills { get; set; }
        public byte StudyRegime { get; set; }
        public DateTime? ValidityEnd { get; set; }
        public DateTime ValidityStart { get; set; }
        public int Year { get; set; }

        [NotMapped]
        public string Name => $"{Year}-{Code}";

        //Resoluciones

        //Resolución de aprobación / Resolución decanal
        public DateTime? ApprovedResolutionDate { get; set; }
        public string ApprovedResolutionFile { get; set; }

        [StringLength(50)]
        public string ApprovedResolutionNumber { get; set; }

        //Resolución de creación / Resolución rectoral
        public DateTime? CreationResolutionDate { get; set; }
        public string CreationResolutionFile { get; set; }

        [StringLength(50)]
        public string CreationResolutionNumber { get; set; }

        //Resolución de asamblea universitaria
        public string UniversityAssemblyResolutionFile { get; set; }
        public DateTime? UniversityAssemblyResolutionDate { get; set; }
        [StringLength(50)]
        public string UniversityAssemblyResolution { get; set; }

        //Diseño Curricular
        public string CurricularDesignFile { get; set; }

        public AcademicProgram AcademicProgram { get; set; }
        public Career Career { get; set; }

        public ICollection<AcademicYearCourse> AcademicYearCourses { get; set; }
        public ICollection<CurriculumCompetencie> CurriculumCompetencies { get; set; }
    }
}
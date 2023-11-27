using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AKDEMIC.CORE.Helpers;

namespace AKDEMIC.ENTITIES.Models.Generals
{
    public class AcademicProgram : Entity, IKeyNumber, ITrackNumber
    {
        public Guid Id { get; set; }
        public Guid CareerId { get; set; }

        [StringLength(50)]
        public string Code { get; set; }

        public string SuneduCode { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsProgram { get; set; }

        public byte Type { get; set; } = ConstantHelpers.AcademicProgram.Type.UNDEFINED;

        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public DateTime? DecanalResolutionDate { get; set; }
        public string DecanalResolutionFile { get; set; }
        public string DecanalResolutionNumber { get; set; }
        public DateTime? RectoralResolutionDate { get; set; }
        public string RectoralResolutionFile { get; set; }
        public string RectoralResolutionNumber { get; set; }

        public Career Career { get; set; }

        public ICollection<Specialty> Specialties { get; set; }
        public ICollection<Student> Students { get; set; }
        public ICollection<Course> Courses { get; set; }
        public ICollection<AcademicProgramCurriculum> AcademicProgramCurriculums { get; set; }
    }
}
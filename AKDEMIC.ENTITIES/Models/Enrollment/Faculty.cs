using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Intranet;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class Faculty : Entity, IKeyNumber, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }

        [StringLength(10)]
        public string Abbreviation { get; set; }

        [StringLength(50)]
        public string Code { get; set; }
        public string SuneduCode { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsValid { get; set; } = true;

        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public DateTime? DecanalResolutionDate { get; set; }
        public string DecanalResolutionFile { get; set; }

        [StringLength(50)]
        public string DecanalResolutionNumber { get; set; }
        public DateTime? RectoralResolutionDate { get; set; }
        public string RectoralResolutionFile { get; set; }

        [StringLength(50)]
        public string RectoralResolutionNumber { get; set; }

        /// decano y secretario
        public string DeanId { get; set; }
        public string SecretaryId { get; set; }
        public string AdministrativeAssistantId { get; set; }
        public string DeanGrade { get; set; }
        public string DeanResolution { get; set; }
        public string DeanResolutionFile { get; set; }
        public ApplicationUser Dean { get; set; }
        public ApplicationUser Secretary { get; set; }
        public ApplicationUser AdministrativeAssistant { get; set; }

        public ICollection<DeanFaculty> FacultyHistories { get; set; }
        public ICollection<Career> Careers { get; set; }
        public ICollection<FacultyCurriculumArea> FacultyCurriculumAreas { get; set; }
        public ICollection<Procedure> Procedures { get; set; }
        public ICollection<Classroom> Classrooms { get; set; }
    }
}

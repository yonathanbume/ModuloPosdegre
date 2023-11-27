using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Generals
{
    public class Specialty : Entity, ITimestamp , ITrackNumber
    {
        public Guid Id { get; set; }
        public Guid AcademicProgramId { get; set; }

        [StringLength(50)]
        public string Code { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public DateTime? DecanalResolutionDate { get; set; }
        public string DecanalResolutionFile { get; set; }
        public string DecanalResolutionNumber { get; set; }

        public DateTime? RectoralResolutionDate { get; set; }
        public string RectoralResolutionFile { get; set; }
        public string RectoralResolutionNumber { get; set; }

        public AcademicProgram AcademicProgram { get; set; }

        public ICollection<Student> Students { get; set; }
    }
}

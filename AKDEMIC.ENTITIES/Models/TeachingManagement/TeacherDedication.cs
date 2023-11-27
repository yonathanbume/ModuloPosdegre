using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.TeachingManagement
{
    public class TeacherDedication : Entity, IKeyNumber
    {
        public Guid Id { get; set; }
        public Guid WorkerLaborRegimeId { get; set; }

        public decimal MaxLessonHours { get; set; }
        public decimal MaxNoLessonHours { get; set; }
        public decimal MinLessonHours { get; set; }
        public decimal MinNoLessonHours { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        public int Status { get; set; } = 1;

        public WorkerLaborRegime WorkerLaborRegime { get; set; }

        public ICollection<Teacher> Teachers { get; set; }
    }
}
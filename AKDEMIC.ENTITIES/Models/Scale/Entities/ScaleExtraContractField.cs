using System;
using System.ComponentModel.DataAnnotations;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.TeachingManagement;

namespace AKDEMIC.ENTITIES.Models.Scale.Entities
{
    public class ScaleExtraContractField : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }

        [StringLength(100)]
        public string LaborPositionType { get; set; }

        [StringLength(100)]
        public string LaborPosition { get; set; }

        public Guid? WorkerLaborCategoryId { get; set; }
        public WorkerLaborCategory WorkerLaborCategory { get; set; }

        public Guid? WorkerLaborConditionId { get; set; }
        public WorkerLaborCondition WorkerLaborCondition { get; set; }

        public Guid? DependencyId { get; set; }

        //Actividad Academica o Labor Docente
        public bool IsAcademicActivity { get; set; }
        public string Functions { get; set; }
        public string Investigations { get; set; }

        public Guid TeacherDedicationId { get; set; }
        public TeacherDedication TeacherDedication { get; set; }

        [Required]
        public Guid ScaleResolutionId { get; set; }
        public ScaleResolution ScaleResolution { get; set; }
    }
}

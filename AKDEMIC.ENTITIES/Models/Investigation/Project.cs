using System;
using System.Collections.Generic;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.ENTITIES.Models.Investigation
{
    public class Project : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid ResearchLineId { get; set; }
        public ResearchLine ResearchLine { get; set; }
        public Guid CareerId { get; set; }
        public Career Career { get; set; }
        public bool IsFormative { get; set; }
        public Guid? SectionId { get; set; }
        public string Resolution { get; set; }
        public Section Section { get; set; }
        public string CoordinatorId { get; set; }
        public ApplicationUser Coordinator { get; set; }
        public Guid AdvanceRubricId { get; set; }
        public ProjectRubric AdvanceRubric { get; set; }
        public Guid FinalRubricId { get; set; }
        public ProjectRubric FinalRubric { get; set; }
        public Guid AcademicProgramId { get; set; }
        public AcademicProgram AcademicProgram { get; set; }
        public string ExecuteLocation { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Objective { get; set; }
        public decimal Budget { get; set; }
        public byte Status { get; set; }
        public string File { get; set; }
        public bool Sabatical { get; set; }
        public string SabaticalFile { get; set; }
        public string EvaluationFile { get; set; }
        public string DeletedDescription { get; set; }
        public string Code { get; set; }
        public ICollection<ProjectMember> Members { get; set; }
        public ICollection<ProjectAdvance> ProjectAdvances { get; set; }
        public ICollection<ProjectSchedule> ProjectSchedules { get; set; }
        public ICollection<ProjectEvaluator> ProjectEvaluators { get; set; }
    }
}

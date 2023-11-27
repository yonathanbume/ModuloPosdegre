using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.Evaluation
{
    public class Project : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public byte Modality { get; set; }
        public Guid? AreaId { get; set; }
        public EvaluationArea Area { get; set; }
        public Guid DependencyId { get; set; }
        public Dependency Dependency { get; set; }
        public string PublicObjective { get; set; }
        public Guid? CareerId { get; set; }
        public Career Career { get; set; }
        public string CoordinatorId { get; set; }
        public ApplicationUser Coordinator { get; set; }
        public Guid? AcademicProgramId { get; set; }
        public AcademicProgram AcademicProgram { get; set; }
        public Guid ProjectConfigurationId { get; set; }
        public ProjectConfiguration ProjectConfiguration { get; set; }
        public Guid? DistrictId { get; set; }
        public District District { get; set; }
        public string ExecuteLocation { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Objective { get; set; }
        public decimal Budget { get; set; }
        public byte Status { get; set; }
        public string File { get; set; }
        public string Goal { get; set; }
        public int Correlative { get; set; }
        public bool Sabatical { get; set; }
        public string SabaticalFile { get; set; }
        public string ScheduleFile { get; set; }
        public string EvaluationFile { get; set; }
        public ICollection<ProjectSustainableDevelopmentGoal> ProjectSustainableDevelopmentGoals { get; set; }
        public ICollection<ProjectMember> Members { get; set; }
        public ICollection<ProjectEvaluator> ProjectEvaluators { get; set; }
        public ICollection<ProjectAdvance> ProjectAdvances { get; set; }
        public ICollection<ProjectReport> ProjectReports { get; set; }
        public ICollection<ProjectScheduleHistoric> ProjectScheduleHistorics { get; set; }
    }
}

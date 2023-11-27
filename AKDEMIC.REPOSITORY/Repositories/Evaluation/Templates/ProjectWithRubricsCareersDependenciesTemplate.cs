using System;
using System.Collections.Generic;

namespace AKDEMIC.REPOSITORY.Repositories.Evaluation.Templates
{
    public class ProjectWithRubricsCareersDependenciesTemplate
    {
        public Guid Id { get;  set; }
        public string Name { get;  set; }
        public byte Modality { get;  set; }
        public string Area { get;  set; }
        public Guid DependencyId { get;  set; }
        public Guid DistrictId { get; set; }
        public Guid DeparmentId { get; set; }
        public Guid ProvinceId { get; set; }
        public string PublicObjective { get;  set; }
        public Guid? CareerId { get;  set; }
        public Guid? AcademicProgramId { get;  set; }
        public string ExecuteLocation { get;  set; }
        public string StartDateString { get;  set; }
        public string EndDateString { get;  set; }
        public string Objective { get;  set; }
        public decimal Budget { get;  set; }
        public string FileUrl { get;  set; }
        public string CoordinatorId { get;  set; }
        public List<string> TeacherMembers { get;  set; }
        public List<string> StudentMembers { get; set; }
        public List<Tuple<Guid, string>> ListCareers { get;  set; }
        public List<Tuple<Guid, string>> ListDependencies { get;  set; }
        public List<Tuple<string, string>> ListMembers { get; set; }
        public List<string> Goals { get; set; }
        public Guid AdvanceRubricId { get; set; }
        public Guid FinalRubricId { get; set; }
    }
}

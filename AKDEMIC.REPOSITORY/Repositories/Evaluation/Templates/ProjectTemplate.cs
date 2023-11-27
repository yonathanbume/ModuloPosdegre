using System;
using System.Collections.Generic;

namespace AKDEMIC.REPOSITORY.Repositories.Evaluation.Templates
{
    public class ProjectTemplate
    {
        public Guid Id { get; set; }
        public bool Sabatical { get; set; }
        public string Name { get; set; }
        public string Area { get; set; }
        public string Career { get; set; }
        public string Coordinator { get; set; }
        public string AcademicPrograms { get; set; }
        public string ExecuteLocation { get; set; }
        public string District { get; set; }
        public string Province { get; set; }
        public string Department { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Objective { get; set; }
        public string Budget { get; set; }
        public List<string> Members { get; set; }
        public List<string> Evaluators { get; set; }
        public string Dependency { get; set; }
        public string PublicObjective { get; set; }
        public string Modality { get; set; }
    }
    public class ProjectReportDataTemplate
    {
        public string Name { get; set; }
        public string Area { get; set; }
        public string Career { get; set; }
        public string AcademicPrograms { get; set; }
        public string Location { get; set; }
        public string Duration { get; set; }
        public string Objective { get; set; }
        public string Budget { get; set; }
        public string Modality { get; set; }
        public string ExecuterUnity { get; set; }
        public string PublicObjective { get; set; }
        public int Status { get; set; }
    }
}

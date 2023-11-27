using System.Collections.Generic;

namespace AKDEMIC.REPOSITORY.Repositories.Investigation.Templates
{
    public class ProjectTemplate
    {
        public bool Sabatical { get; set; }
        public string Name { get; set; }
        public string ResearchArea { get; set; }
        public string ResearchLine { get; set; }
        public string Career { get; set; }
        public string CoordinatorCode { get; set; }
        public string Coordinator { get; set; }
        public string AcademicProgram { get; set; }
        public string ExecuteLocation { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Objective { get; set; }
        public string Budget { get; set; }
        public List<string> Members { get; set; }
        public List<string> Evaluators { get; set; }
        public List<AdvanceTemplate> AdvanceTemplates { get; set; }
    }
}

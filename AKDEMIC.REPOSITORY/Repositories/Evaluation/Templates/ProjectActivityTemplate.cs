using System;

namespace AKDEMIC.REPOSITORY.Repositories.Evaluation.Templates
{
    public class ProjectActivityTemplate
    {
        public string Name { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Budget { get; set; }
        public string State { get;  set; }
        public Guid Id { get;  set; }
    }
}

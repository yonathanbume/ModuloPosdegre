using System;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Templates.Activity
{
    public sealed class ActivityTemplateA
    {
        public Guid Id { get; set; }
        //public Guid ResolutionId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public decimal MinHours { get; set; }
        public decimal MaxHours { get; set; }


        public string Number { get; set; }

        public string IssueDate { get; set; }

        public string ResolutionPath { get; set; }
    }
}
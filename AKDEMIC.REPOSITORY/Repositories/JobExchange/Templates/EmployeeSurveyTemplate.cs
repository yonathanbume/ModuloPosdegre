using System;

namespace AKDEMIC.REPOSITORY.Repositories.JobExchange.Templates
{
    public class EmployeeSurveyTemplate
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
        public string CreatedAt { get; set; }
        public string Company { get; set; }
        public string Image { get; set; }
        public string Sector { get; set; }
        public string Position { get; set; }
    }
}

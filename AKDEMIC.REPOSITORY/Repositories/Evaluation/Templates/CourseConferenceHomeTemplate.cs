using System;
using System.Collections.Generic;

namespace AKDEMIC.REPOSITORY.Repositories.Evaluation.Templates
{
    public class CourseConferenceHomeTemplate
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Presentation { get; set; }
        public string Content { get; set; }
        public string Requirements { get; set; }
        public string Location { get; set; }
        public string Faculty { get; set; }
        public string Duration { get; set; }
        public byte Modality { get; set; }
        public string Vacancies { get; set; }
        public string StartDate { get; set; }
        public string CertificateDate { get; set; }
        public List<ExhibitorTemplate> Exhibitors { get; set; }
    }

    public class ExhibitorTemplate
    {
        public string Name { get; set; }
        public string Topic { get; set; }
        public string Organization { get; set; }
    }
}

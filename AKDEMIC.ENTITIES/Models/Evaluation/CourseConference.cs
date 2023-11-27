using System;
using System.Collections.Generic;
using AKDEMIC.ENTITIES.Models.Enrollment;

namespace AKDEMIC.ENTITIES.Models.Evaluation
{
    public class CourseConference
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Presentation { get; set; }
        public string Content { get; set; }
        public string Requirements { get; set; }
        public string Location { get; set; }
        public string Image { get; set; }
        public string FinalReportUrl { get; set; }
        public Guid FacultyId { get; set; }
        public Faculty Faculty { get; set; }
        public string Duration { get; set; }
        public byte Modality { get; set; }
        public byte Vacancies { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime CertificateDate { get; set; }
        public ICollection<RegisterCourseConference> Registers { get; set; }
        public ICollection<CourseConferenceExhibitor> Exhibitors { get; set; }
    }
}

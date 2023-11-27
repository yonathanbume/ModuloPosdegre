using System;

namespace AKDEMIC.ENTITIES.Models.Evaluation
{
    public class CourseConferenceExhibitor
    {
        public Guid Id { get; set; }
        public Guid CourseConferenceId { get; set; }
        public CourseConference CourseConference { get; set; }
        public string Responsible { get; set; }
        public string Topic { get; set; }
        public string OriginOrganization { get; set; }
    }
}

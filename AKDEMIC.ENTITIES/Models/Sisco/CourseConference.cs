using System;

namespace AKDEMIC.ENTITIES.Models.Sisco
{
    public class CourseConference
    {
        public Guid Id { get; set; }
        public string Date { get; set; }
        public string Title { get; set; }
        public string Place { get; set; }
        public string Content { get; set; }
        public string Url { get; set; }
    }
}

using System;

namespace AKDEMIC.REPOSITORY.Repositories.Evaluation.Templates
{
    public class CourseConferenceTemplate
    {
        public Guid Id { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Title { get; set; }
        public string Place { get; set; }
        public string Content { get; set; }
        public string Url { get; set; }
    }
}

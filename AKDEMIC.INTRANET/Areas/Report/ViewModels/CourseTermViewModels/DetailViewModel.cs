using System;

namespace AKDEMIC.INTRANET.Areas.Report.ViewModels.CourseTermViewModels
{
    public class DetailViewModel
    {
        public Guid CurriculumId { get; set; }
        public int AcademicYear { get; set; }
        public Guid CourseId { get; set; }

        public string Career { get; set; }
        public string Curriculum { get; set; }

        public Guid TermId { get; set; }
    }
}

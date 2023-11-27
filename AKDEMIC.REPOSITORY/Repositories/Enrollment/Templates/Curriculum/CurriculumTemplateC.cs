using System;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.Curriculum
{
    public class CurriculumTemplateC
    {
        public Guid Id { get; set; }
        public string Term { get; set; }
        public string Career { get; set; }
        public bool IsActive { get; set; }
    }
}

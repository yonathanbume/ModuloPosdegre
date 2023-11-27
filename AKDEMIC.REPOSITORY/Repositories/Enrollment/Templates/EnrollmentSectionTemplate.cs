using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates
{
    public class EnrollmentSectionTemplate
    {
        public Guid SectionId { get; set; }

        public Guid SectionGroupId { get; set; }

        public string Section { get; set; }

        public string Teachers { get; set; }

        public string Schedules { get; set; }

        public int Vacancies { get; set; }

        public bool Intersection { get; set; }

        public bool IsActive { get; set; }
    }
}

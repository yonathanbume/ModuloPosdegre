using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.ClassSchedule
{
    public class ValidateClassScheduleTemplate
    {
        public Guid? Id { get; set; }
        public Guid ClassroomId { get; set; }
        public Guid SectionId { get; set; }
        public TimeSpan StartTimeUTC { get; set; }
        public TimeSpan EndTimeUTC { get; set; }
        public int WeekDay { get; set; }
        public int SessionType { get; set; }
        public Guid? SectionGroupId { get; set; }
        public bool DividedByGroups { get; set; }

        public bool ValidateTeachers { get; set; }
        public List<string> Teachers { get; set; }
    }
}

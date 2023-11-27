using System;
using System.Collections.Generic;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.CourseUnit
{
    public class CourseUnitModelA
    {
        public Guid? Id { get; set; }
        public Guid SylabusId { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public int WeekNumberStart { get; set; }
        public int WeekNumberEnd { get; set; }
        public int AcademicProgressPercentage { get; set; }
        public Guid CourseId { get; set; }
        public IList<CourseUnitModelAUnitActivity> UnitActivities { get; set; }

        public IList<string> Teachers { get; set; }

        public IList<CourseUnitModelAUnitResourse> UnitResources { get; set; }
    }

    public class CourseUnitModelAUnitActivity
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public int Week { get; set; }
        public int Order { get; set; }
        public Guid CourseUnitId { get; set; }
    }

    public class CourseUnitModelAUnitResourse
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public int Week { get; set; }
        public Guid CourseUnitId { get; set; }
    }
}
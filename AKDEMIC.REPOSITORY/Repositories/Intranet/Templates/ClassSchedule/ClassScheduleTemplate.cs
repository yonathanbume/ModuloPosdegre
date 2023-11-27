using System;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.ClassSchedule
{
    public class ClassScheduleTemplate
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public bool AllDay { get; set; }
        public string Description { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
    }
    public class SectionClassSchedulesTemplate
    {
        public Guid Id { get;  set; }
        public string Weekday { get;  set; }
        public string StartTime { get;  set; }
        public string EndTime { get;  set; }
        public string Classroom { get;  set; }
        public int SessionType { get;  set; }
        public double Duration { get;  set; }
        public bool HasAssigned { get; set; }
        public string Teacher { get;  set; }
    }
}

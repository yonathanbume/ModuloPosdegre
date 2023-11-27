using System;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.ClassSchedule
{
    public sealed class ClassScheduleTemplateA
    {
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Code { get; set; }
        public string TimeText { get; set; }
        public int WeekDay { get; set; }
        public Guid SectionId { get; set; }
        public int SessionType { get; set; }
        public string SectionGroup { get; set; }
        public int SectionGroupStudentsCount { get; set; }
    }
}
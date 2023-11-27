using System;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.ClassStudent
{
    public class SectionAbsenceDetailDataTableTemplate
    {
        public Guid ClassId { get;  set; }
        public int Week { get;  set; }
        public int SessionNumber { get;  set; }
        public string Date { get;  set; }
        public string WeekDay { get;  set; }
        public string StartTime { get;  set; }
        public DateTime DStartTime { get;  set; }
        public string EndTime { get;  set; }
        public bool IsAbsent { get;  set; }
    }
}

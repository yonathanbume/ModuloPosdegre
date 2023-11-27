using System;

namespace AKDEMIC.REPOSITORY.Repositories.Reservation.Templates
{
    public class ScheduleTemplate
    {
        public string id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public bool allDay { get; set; }
        public int weekday { get; set; }
        public TimeSpan start { get; set; }
        public TimeSpan end { get; set; }
    }
}

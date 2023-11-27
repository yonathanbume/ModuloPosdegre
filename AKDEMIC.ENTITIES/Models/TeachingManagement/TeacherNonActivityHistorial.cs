using AKDEMIC.ENTITIES.Models.Generals;
using System;

namespace AKDEMIC.ENTITIES.Models.TeachingManagement
{
    public class TeacherNonActivityHistorial
    {
        public Guid Id { get; set; }

        public Guid TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        public Guid NonActivityId { get; set; }
        public NonActivity NonActivity { get; set; }

        public string Commentary { get; set; }

        //public DateTime? Date { get; set; }

        //public TimeSpan? StartTime { get; set; }

        //public TimeSpan? EndTime { get; set; }

        //public string TermId { get; set; }

        //public Term Term { get; set; }

        public decimal TotalHours { get; set; }

        public string FilePath { get; set; }
    }
}

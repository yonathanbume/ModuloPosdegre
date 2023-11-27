using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.Preuniversitary
{
    public class PreuniversitarySchedule
    {
        public Guid Id { get; set; }

        public Guid PreuniversitaryGroupId { get; set; }

        public PreuniversitaryGroup PreuniversitaryGroup { get; set; }

        public byte DayOfWeek { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public Guid ClassroomId { get; set; }

        public Classroom Classroom { get; set; }

        public ICollection<PreuniversitaryAssistance> PreuniversitaryAssistances { get; set; }
    }
}

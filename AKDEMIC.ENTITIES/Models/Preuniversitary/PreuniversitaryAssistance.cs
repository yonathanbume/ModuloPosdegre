using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.Preuniversitary
{
    public class PreuniversitaryAssistance
    {
        public Guid Id { get; set; }

        public PreuniversitarySchedule PreuniversitarySchedule { get; set; }

        public Guid PreuniversitaryScheduleId { get; set; }

        public DateTime DateTime { get; set; }

        public Guid PreuniversitaryTemaryId { get; set; }

        public PreuniversitaryTemary PreuniversitaryTemary { get; set; }

        public ICollection<PreuniversitaryAssistanceStudent> PreuniversitaryAssistanceStudents { get; set; }
    }
}

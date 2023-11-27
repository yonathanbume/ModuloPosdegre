using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class ExamWeek : Entity, ITimestamp 
    {
        [Key]
        public Guid TermId { get; set; }
        [Key]
        public byte Type { get; set; }
        public int Week { get; set; }
        public DateTime WeekStart { get; set; }
        public DateTime WeekEnd { get; set; }
        public Term Term { get; set; }
    }
}

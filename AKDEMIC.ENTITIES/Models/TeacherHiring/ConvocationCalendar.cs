using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.TeacherHiring
{
    public class ConvocationCalendar
    {
        public Guid Id { get; set; }
        public Guid ConvocationId { get; set; }
        public Convocation Convocation { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Description { get; set; }
    }
}

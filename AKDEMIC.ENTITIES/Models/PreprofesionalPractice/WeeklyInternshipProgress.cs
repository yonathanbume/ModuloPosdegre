using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.PreprofesionalPractice
{
    public class WeeklyInternshipProgress : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid InternshipRequestId { get; set; }
        public int Week { get; set; }
        public bool IsConfirmed { get; set; }
        public DateTime? ConfirmeDateTime { get; set; }

        public InternshipRequest InternshipRequest { get; set; }

        public ICollection<WeeklyInternshipProgressDetail> WeeklyInternshipProgressDetails { get; set; }
    }
}

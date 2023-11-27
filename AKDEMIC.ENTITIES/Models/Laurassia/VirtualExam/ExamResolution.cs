using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;

namespace AKDEMIC.ENTITIES.Models.Laurassia
{
    public class ExamResolution : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid ExamStudentId { get; set; }
        [StringLength(3)]
        public string State { get; set; }//rf: resolucion finalizada/ nr: nueva resolucion
        public TimeSpan Duration { get; set; } 
        public decimal MaxPoints { get; set; } 
        public DateTime ReceivedDate { get; set; }
        public DateTime StartDate { get; set; } 
        public decimal Points { get; set; } 
        public decimal PointsBase20 { get; set; }
        public string IpAddress { get; set; }
        public virtual VExamStudent ExamStudent { get; set; }
        public virtual ICollection<VExamDetail> Details { get; set; }
    }
}

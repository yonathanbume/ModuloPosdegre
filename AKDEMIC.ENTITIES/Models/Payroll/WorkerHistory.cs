using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.Payroll
{
    public class WorkerHistory
    {
        public Guid Id { get; set; }
        
        public Guid WorkerId { get; set; }

        public Worker Worker { get; set; }

        public bool IsFinished { get; set; }

        [Required]
        public DateTime StartDate { get; set; }
        
        public DateTime? EndDate { get; set; }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class PsychologyTestQuestion
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }

        [Required]
        [StringLength(400)]
        public string Question { get; set; }

        public bool State { get; set; } = true;
        public PsychologyCategory Category { get; set; }

        public ICollection<PsychologyTestExam> PsychologyTestExams { get; set; }
    }
}

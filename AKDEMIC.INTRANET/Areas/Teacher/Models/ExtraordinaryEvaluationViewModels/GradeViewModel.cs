using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Teacher.Models.ExtraordinaryEvaluationViewModels
{
    public class GradeViewModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Nota obtenida")]
        public decimal Grade { get; set; }

        [Display(Name = "Observaciones")]
        public string Observations { get; set; }
    }
}

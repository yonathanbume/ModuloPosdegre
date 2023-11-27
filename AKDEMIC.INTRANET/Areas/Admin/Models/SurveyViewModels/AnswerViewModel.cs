using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.SurveyViewModels
{
    public class AnswerViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Descripción")]
        public string Description { get; set; }

    }

    public class AnswerCreateViewModel
    {
        [Required]
        public string Description { get; set; }
    }
}

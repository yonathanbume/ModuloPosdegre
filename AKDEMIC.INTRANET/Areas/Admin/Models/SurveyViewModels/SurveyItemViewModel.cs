using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.SurveyViewModels
{
    public class SurveyItemViewModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Sección")]
        public string Title { get; set; }

        public bool IsLikert { get; set; }
    }
}

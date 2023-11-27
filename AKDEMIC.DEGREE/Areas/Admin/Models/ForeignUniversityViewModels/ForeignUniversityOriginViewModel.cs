using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.DEGREE.Areas.Admin.Models.ForeignUniversityViewModels
{
    public class ForeignUniversityOriginViewModel
    {
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        public string Name { get; set; }
    }
}

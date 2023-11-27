using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.DEGREE.Areas.Admin.Models.HistoricalRegistryPatternViewModels
{
    public class HistoricalRegistryPatternViewModel
    {
        public Guid Id { get; set; }

        [Display(Name ="Nro. Oficio")]
        public string OfficeNumber { get; set; }

        [Required]
        [Display(Name = "Descripción")]
        public string Description { get; set; }

        public IFormFile File { get; set; }
    }
}

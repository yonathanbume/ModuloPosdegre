using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.YearInformationViewModels
{
    public class YearInformationViewModel
    {
        public Guid Id { get; set; }
        [Required]
        public int Year { get; set; }
        [Required]
        public string Name { get; set; }
    }
}

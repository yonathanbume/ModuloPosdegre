using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Academic.ViewModels.StudentInformation
{
    public class AmnestyRequestViewModel
    {
        [Required]
        public string Observation { get; set; }

        public IFormFile File { get; set; }
    }
}

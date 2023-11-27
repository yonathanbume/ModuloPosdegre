using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.INTRANET.Areas.Academic.ViewModels.StudentInformation
{
    public class ReservationRequestViewModel
    {
        [Required]
        public string Receipt { get; set; }

        //[Required]
        public IFormFile File { get; set; }

        public string Observation { get; set; }
    }
    public class ExtemporaneousViewModel
    {
        [Required]
        public string Receipt { get; set; }
    }
}

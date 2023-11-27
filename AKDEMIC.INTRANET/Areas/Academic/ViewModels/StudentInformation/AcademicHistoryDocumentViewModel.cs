using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.INTRANET.Areas.Academic.ViewModels.StudentInformation
{
    public class AcademicHistoryDocumentViewModel
    {
        [Required]
        public IFormFile CPDocument { get; set; }

        public string PhysicalLocation { get; set; }
    }
}

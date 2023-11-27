using Microsoft.AspNetCore.Http;
using System;

namespace AKDEMIC.INTRANET.Areas.Academic.ViewModels.StudentInformation
{
    public class ChangeSectionViewModel
    {
        public Guid StudentSectionId { get; set; }

        public Guid NewSectionId { get; set; }

        public string Resolution { get; set; }

        public IFormFile File { get; set; }


    }
}

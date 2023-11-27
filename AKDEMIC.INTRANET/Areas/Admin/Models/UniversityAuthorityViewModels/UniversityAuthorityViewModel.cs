using Microsoft.AspNetCore.Http;
using System;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.UniversityAuthorityViewModels
{
    public class UniversityAuthorityViewModel
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public byte Type { get; set; }
        public string ResolutionDate { get; set; }

        public FileVM File { get; set; }
    }
    public class FileVM
    {
        public IFormFile File { get; set; }
    }
}

using AKDEMIC.CORE.Overrides;
using AKDEMIC.INTRANET.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.DependencyViewModels
{
    public class DependencyViewModel
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Acronym { get; set; }
        public string Signature { get; set; }

        [DataType(DataType.Upload)]
        [Extensions(ConstantHelpers.DOCUMENTS.FILE_EXTENSION_GROUP.IMAGES)]
        public IFormFile SignatureFile { get; set; }
    }
}

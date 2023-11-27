using AKDEMIC.CORE.Overrides;
using AKDEMIC.DEGREE.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.DEGREE.Areas.Admin.Models.AnnouncementViewModels
{
    public class AnnouncementViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; } 
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public DateTime CreatedDate { get; set; }
        [DataType(DataType.Upload)]
        [Extensions("jpg,jpeg,png,bmp,svg,gif,PNG", ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.FILE_EXTENSIONS)]
        [Display(Name = "Foto")]
        public IFormFile Picture { get; set; }
        public string Pathfile { get; set; }
        [Required(ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [Display(Name = "Roles")]
        public List<string> SelectedRoles { get; set; }

    }
}

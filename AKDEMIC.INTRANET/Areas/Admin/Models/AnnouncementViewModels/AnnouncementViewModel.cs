using AKDEMIC.CORE.Overrides;
using AKDEMIC.INTRANET.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.AnnouncementViewModels
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
    public class BeginningAnnouncementViewModel: ParentAnnouncement
    {
        public byte System { get; set; }       
    }
    public class UserAnnouncementViewModel: ParentAnnouncement
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
    }
    public class ParentAnnouncement
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string YoutubeUrl { get; set; }
        public IFormFile Image { get; set; }
        public string ImageUrl { get; set; }
        public IFormFile File { get; set; }
        public string FileUrl { get; set; }
        public string Description { get; set; }
        public byte AppearsIn { get; set; }
        public byte Type { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public byte ImgOrVid { get; set; }
        public List<string> Roles { get; set; }
    }
}

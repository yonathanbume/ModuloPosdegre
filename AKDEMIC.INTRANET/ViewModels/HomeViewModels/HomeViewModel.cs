using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.INTRANET.Areas.Admin.Models.AnnouncementViewModels;
using AKDEMIC.INTRANET.ViewModels.ProfileViewModels;

namespace AKDEMIC.INTRANET.ViewModels.HomeViewModels
{
    public class HomeViewModel
    {
        public string EmailInstitutional { get; set; }
        public string ActivationPassword { get; set; }
        public ProfileViewModel Profile { get; set; }
        public List<AnnouncementViewModel> Announcements { get; set; }
        public List<BeginningAnnouncement> BeginningAnnouncements { get; set; }
        public UserAnnouncement UserAnnouncement { get; set; }
        public List<AnnouncementManagementViewModel> ManageAnnouncements { get; set; }
    }
    public class UpdateinformationViewModel
    {
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }

    }

    public class AnnouncementManagementViewModel
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        public string Title { get; set; }
        public bool AllRoles { get; set; }
        public bool AllCareers { get; set; }
        public IList<string> Careers { get; set; }
        public IList<string> Roles { get; set; }
        public DateTime DisplayTime { get; set; }
        public bool HasFile { get; set; }
    }
}

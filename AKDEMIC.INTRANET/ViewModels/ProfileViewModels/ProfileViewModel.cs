using AKDEMIC.ENTITIES.Models.Generals;
using System;

namespace AKDEMIC.INTRANET.ViewModels.ProfileViewModels
{
    public class ApplicationUserViewModel
    {
        public bool? FirstTime { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Picture { get; set; }
        public string Dni { get; set; }
        public string Career { get; set; }

        public Guid DistrictId { get; set; }
        public Guid ProvinceId { get; set; }
        public Guid DepartmentId { get; set; }
        public string Address { get; set; }

    }
    public class ProfileViewModel
    {
        public ProfileViewModel(bool? firstTime, string email, string phoneNumber)
        {
            UserViewModel = new ApplicationUserViewModel
            {
                FirstTime = firstTime,
                Email = email,
                PhoneNumber = phoneNumber
            };
        }
        public ProfileViewModel(ApplicationUser user, Student student)
        {
            UserViewModel = new ApplicationUserViewModel
            {
                FirstTime = user.FirstTime,
                Email = user.PersonalEmail,
                PhoneNumber = user.PhoneNumber,
                Career = student?.Career?.Name ?? "",
                Dni = user.Document,
                Picture = user.Picture,
                DepartmentId = user.DepartmentId ?? Guid.Empty,
                ProvinceId = user.ProvinceId ?? Guid.Empty,
                DistrictId = user.DistrictId ?? Guid.Empty,
                Address = user.Address
            };
        }
        public ApplicationUserViewModel UserViewModel { get; set; }
    }
}

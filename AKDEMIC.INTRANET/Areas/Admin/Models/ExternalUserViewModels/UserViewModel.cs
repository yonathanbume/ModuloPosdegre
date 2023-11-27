using System;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.ExternalUserViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string PaternalSurname { get; set; }
        public string MaternalSurname { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int Sex { get; set; }
        public string BirthDate { get; set; }
        public string Document { get; set; }
        public byte DocumentType { get; set; }

        public string Password { get; set; }
        public string PasswordVerifier { get; set; }
    }
}

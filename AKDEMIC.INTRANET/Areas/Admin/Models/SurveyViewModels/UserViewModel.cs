using System;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.SurveyViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public Guid SurveyUserId { get; set; }
        public string Name { get; set; }
        public string PaternalSurname { get; set; }
        public string MaternalSurname { get; set; }
        public string Email { get; set; }        
    }
}

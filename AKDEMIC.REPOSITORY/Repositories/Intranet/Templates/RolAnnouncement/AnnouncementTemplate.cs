using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.RolAnnouncement
{
    public class AnnouncementTemplate
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public IFormFile Picture { get; set; }
        public string Pathfile { get; set; }
        public List<string> SelectedRoles { get; set; }
    }
}

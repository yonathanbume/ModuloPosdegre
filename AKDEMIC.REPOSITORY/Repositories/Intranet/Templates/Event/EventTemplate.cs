using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.Event
{
    public class EventTemplate
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Place { get; set; }
        public string UrlPicture { get; set; }
        public string UrlVideo { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public decimal Cost { get; set; }
        public byte System { get; set; }
        public bool IsPublic { get; set; }
        public bool HasCertification { get; set; }

        public List<EventRoleTemplate> EventRoles { get; set; }
        public List<EventCareerTemplate> EventCareers { get; set; }
    }

    public class EventRoleTemplate
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }

        public string RoleId { get; set; }
        public string RoleName { get; set; }
    }

    public class EventCareerTemplate
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public Guid CareerId { get; set; }
        public string CareerCode { get; set; }
        public string CareerName { get; set; }
    }
}

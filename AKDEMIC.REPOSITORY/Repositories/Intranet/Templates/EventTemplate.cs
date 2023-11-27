using System;
using System.Collections.Generic;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Templates
{
    public class EventDataTableTemplate
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Eventtype { get; set; }
        public string Organizer { get; set; }
        public string Place { get; set; }
        public string Eventdate { get; set; }
        public string RegistrationStartDate { get; set; }
        public string RegistrationEndDate { get; set; }
    }
    public class ReportEventTemplate
    {
        public Guid Id { get; set; }
        public string StartEndDate { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public decimal GeneralCost { get; set; }
        public string EventDate { get; set; }
        public string OrganizerName { get; set; }
        public string Place { get; set; }
    }

    public class UserEventPDFTemplate
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string EventName { get; set; }
        public string Place { get; set; }
        public string EventDate { get; set; }
        public string Image { get; set; }
        public string CertificateTitle { get; set; }
        public string CertificateContent { get; set; }
        public string ReportDate { get; set; }
    }
}

using System;

namespace AKDEMIC.INTRANET.ViewModels.EventViewModels
{
    public class EventViewModel
    {
        public Guid Id { get; set; }
        public Guid EventTypeId { get; set; }
        public string EventTypeName { get; set; }
        public string EventDate { get; set; }

        public string RegistrationStartDate { get; set; }
        public string RegistrationEndDate { get; set; }
        public string Creator { get; set; }

        public string EventTypeColor { get; set; }
        public string PathPicture { get; set; }
        public string EventName { get; set; }
        public string Description { get; set; }
        public bool SignedUp { get; set; }
        public string UrlVideo { get; set; }
    }
}

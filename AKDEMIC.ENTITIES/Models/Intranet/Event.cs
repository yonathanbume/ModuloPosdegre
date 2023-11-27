using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class Event : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }
        public int ImageName { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(800)]
        public string Description{ get; set; }
        public string OrganizerId { get; set; }      
        public Guid EventTypeId { get; set; }
        public Guid? EventCertificationId { get; set; }
        public string PathPicture { get; set; }
        public string Place { get; set; }
        public string UrlVideo { get; set; }
        public decimal Cost { get; set; }
        public byte System { get; set; }
        public bool IsPublic { get; set; }
        public bool HasCertification { get; set; }
        public DateTime EventDate { get; set; }
        public DateTime RegistrationStartDate { get; set; }
        public DateTime RegistrationEndDate { get; set; }
        public ApplicationUser Organizer { get; set; }
        public EventCertification EventCertification { get; set; }
        public EventType EventType { get; set; }
        public ICollection<EventRole> EventRoles { get; set; }
        public ICollection<EventCareer> EventCareers { get; set; }
        public ICollection<UserEvent> UserEvents { get; set; }
        public ICollection<EventFile> EventFiles { get; set; }
        public ICollection<EventEvidence> EventEvidences { get; set; }
    }
}

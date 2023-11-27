using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.Laurassia
{
    public class Consult : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid ConsultTypeId { get; set; }
        public Guid SectionId { get; set; }
        public string UserId { get; set; }
        public string AttachmentUrl { get; set; }
        public DateTime? DateCreated { get; set; }
        public string Message { get; set; }
        public string Phone { get; set; }
        public string Rol { get; set; }
        public string State { get; set; }
        public string Response { get; set; }
        public string Subject { get; set; }
        public ApplicationUser User { get; set; }
        public ConsultType ConsultType { get; set; }
        public Section Section { get; set; }
    }
}

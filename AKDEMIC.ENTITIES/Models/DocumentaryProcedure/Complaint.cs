using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

namespace AKDEMIC.ENTITIES.Models.DocumentaryProcedure
{
    public class Complaint : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public string Code { get; set; }

        public Guid? DependencyId { get; set; }
        public Dependency Dependency { get; set; }
        //Identificación del consumidor reclamante
        public Guid ExternalUserId { get; set; }
        public ExternalUser ExternalUser { get; set; }

        //Identificación del bien contratado
        public byte HiredType { get; set; }
        public decimal ReclaimedAmount { get; set; }
        public string Description { get; set; }
        public string UnitName { get; set; }
        public Guid CampusId { get; set; }
        public Campus Campus { get; set; }

        //Detalle de la reclamación
        public Guid ComplaintTypeId { get; set; }
        public ComplaintType ComplaintType { get; set; }
        public string Detail { get; set; }
        public string Request { get; set; }

        //Respuesta del usuario
        public DateTime? DateTimeResponse { get; set; }
        public byte Status { get; set; }
        public string UserResponse { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<ComplaintFile> ComplaintFiles { get; set; }
    }
}

using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.ENTITIES.Models.Scale.Entities
{
    public class WorkerFamilyInformation : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string PaternalName { get; set; }
        public string MaternalName { get; set; }
        public int Sex { get; set; } // AKDEMIC.CORE.Helpers.ConstantHelpers.SEX
        public int Relationship { get; set; } // AKDEMIC.CORE.Helpers.ConstantHelpers.WORKERFAMILYINFORMATION_RELATIONSHIP
        public DateTime? BirthDate { get; set; }
        public bool IsAlive { get; set; }
        public bool HasDiscapacity { get; set; }
        public string DniOtherDoc { get; set; }
        public Guid WorkerLaborInformationId { get; set; }

        public WorkerLaborInformation WorkerLaborInformation { get; set; }
    }
}

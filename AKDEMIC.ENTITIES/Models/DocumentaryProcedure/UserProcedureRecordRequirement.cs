using System;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;

namespace AKDEMIC.ENTITIES.Models.DocumentaryProcedure
{
    public class UserProcedureRecordRequirement : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid ProcedureRequirementId { get; set; }
        public Guid UserProcedureRecordId { get; set; }

        public ProcedureRequirement ProcedureRequirement { get; set; }
        public UserProcedureRecord UserProcedureRecord { get; set; }
    }
}

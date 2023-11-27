using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.DocumentaryProcedure
{
    public class UserProcedureDerivation : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid DependencyId { get; set; }
        public Guid DependencyFromId { get; set; }
        public Guid ProcedureTaskId { get; set; }
        public string UserId { get; set; }
        public Guid UserProcedureId { get; set; }
        public string UserTargetId { get; set; }
        public string Observation { get; set; }
        public string FinalFileUrl { get; set; }
        public ProcedureTask ProcedureTask { get; set; }
        public ApplicationUser User { get; set; }
        public Dependency Dependency { get; set; }
        public Dependency DependencyFrom { get; set; }
        public UserProcedure UserProcedure { get; set; }
        public ICollection<UserProcedureDerivationFile> UserProcedureDerivationFiles { get; set; }
    }
}

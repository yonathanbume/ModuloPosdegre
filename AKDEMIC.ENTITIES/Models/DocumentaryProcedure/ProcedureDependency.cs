using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.DocumentaryProcedure
{
    public class ProcedureDependency : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid DependencyId { get; set; }
        public Guid ProcedureId { get; set; }

        [NotMapped]
        public int Index { get; set; }

        public Dependency Dependency { get; set; }
        public Procedure Procedure { get; set; }
        public ICollection<UserProcedure> UserProceduresProcedureDependency { get; set; }
        /*public ICollection<UserProcedure> UserProceduresNextProcedureDependency { get; set; }
        public ICollection<UserProcedure> UserProceduresPrevProcedureDependency { get; set; }*/
    }
}

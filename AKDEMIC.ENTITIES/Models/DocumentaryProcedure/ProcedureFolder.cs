using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.DocumentaryProcedure
{
    public class ProcedureFolder
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public Guid DependencyId { get; set; }

        public Dependency Dependency { get; set; }

        public ICollection<UserProcedure> UserProcedures { get; set; }
    }
}

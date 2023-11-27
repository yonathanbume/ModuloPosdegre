using System;

namespace AKDEMIC.ENTITIES.Models.DocumentaryProcedure
{
    public class ProcedureResolution
    {
        public Guid Id { get; set; }
        public Guid DependencyId { get; set; }
        public Guid ProcedureId { get; set; }
        public int PresentationTerm { get; set; }
        public int ResolutionTerm { get; set; }
        public int ResolutionType { get; set; }

        public Dependency Dependency { get; set; }
        public Procedure Procedure { get; set; }
    }
}

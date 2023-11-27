using System;

namespace AKDEMIC.ENTITIES.Models.DocumentaryProcedure
{
    public class InternalProcedureReference
    {
        public Guid Id { get; set; }
        public Guid InternalProcedureId { get; set; }

        public string Reference { get; set; }
    }
}

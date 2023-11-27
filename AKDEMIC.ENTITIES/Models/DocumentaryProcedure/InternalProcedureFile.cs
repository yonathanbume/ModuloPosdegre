using System;

namespace AKDEMIC.ENTITIES.Models.DocumentaryProcedure
{
    public class InternalProcedureFile
    {
        public Guid Id { get; set; }
        public Guid InternalProcedureId { get; set; }

        public string FileName { get; set; }
        public string Path { get; set; }
        public long Size { get; set; }

        public InternalProcedure InternalProcedure { get; set; }
    }
}

using System;

namespace AKDEMIC.ENTITIES.Models.DocumentaryProcedure
{
    public class UserProcedureDerivationFile
    {
        public Guid Id { get; set; }
        public Guid UserProcedureDerivationId { get; set; }

        public string FileName { get; set; }
        public string Path { get; set; }
        public long Size { get; set; }

        public UserProcedureDerivation UserProcedureDerivation { get; set; }
    }
}

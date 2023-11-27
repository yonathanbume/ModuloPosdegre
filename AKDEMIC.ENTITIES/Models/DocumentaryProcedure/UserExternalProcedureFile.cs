using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.DocumentaryProcedure
{
    public class UserExternalProcedureFile
    {
        public Guid Id { get; set; }
        public Guid UserExternalProcedureId { get; set; }

        public string FileName { get; set; }
        public string Path { get; set; }
        public long Size { get; set; }

        public UserExternalProcedure UserExternalProcedure { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.DocumentaryProcedure
{
    public class UserProcedureFile
    {
        public Guid Id { get; set; }
        public Guid? ProcedureRequirementId { get; set; }
        public Guid UserProcedureId { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
        public long Size { get; set; }
        public int Status { get; set; }
        public UserProcedure UserProcedure { get; set; }
        public ProcedureRequirement ProcedureRequirement { get; set; }
    }
}

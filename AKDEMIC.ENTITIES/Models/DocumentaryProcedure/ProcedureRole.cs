using System;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.ENTITIES.Models.DocumentaryProcedure
{
    public class ProcedureRole
    {
        public Guid Id { get; set; }
        public Guid ProcedureId { get; set; }
        public string RoleId { get; set; }

        public ApplicationRole Role { get; set; }
        public Procedure Procedure { get; set; }
    }
}

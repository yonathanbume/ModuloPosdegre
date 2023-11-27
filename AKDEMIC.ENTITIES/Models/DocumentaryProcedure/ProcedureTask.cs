using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.DocumentaryProcedure
{
    public class ProcedureTask : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid ProcedureId { get; set; }
        public Procedure Procedure { get; set; }
        public byte Type { get; set; }
        public string Description { get; set; }

        public byte? ActivityType { get; set; }
        public byte? RecordHistoryType{ get; set; }
    }
}

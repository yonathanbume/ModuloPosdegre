using AKDEMIC.ENTITIES.Models.Admission;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.DocumentaryProcedure
{
    public class ProcedureAdmissionType
    {
        public Guid Id { get; set; }

        public Guid ProcedureId { get; set; }
        public Guid AdmissionTypeId { get; set; }

        public Procedure Procedure { get; set; }
        public AdmissionType AdmissionType { get; set; }
    }
}

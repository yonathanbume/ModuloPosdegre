using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;

namespace AKDEMIC.ENTITIES.Models.Payroll
{
    public class Payroll : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public Guid PayrollClassId { get; set; }

        public Guid PayrollTypeId { get; set; }

        public PayrollClass PayrollClass { get; set; }

        public PayrollType PayrollType { get; set; }

        public byte Interval { get; set; }

        public bool Processed { get; set; } = false;

        public Guid WorkingTermId { get; set; }

        public WorkingTerm WorkingTerm { get; set; }
    }
}

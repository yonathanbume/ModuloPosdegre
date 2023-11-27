using System;

namespace AKDEMIC.ENTITIES.Models.Payroll
{
    public class PaymentMethod
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; } = true;
    }
}

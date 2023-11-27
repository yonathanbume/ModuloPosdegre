using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Generals
{
    public class StudentBenefit
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Discount { get; set; }
    }
}

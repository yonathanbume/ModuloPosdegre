﻿using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.ENTITIES.Models.Payroll
{
    public class ConceptType : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsDiscount { get; set; }//Es descuento o no
        public ICollection<WageItem> WageItems { get; set; }
    }
}

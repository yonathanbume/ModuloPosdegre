using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.EconomicManagement
{
    public class MonthlyBalance
    {
        public byte Month { get; set; }
        public int Year { get; set; }

        public bool WasClosed { get; set; }
    }
}

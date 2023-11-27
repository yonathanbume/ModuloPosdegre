using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.ENTITIES.Models.Indicators
{
    public class IndicatorImprovement : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public int Year { get; set; }
        public int Number { get; set; }
        public Guid IndicatorProcessesId { get; set; }
        public IndicatorProcesses IndicatorProcesses { get; set; }
    }
}
 
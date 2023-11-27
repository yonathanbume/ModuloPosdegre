using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.ENTITIES.Models.Indicators
{
    public class ResearchPerYear
    {
        public Guid Id { get; set; }

        public int Year { get; set; }

        public int PublishedResearch { get; set; }

        public int TotalInvestigations { get; set; }
    }
}

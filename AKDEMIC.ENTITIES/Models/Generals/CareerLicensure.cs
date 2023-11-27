using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Generals
{
    public class CareerLicensure
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string ResolutionFile { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid CareerId { get; set; }
        public Career Career { get; set; }
    }
}

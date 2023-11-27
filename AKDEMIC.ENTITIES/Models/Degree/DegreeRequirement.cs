using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Degree
{
    public class DegreeRequirement
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
    }
}

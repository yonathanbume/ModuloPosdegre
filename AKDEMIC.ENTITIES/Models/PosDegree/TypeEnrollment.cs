using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.ENTITIES.Models.PosDegree
{
     public class TypeEnrollment
    {
        public Guid id { get; set;}
        public string Name { get; set; }
        public string Description { get; set; }
        public float Costo { get; set; }
        public ICollection<TypeEnrollment> TypeEnrollments { get; set; }
    }
}

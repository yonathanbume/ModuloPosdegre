using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.ENTITIES.Models.PosDegree
{
    public  class Semestre
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public DateTime startaDate { get; set; }
        public DateTime endaDate { get; set;}
        
    }
}

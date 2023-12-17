using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.ENTITIES.Models.PosDegree
{
    public  class RegistroNotas
    {
        public Guid id { get; set; }
        public int Creditos { get; set; }
        public int Nota { get; set; }  
    }
}

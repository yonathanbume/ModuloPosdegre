using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.ENTITIES.Models.PosDegree
{
    public  class Master
    {
        public Guid id { get; set; }
        public string Nombre { get; set; }
        public int Duracion { get; set; } //duracion en años 
        public int Creditos { get; set; }
        public string Descripcion { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.ENTITIES.Models.PosDegree
{
    public  class RegistroNotas
    {
        public Guid id { get; set; }
        public int Nota { get; set; }
        public string NotaTexto { get; set; }
        public Guid EnrollmentId { get; set; }
        public Enrollment enrollment { get; set; } = null; // Reference navigation to dependent

    }
}

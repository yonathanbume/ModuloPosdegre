using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace AKDEMIC.ENTITIES.Models.PosDegree
{
    public  class Asignatura
    {
        public Guid id { get; set; }
        public string  Code { get; set; }
        public string NameAsignatura { get; set; }
        public decimal Credits { get; set; } = 1.0M;
        public byte PracticalHours { get; set; } = 0;
        public byte TeoricasHours { get; set; } = 0;
        public byte TotalHours { get; set; } = 0;
        public string Requisito { get; set; }
        public Guid RegistroNotasId { get; set; }
        public RegistroNotas RegistroNotas { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
    }
}

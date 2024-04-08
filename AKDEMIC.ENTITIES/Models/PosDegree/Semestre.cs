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
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set;}
        public int CreditMin { get; set; }
        public int CreditMax { get;set; }
        public int NotaMin { get; set; }
        public int NotaMax { get; set; }
        public ICollection<TeachingLoad> teachingLoads { get; set; }
    }
}

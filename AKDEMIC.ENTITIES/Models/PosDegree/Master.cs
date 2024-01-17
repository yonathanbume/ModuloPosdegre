using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.ENTITIES.Models.PosDegree
{
    public  class Master
    {   
        public Guid id { get; set; }
        public int Nro { get; set; }
        public string Campus { get; set; } 
        public string MallaCuricular { get; set; }
        public string StudyProgram { get; set; }
        public string StudyMode { get; set; } 
        public bool current { get; set; } = true;
        public bool state { get; set; } = true;
        public string Nombre { get; set; }
        public int Duracion { get; set; } // Duración en años
        public int Creditos { get; set; }
        public string Descripcion { get; set; }
        public ICollection<TeachingLoad> teachingLoads { get; set; }

    }


}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.ENTITIES.Models.PosDegree
{
    public  class TeachingLoad
    {
        public Guid Id { get; set; }

        public Guid MasterId { get; set; }
        public Master master { get; set; }

        public Guid AsignaturaId { get; set; }
        public Asignatura asignatura { get;set; }

        public Guid SemestreId { get; set; }
        public Semestre semestre { get; set; }
        
        public Guid PosdegreeTeacherId { get; set; }
        public PosdegreeTeacher posdegreeTeacher { get; set; }

        public ICollection<Enrollment> enrollments { get; set; }


    }
}

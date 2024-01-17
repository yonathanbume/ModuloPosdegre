using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.ENTITIES.Models.PosDegree
{
    public  class Enrollment
    {
        public Guid id { get; set; }
        public RegistroNotas? registroNotas { get; set; } // Reference navigation to dependent

        public DateTime FechaMatricula { get; set; }
        public int state { get; set; }
        public string observation { get; set; }

        public Guid TypeEnrollmentId { get; set; }
        public TypeEnrollment TypeEnrollment { get; set; }

        public Guid PosdegreeStudentId { get; set; }
        public PosdegreeStudent posdegreeStudent { get; set; }

        public Guid TeachingLoadId { get; set; }
        public TeachingLoad teachingLoad { get; set; }

      
     

    }
}

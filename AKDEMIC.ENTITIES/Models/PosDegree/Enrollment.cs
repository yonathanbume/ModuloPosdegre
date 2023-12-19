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
        public DateTime FechaMatricula { get; set; }
        public Guid SemestreId { get; set; }
        public Semestre semestre { get; set; }
        public Guid PosdegreeStudentId { get; set; }
        public PosdegreeStudent PosdegreeStudent { get; set; }
        public Guid TypeEnrollmentId { get; set; }
        public TypeEnrollment TypeEnrollment { get; set; }
        public Guid AsignaturaId { get; set; }
        public Asignatura Asignatura { get; set; }
        public ICollection<DetailEnrollmentMaster> DetailEnrollmentMasters { get; set; }
    }
}

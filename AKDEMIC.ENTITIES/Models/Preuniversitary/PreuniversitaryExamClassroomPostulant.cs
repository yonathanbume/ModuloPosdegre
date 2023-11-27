using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.ENTITIES.Models.Preuniversitary
{
    public class PreuniversitaryExamClassroomPostulant : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid PreuniversitaryPostulantId { get; set; }
        public PreuniversitaryPostulant PreuniversitaryPostulant { get; set; }
        public Guid PreuniversitaryExamClassroomId { get; set; }
        public PreuniversitaryExamClassroom PreuniversitaryExamClassroom { get; set; }
        public bool Attended { get; set; }
        public int Seat { get; set; }
    }
}

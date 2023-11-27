using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.ENTITIES.Models.Preuniversitary
{
    public class PreuniversitaryExamClassroom : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid ClassroomId { get; set; }
        public int Vacancies { get; set; }
        public Guid PreuniversitaryExamId { get; set; }
        public Classroom Classroom { get; set; }
        public PreuniversitaryExam PreuniversitaryExam { get; set; }
    }
}

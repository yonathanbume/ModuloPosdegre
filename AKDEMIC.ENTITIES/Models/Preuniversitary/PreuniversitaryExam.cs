using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.ENTITIES.Models.Preuniversitary
{
    public class PreuniversitaryExam : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal MinimumScore { get; set; }
        public int Weight { get; set; }
        public DateTime DateEvaluation { get; set; }
        public Guid PreuniversitaryTermId { get; set; }
        public PreuniversitaryTerm PreuniversitaryTerm { get; set; }
        public ICollection<PreuniversitaryExamClassroom> Classrooms { get; set; }
    }
}

using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;

namespace AKDEMIC.ENTITIES.Models.Preuniversitary
{
    public class PreuniversitaryExamTeacher : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid PreuniversitaryExamId { get; set; }
        public PreuniversitaryExam PreuniversitaryExam { get; set; }
        public Guid? PreuniversitaryExamClassroomId { get; set; }
        public PreuniversitaryExamClassroom PreuniversitaryExamClassroom { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

    }
}

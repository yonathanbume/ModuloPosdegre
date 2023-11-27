using System;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.ENTITIES.Models.TeachingManagement
{
    public class TeacherTermInform : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public string TeacherId { get; set; }
        public Guid TermInformId { get; set; }
        public Guid? SectionId { get; set; }
        public string Url { get; set; }
        public TermInform TermInform { get; set; }
        public Section Section { get; set; }
        public Teacher Teacher { get; set; }
    }
}

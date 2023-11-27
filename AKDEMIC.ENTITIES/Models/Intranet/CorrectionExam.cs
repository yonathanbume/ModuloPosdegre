using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Section = AKDEMIC.ENTITIES.Models.Enrollment.Section;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class CorrectionExam : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        
        public Guid TermId { get; set; }
        public Term Term { get; set; }

        public Guid SectionId { get; set; }
        public Section Section { get; set; }

        public Guid? ClassroomId { get; set; }
        public Classroom Classroom { get; set; }

        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }

        public string FileResolution { get; set; }
        public string File { get; set; }

        public string TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        public List<CorrectionExamStudent> CorrectionExamStudents { get; set; }
    }
}

using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.Text;


namespace AKDEMIC.ENTITIES.Models.InstitutionalWelfare
{
    public class InstitutionalRecordCategorizationByStudent
    {
        public Guid Id { get; set; }
        public Guid InstitutionalWelfareRecordId { get; set; }        
        public Guid StudentId { get; set; }
        public Guid CategorizationLevelId { get; set; }
        public int StudentScore { get; set; }
        public Guid TermId { get; set; }
        public bool IsEvaluated { get; set; }
        public InstitutionalWelfareRecord InstitutionalWelfareRecord { get; set; }
        public CategorizationLevel CategorizationLevel { get; set; }
        public Student Student { get; set; }      
        public Term Term { get; set; }
        public byte SisfohClasification { get; set; } // AKDEMIC.CORE.Helpers.ConstantHelpers.INSTITUTIONAL_WELFARE_SURVEY.SISFOH.VALUES
        public string SisfohConstancy { get; set; }

    }
}

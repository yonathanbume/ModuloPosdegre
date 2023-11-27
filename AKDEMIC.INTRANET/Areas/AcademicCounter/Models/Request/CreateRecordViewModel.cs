using System;

namespace AKDEMIC.INTRANET.Areas.AcademicCounter.Models.Request
{
    public class CreateRecordViewModel
    {
        public Guid StudentId { get; set; }

        public int RecordType { get; set; }

        public string AcademicRecordStaffId { get; set; }

        public Guid? TermId { get; set; }

        //Certificado Parcial
        public int? RangeType { get; set; }
        public int? StartAcademicYear { get; set; }
        public int? EndAcademicYear { get; set; }
        public Guid? StartTerm { get; set; }
        public Guid? EndTerm { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.AcademicRecord.Models.Request
{
    public class CreateDocumentViewModel
    {
        public Guid StudentId { get; set; }
        public Guid? TermId { get; set; }
        public Guid? UserProcedureId { get; set; }
        public int RecordType { get; set; }

        //Certificado Parcial
        public int? RangeType { get; set; }
        public string JsonAcademicYearPartials { get; set; }
    }
}

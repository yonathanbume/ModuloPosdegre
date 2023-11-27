using System;

namespace AKDEMIC.INTRANET.Areas.Academic.ViewModels.ConstancyViewModels
{
    public class CreateRecordViewModel
    {
        public Guid StudentId { get; set; }

        public int RecordType { get; set; }

        public string Observations { get; set; }

        public string AcademicRecordStaffId { get; set; }
    }
}

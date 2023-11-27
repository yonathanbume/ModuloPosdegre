using System;

namespace AKDEMIC.INTRANET.Areas.AcademicRecord.Models.Request
{
    public class UpdateRequestViewModel
    {
        public Guid RecordHistoryId { get; set; }
        public byte Status { get; set; }
        public string Observation { get; set; }
    }
}

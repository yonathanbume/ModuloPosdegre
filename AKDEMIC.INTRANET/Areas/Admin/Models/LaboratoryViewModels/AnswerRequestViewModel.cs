using System;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.LaboratoryViewModels
{
    public class AnswerRequestViewModel
    {
        public Guid SectionId { get; set; }

        public string Observation { get; set; }

        public string Date { get; set; }

        public string TimeStart { get; set; }

        public string TimeEnd { get; set; }
    }
}

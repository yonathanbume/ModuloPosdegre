using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Admission
{
    public class AdmissionExamChannel
    {
        public Guid AdmissionExamId { get; set; }
        public Guid ChannelId { get; set; }

        public AdmissionExam AdmissionExam { get; set; }
        public Channel Channel { get; set; }
    }
}

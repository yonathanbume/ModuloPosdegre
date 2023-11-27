using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Admission.Interfaces
{
    public interface IAdmissionExamChannelService
    {
        Task<List<Channel>> GetChannelsByAdmissionExamId(Guid admissionExamId);
        void RemoveRange(List<AdmissionExamChannel> applicationTerms);
    }
}

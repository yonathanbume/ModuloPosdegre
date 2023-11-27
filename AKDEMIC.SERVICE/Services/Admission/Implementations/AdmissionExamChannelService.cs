using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces;
using AKDEMIC.SERVICE.Services.Admission.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Admission.Implementations
{
    public class AdmissionExamChannelService : IAdmissionExamChannelService
    {
        private readonly IAdmissionExamChannelRepository _admissionExamChannelRepository;
        public AdmissionExamChannelService(IAdmissionExamChannelRepository admissionExamChannelRepository)
        {
            _admissionExamChannelRepository = admissionExamChannelRepository;
        }

        public async Task<List<Channel>> GetChannelsByAdmissionExamId(Guid admissionExamId)
            => await _admissionExamChannelRepository.GetChannelsByAdmissionExamId(admissionExamId);

        public void RemoveRange(List<AdmissionExamChannel> channels)
            => _admissionExamChannelRepository.RemoveRange(channels);

    }
}

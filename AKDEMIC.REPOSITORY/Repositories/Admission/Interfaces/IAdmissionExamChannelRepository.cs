using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces
{
    public interface IAdmissionExamChannelRepository : IRepository<AdmissionExamChannel>
    {
        Task<List<Channel>> GetChannelsByAdmissionExamId(Guid admissionExamId);
    }
}

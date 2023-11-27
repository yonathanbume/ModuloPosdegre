using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class ExtracurricularCourseGroupAssistanceService : IExtracurricularCourseGroupAssistanceService
    {
        private readonly IExtracurricularCourseGroupAssistanceRepository _extracurricularCourseGroupAssistanceRepository;

        public ExtracurricularCourseGroupAssistanceService(IExtracurricularCourseGroupAssistanceRepository extracurricularCourseGroupAssistanceRepository)
        {
            _extracurricularCourseGroupAssistanceRepository = extracurricularCourseGroupAssistanceRepository;
        }

        public Task DeleteById(Guid id)
            => _extracurricularCourseGroupAssistanceRepository.DeleteById(id);

        public Task<ExtracurricularCourseGroupAssistance> Get(Guid id)
            => _extracurricularCourseGroupAssistanceRepository.Get(id);

        public Task<IEnumerable<ExtracurricularCourseGroupAssistance>> GetAllByGroup(Guid groupId)
            => _extracurricularCourseGroupAssistanceRepository.GetAllByGroup(groupId);

        public Task Insert(ExtracurricularCourseGroupAssistance extracurricularCourseGroupAssistance)
            => _extracurricularCourseGroupAssistanceRepository.Insert(extracurricularCourseGroupAssistance);

        public Task Update(ExtracurricularCourseGroupAssistance extracurricularCourseGroupAssistance)
            => _extracurricularCourseGroupAssistanceRepository.Update(extracurricularCourseGroupAssistance);
    }
}

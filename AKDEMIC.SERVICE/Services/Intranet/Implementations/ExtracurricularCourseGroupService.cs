using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public class ExtracurricularCourseGroupService : IExtracurricularCourseGroupService
    {
        private readonly IExtracurricularCourseGroupRepository _extracurricularCourseGroupRepository;

        public ExtracurricularCourseGroupService(IExtracurricularCourseGroupRepository extracurricularCourseGroupRepository)
        {
            _extracurricularCourseGroupRepository = extracurricularCourseGroupRepository;
        }

        public Task DeleteById(Guid id)
            => _extracurricularCourseGroupRepository.DeleteById(id);

        public Task<ExtracurricularCourseGroup> Get(Guid id)
            => _extracurricularCourseGroupRepository.Get(id);

        public Task<IEnumerable<ExtracurricularCourseGroup>> GetAll(string teacherId = null)
            => _extracurricularCourseGroupRepository.GetAll(teacherId);

        public Task<ExtracurricularCourseGroup> GetByCode(string code)
            => _extracurricularCourseGroupRepository.GetByCode(code);

        public async Task<object> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string search = null)
            => await _extracurricularCourseGroupRepository.GetDataDatatable(sentParameters, search);


        public async Task<ExtracurricularCourseGroup> GetWithIncludes(Guid id)
        {
            return await _extracurricularCourseGroupRepository.GetWithIncludes(id);
        }

        public Task Insert(ExtracurricularCourseGroup extracurricularCourseGroup)
            => _extracurricularCourseGroupRepository.Insert(extracurricularCourseGroup);

        public Task Update(ExtracurricularCourseGroup extracurricularCourseGroup)
            => _extracurricularCourseGroupRepository.Update(extracurricularCourseGroup);
    }
}

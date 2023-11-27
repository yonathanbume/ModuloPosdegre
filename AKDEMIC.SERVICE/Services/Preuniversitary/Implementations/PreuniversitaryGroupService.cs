using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Preuniversitary;
using AKDEMIC.REPOSITORY.Repositories.Preuniversitary.Interfaces;
using AKDEMIC.SERVICE.Services.Preuniversitary.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Preuniversitary.Implementations
{
    public class PreuniversitaryGroupService : IPreuniversitaryGroupService
    {
        private readonly IPreuniversitaryGroupRepository _preuniversitaryGroupRepository;

        public PreuniversitaryGroupService(IPreuniversitaryGroupRepository preuniversitaryGroupRepository)
        {
            _preuniversitaryGroupRepository = preuniversitaryGroupRepository;
        }

        public async Task Delete(PreuniversitaryGroup entity)
            => await _preuniversitaryGroupRepository.Delete(entity);

        public async Task<PreuniversitaryGroup> Get(Guid id)
            => await _preuniversitaryGroupRepository.Get(id);

        public async Task<List<PreuniversitaryGroup>> GetAllByTermIdAndCourseId(Guid preuniversitaryTermId, Guid preuniversitaryCourseId)
            => await _preuniversitaryGroupRepository.GetAllByTermIdAndCourseId(preuniversitaryTermId, preuniversitaryCourseId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetCourseGroupByTeacherDatatable(DataTablesStructs.SentParameters sentParameters, Guid currentTermId, string teacherId, string searchValue = null)
            => await _preuniversitaryGroupRepository.GetCourseGroupByTeacherDatatable(sentParameters, currentTermId, teacherId, searchValue);

        public async Task<DataTablesStructs.ReturnedData<object>> GetGroupsDatatable(DataTablesStructs.SentParameters sentParameters, Guid courseId, Guid termId, string searchValue = null)
            => await _preuniversitaryGroupRepository.GetGroupsDatatable(sentParameters, courseId, termId, searchValue);

        public async Task<DataTablesStructs.ReturnedData<object>> GetReportAdvanceDatatable(DataTablesStructs.SentParameters sentParameters, Guid preuniversitaryTermId, string searchValue = null)
            => await _preuniversitaryGroupRepository.GetReportAdvanceDatatable(sentParameters, preuniversitaryTermId, searchValue);

        public async Task<DataTablesStructs.ReturnedData<object>> GetReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid preuniversitaryTermId, string searchValue)
            => await _preuniversitaryGroupRepository.GetReportDatatable(sentParameters, preuniversitaryTermId, searchValue);

        public async Task<object> GetStudentsByGroupId(Guid groupId)
            => await _preuniversitaryGroupRepository.GetStudentsByGroupId(groupId);

        public async Task<object> GetTemaries(Guid groupId, bool? status = null)
            => await _preuniversitaryGroupRepository.GetTemaries(groupId, status);

        public async Task Insert(PreuniversitaryGroup entity)
            => await _preuniversitaryGroupRepository.Insert(entity);

        public async Task Update(PreuniversitaryGroup entity)
            => await _preuniversitaryGroupRepository.Update(entity);
    }
}

using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Templates.ScoreInputSchedule;
using AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces;

namespace AKDEMIC.SERVICE.Services.TeachingManagement.Implementations
{
    public sealed class ScoreInputScheduleService : IScoreInputScheduleService
    {
        private readonly IScoreInputScheduleRepository _scoreInputScheduleRepository;
        public ScoreInputScheduleService(IScoreInputScheduleRepository scoreInputScheduleRepository)
        {
            _scoreInputScheduleRepository = scoreInputScheduleRepository;
        }

        public async Task<ScoreInputSchedule> GetByTermAndCourseComponent(Guid termId, Guid courseComponentId)
            => await _scoreInputScheduleRepository.GetByTermAndCourseComponent(termId, courseComponentId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetReportScoreInputScheduleDatatable(DataTablesStructs.SentParameters sentParameters, Guid id, Guid? careerId, Guid? curriculumId,int? unit, byte? status, Guid? academicDepartmentId , ClaimsPrincipal user, string searchValue,int? week)
            => await _scoreInputScheduleRepository.GetReportScoreInputScheduleDatatable(sentParameters, id, careerId, curriculumId,unit, status, academicDepartmentId, user, searchValue, week);

        public async Task<ScoreInputScheduleViewModel> GetReportScoreInputSchedule(Guid id, Guid? careerId, Guid? curriculumId ,int? unit, byte? status, Guid? academicDepartmentId, ClaimsPrincipal user, int? week)
            => await _scoreInputScheduleRepository.GetReportScoreInputSchedule(id, careerId, curriculumId,unit, status, academicDepartmentId, user, week);

        public async Task<DataTablesStructs.ReturnedData<object>> GetScoreInputScheduleDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue)
            => await _scoreInputScheduleRepository.GetScoreInputScheduleDatatable(sentParameters, searchValue);

        Task<bool> IScoreInputScheduleService.AnyByCourseComponentId(Guid courseComponentId)
            => _scoreInputScheduleRepository.AnyByCourseComponentId(courseComponentId);

        Task<bool> IScoreInputScheduleService.AnyByTermIdCourseComponentId(Guid termId, Guid? courseComponentId, Guid? id)
            => _scoreInputScheduleRepository.AnyByTermIdCourseComponentId(termId, courseComponentId, id);

        Task IScoreInputScheduleService.DeleteAsync(ScoreInputSchedule scoreInputSchedule)
            => _scoreInputScheduleRepository.Delete(scoreInputSchedule);

        Task<object> IScoreInputScheduleService.GetAllAsModelA()
            => _scoreInputScheduleRepository.GetAllAsModelA();

        Task<object> IScoreInputScheduleService.GetAsModelB(Guid? id)
            => _scoreInputScheduleRepository.GetAsModelB(id);

        Task<ScoreInputSchedule> IScoreInputScheduleService.GetAsync(Guid id)
            => _scoreInputScheduleRepository.Get(id);

        Task IScoreInputScheduleService.InsertAsync(ScoreInputSchedule scoreInputSchedule)
            => _scoreInputScheduleRepository.Insert(scoreInputSchedule);

        Task IScoreInputScheduleService.UpdateAsync(ScoreInputSchedule scoreInputSchedule)
            => _scoreInputScheduleRepository.Update(scoreInputSchedule);

        public async Task<DataTablesStructs.ReturnedData<object>> GetReportConsolidatedScoreInputSchedule(DataTablesStructs.SentParameters parameters, ClaimsPrincipal user, Guid termId, Guid? careerId, Guid? curriculumId, string searchValue, byte status)
            => await _scoreInputScheduleRepository.GetReportConsolidatedScoreInputSchedule(parameters, user, termId, careerId, curriculumId, searchValue, status);

        public async Task<List<ReportScoreInputScheduleConsolidated>> GetReportConsolidatedScoreInputSchedule(ClaimsPrincipal user, Guid termId, Guid? careerId, Guid? curriculumId , byte status)
            => await _scoreInputScheduleRepository.GetReportConsolidatedScoreInputSchedule(user, termId, careerId, curriculumId, status);
    }
}
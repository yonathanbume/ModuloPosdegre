using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeachingManagement.Implementations
{
    public sealed class TeacherTermInformService : ITeacherTermInformService
    {
        private readonly ITeacherTermInformRepository _teacherTermInformRepository;

        public TeacherTermInformService(ITeacherTermInformRepository teacherTermInformRepository)
        {
            _teacherTermInformRepository = teacherTermInformRepository;
        }

        public Task<DataTablesStructs.ReturnedData<object>> GetTermInformsDatatable(DataTablesStructs.SentParameters parameters, int? type = null, ClaimsPrincipal user = null)
            => _teacherTermInformRepository.GetTermInformsDatatable(parameters,type, user);

        public Task<object> GetChartReportDataByStateCourseTermAndTerm(int? state, Guid? termId, Guid? courseTermId)
            => _teacherTermInformRepository.GetChartReportDataByStateCourseTermAndTerm(state, termId, courseTermId);

        public Task<object> GetChartReportDatatableByStateCourseTermAndTerm(int? state, Guid? termId, Guid? courseTermId)
            => _teacherTermInformRepository.GetChartReportDatatableByStateCourseTermAndTerm(state, termId, courseTermId);

        public Task InsertAsync(TeacherTermInform teacherTermInform)
            => _teacherTermInformRepository.Insert(teacherTermInform);

        public Task<object> GetTermInformsChart(int? type = null, ClaimsPrincipal user = null)
            => _teacherTermInformRepository.GetTermInformsChart(type, user);

        public async Task<TeacherTermInform> Get(Guid id)
            => await _teacherTermInformRepository.Get(id);

        public async Task Delete(TeacherTermInform entity)
            => await _teacherTermInformRepository.Delete(entity);
    }
}
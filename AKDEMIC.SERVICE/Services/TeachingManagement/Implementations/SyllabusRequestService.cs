using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Templates.SyllabusRequest;
using AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces;

namespace AKDEMIC.SERVICE.Services.TeachingManagement.Implementations
{
    public sealed class SyllabusRequestService : ISyllabusRequestService
    {
        private readonly ISyllabusRequestRepository _syllabusRequestRepository;
        public SyllabusRequestService(ISyllabusRequestRepository syllabusRequestRepository)
        {
            _syllabusRequestRepository = syllabusRequestRepository;
        }

        public async Task<SyllabusRequest> GetByTerm(Guid termId)
            => await _syllabusRequestRepository.GetByTerm(termId);

        public async Task<ChartJSTemplate> GetChartJsReport(Guid termId, Guid? facultyId,ClaimsPrincipal user)
            => await _syllabusRequestRepository.GetChartJsReport(termId, facultyId, user);

        public async Task<ChartJSTemplate> GetChartJsReportByAcademicDepartment(Guid termId, Guid? academicDepartmentId, ClaimsPrincipal user)
            => await _syllabusRequestRepository.GetChartJsReportByAcademicDepartment(termId, academicDepartmentId, user);

        public async Task<SyllabusRequest> GetLastSyllabusRequestOpened()
            => await _syllabusRequestRepository.GetLastSyllabusRequestOpened();

        public async Task<DataTablesStructs.ReturnedData<object>> GetSyllabusRequestDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue)
            => await _syllabusRequestRepository.GetSyllabusRequestDatatable(sentParameters, searchValue);

        public async Task<List<Select2Structs.Result>> GetSyllabusRequestTermSelect2()
            => await _syllabusRequestRepository.GetSyllabusRequestTermSelect2();

        public async Task<DataTablesStructs.ReturnedData<object>> GetSyllabusRequestToTeachersDatatable(DataTablesStructs.SentParameters sentParameters, string teacherId, Guid? termId, string searchValue = null)
            => await _syllabusRequestRepository.GetSyllabusRequestToTeachersDatatable(sentParameters, teacherId, termId, searchValue);

        Task<bool> ISyllabusRequestService.AnyByTermId(Guid termId)
            => _syllabusRequestRepository.AnyByTermId(termId);

        Task ISyllabusRequestService.DeleteAsync(SyllabusRequest syllabusRequest)
            => _syllabusRequestRepository.Delete(syllabusRequest);

        Task<object> ISyllabusRequestService.GetAllAsModelA()
            => _syllabusRequestRepository.GetAllAsModelA();

        Task<object> ISyllabusRequestService.GetAllAsModelB(string coordinatorId, string teacherId)
            => _syllabusRequestRepository.GetAllAsModelB(coordinatorId, teacherId);

        Task<object> ISyllabusRequestService.GetAsModelA(Guid? id)
            => _syllabusRequestRepository.GetAsModelA(id);

        Task<SyllabusRequest> ISyllabusRequestService.GetAsync(Guid id)
            => _syllabusRequestRepository.Get(id);

        Task ISyllabusRequestService.InsertAsync(SyllabusRequest syllabusRequest)
            => _syllabusRequestRepository.Insert(syllabusRequest);

        Task ISyllabusRequestService.UpdateAsync(SyllabusRequest syllabusRequest)
            => _syllabusRequestRepository.Update(syllabusRequest);
    }
}
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces;
using AKDEMIC.SERVICE.Services.JobExchange.Interfaces;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.JobExchange.Implementations
{
    public class CoordinatorCareerService : ICoordinatorCareerService
    {
        private readonly ICoordinatorCareerRepository _coordinatorCareerRepository;

        public CoordinatorCareerService(ICoordinatorCareerRepository coordinatorCareerRepository)
        {
            _coordinatorCareerRepository = coordinatorCareerRepository;
        }

        public async Task<bool> AnyByUserId(string userId)
        {
            return await _coordinatorCareerRepository.AnyByUserId(userId);
        }

        public async Task Delete(CoordinatorCareer coordinatorCareer)
        {
            await _coordinatorCareerRepository.Delete(coordinatorCareer);
        }

        public async Task DeleteById(Guid id)
        {
            await _coordinatorCareerRepository.DeleteById(id);
        }

        public async Task DownloadExcel(IXLWorksheet worksheet)
        {
            await _coordinatorCareerRepository.DownloadExcel(worksheet);
        }

        public async Task<CoordinatorCareer> Get(Guid id)
        {
            return await _coordinatorCareerRepository.Get(id);
        }

        public async Task<object> GetCareersByFacultySelect2ClientSide(Guid? facultyId, ClaimsPrincipal user = null)
        {
            return await _coordinatorCareerRepository.GetCareersByFacultySelect2ClientSide(facultyId, user);
        }

        public async Task<List<CoordinatorCareer>> GetCoordinatorCareer(string userId)
        {
            return await _coordinatorCareerRepository.GetCoordinatorCareer(userId);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetCoordinatorsCareerDatatable(DataTablesStructs.SentParameters sentParameters, Guid? careerId , string searchValue = null)
        {
            return await _coordinatorCareerRepository.GetCoordinatorsCareerDatatable(sentParameters, careerId, searchValue);
        }

        public async Task<object> GetFacultieSelect2ClientSide(ClaimsPrincipal user = null)
        {
            return await _coordinatorCareerRepository.GetFacultieSelect2ClientSide(user);
        }

        public async Task Insert(CoordinatorCareer coordinatorCareer)
        {
            await _coordinatorCareerRepository.Insert(coordinatorCareer);
        }

        public async Task Update(CoordinatorCareer coordinatorCareer)
        {
            await _coordinatorCareerRepository.Update(coordinatorCareer);
        }
    }
}

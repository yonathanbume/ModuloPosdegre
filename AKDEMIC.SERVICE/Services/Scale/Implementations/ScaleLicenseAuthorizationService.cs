using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Scale.Templates.ScaleLicenseAuthorization;
using AKDEMIC.SERVICE.Services.Scale.Interfaces;

namespace AKDEMIC.SERVICE.Services.Scale.Implementations
{
    public class ScaleLicenseAuthorizationService : IScaleLicenseAuthorizationService
    {
        private readonly IScaleLicenseAuthorizationRepository _scaleLicenseAuthorizationRepository;

        public ScaleLicenseAuthorizationService(IScaleLicenseAuthorizationRepository scaleLicenseAuthorizationRepository)
        {
            _scaleLicenseAuthorizationRepository = scaleLicenseAuthorizationRepository;
        }

        public async Task<ScaleLicenseAuthorization> Get(Guid id)
        {
            return await _scaleLicenseAuthorizationRepository.Get(id);
        }

        public async Task Insert(ScaleLicenseAuthorization entity)
        {
            await _scaleLicenseAuthorizationRepository.Insert(entity);
        }

        public async Task Update(ScaleLicenseAuthorization entity)
        {
            await _scaleLicenseAuthorizationRepository.Update(entity);
        }

        public async Task Delete(ScaleLicenseAuthorization entity)
        {
            await _scaleLicenseAuthorizationRepository.Delete(entity);
        }

        public async Task<int> GetLicenseAuthorizationsQuantity(string userId)
        {
            return await _scaleLicenseAuthorizationRepository.GetLicenseAuthorizationsQuantity(userId);
        }

        public async Task<List<ScaleLicenseAuthorization>> GetLicenseAuthorizationsByPaginationParameters(string userId, PaginationParameter paginationParameter)
        {
            return await _scaleLicenseAuthorizationRepository.GetLicenseAuthorizationsByPaginationParameters(userId, paginationParameter);
        }

        public async Task<Tuple<int, List<ScaleLicenseAuthorization>>> GetLicenseAuthorizationsReportByPaginationParameters(PaginationParameter paginationParameter)
        {
            return await _scaleLicenseAuthorizationRepository.GetLicenseAuthorizationsReportByPaginationParameters(paginationParameter);
        }

        public async Task<List<ScaleLicenseAuthorization>> GetLicenseAuthorizationsReport(string search)
        {
            return await _scaleLicenseAuthorizationRepository.GetLicenseAuthorizationsReport(search);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetLicensesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, ClaimsPrincipal claimPrincipal = null)
            => await _scaleLicenseAuthorizationRepository.GetLicensesDatatable(sentParameters,searchValue, claimPrincipal);

        public async Task<List<TeacherLicenseTemplate>> GetLicenseRecordReport(Guid facultyId)
            => await _scaleLicenseAuthorizationRepository.GetLicenseRecordReport(facultyId);

        public async Task<IEnumerable<ScaleLicenseAuthorization>> GetAllByUserIdAndRemunerateState(string userId,bool isRemunerated)
        {
            return await _scaleLicenseAuthorizationRepository.GetAllByUserIdAndRemunerateState(userId,isRemunerated);
        }

        public async Task<IEnumerable<ScaleLicenseAuthorization>> GetAllByUserId(string userId)
            => await _scaleLicenseAuthorizationRepository.GetAllByUserId(userId);

        public Task<int> GetTotalLicenseTimeByUser(string userId)
            => _scaleLicenseAuthorizationRepository.GetTotalLicenseTimeByUser(userId);
    }
}

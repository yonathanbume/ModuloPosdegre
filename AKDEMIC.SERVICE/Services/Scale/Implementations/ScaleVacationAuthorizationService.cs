using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using AKDEMIC.SERVICE.Services.Scale.Interfaces;

namespace AKDEMIC.SERVICE.Services.Scale.Implementations
{
    public class ScaleVacationAuthorizationService : IScaleVacationAuthorizationService
    {
        private readonly IScaleVacationAuthorizationRepository _scaleVacationAuthorizationRepository;

        public ScaleVacationAuthorizationService(IScaleVacationAuthorizationRepository scaleVacationAuthorizationRepository)
        {
            _scaleVacationAuthorizationRepository = scaleVacationAuthorizationRepository;
        }

        public async Task<ScaleVacationAuthorization> Get(Guid id)
        {
            return await _scaleVacationAuthorizationRepository.Get(id);
        }

        public async Task Insert(ScaleVacationAuthorization entity)
        {
            await _scaleVacationAuthorizationRepository.Insert(entity);
        }

        public async Task Update(ScaleVacationAuthorization entity)
        {
            await _scaleVacationAuthorizationRepository.Update(entity);
        }

        public async Task Delete(ScaleVacationAuthorization entity)
        {
            await _scaleVacationAuthorizationRepository.Delete(entity);
        }

        public async Task<int> GetVacationAuthorizationsQuantity(string userId)
        {
            return await _scaleVacationAuthorizationRepository.GetVacationAuthorizationsQuantity(userId);
        }

        public async Task<List<ScaleVacationAuthorization>> GetVacationAuthorizationsByPaginationParameters(string userId, PaginationParameter paginationParameter)
        {
            return await _scaleVacationAuthorizationRepository.GetVacationAuthorizationsByPaginationParameters(userId, paginationParameter);
        }

        public async Task<Tuple<int, List<ScaleVacationAuthorization>>> GetVacationAuthorizationsReportByPaginationParameters(PaginationParameter paginationParameter, ClaimsPrincipal user = null)
        {
            return await _scaleVacationAuthorizationRepository.GetVacationAuthorizationsReportByPaginationParameters(paginationParameter,user);
        }

        public async Task<List<ScaleVacationAuthorization>> GetVacationAuthorizationsReport(string search, ClaimsPrincipal user = null)
        {
            return await _scaleVacationAuthorizationRepository.GetVacationAuthorizationsReport(search,user);
        }
    }
}

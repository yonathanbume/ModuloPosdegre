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
    public class ScalePermitAuthorizationService : IScalePermitAuthorizationService
    {
        private readonly IScalePermitAuthorizationRepository _scalePermitAuthorizationRepository;

        public ScalePermitAuthorizationService(IScalePermitAuthorizationRepository scalePermitAuthorizationRepository)
        {
            _scalePermitAuthorizationRepository = scalePermitAuthorizationRepository;
        }

        public async Task<ScalePermitAuthorization> Get(Guid id)
        {
            return await _scalePermitAuthorizationRepository.Get(id);
        }

        public async Task Insert(ScalePermitAuthorization entity)
        {
            await _scalePermitAuthorizationRepository.Insert(entity);
        }

        public async Task Update(ScalePermitAuthorization entity)
        {
            await _scalePermitAuthorizationRepository.Update(entity);
        }

        public async Task Delete(ScalePermitAuthorization entity)
        {
            await _scalePermitAuthorizationRepository.Delete(entity);
        }

        public async Task<int> GetPermitAuthorizationsQuantity(string userId)
        {
            return await _scalePermitAuthorizationRepository.GetPermitAuthorizationsQuantity(userId);
        }

        public async Task<List<ScalePermitAuthorization>> GetPermitAuthorizationsByPaginationParameters(string userId, PaginationParameter paginationParameter)
        {
            return await _scalePermitAuthorizationRepository.GetPermitAuthorizationsByPaginationParameters(userId, paginationParameter);
        }

        public async Task<Tuple<int, List<ScalePermitAuthorization>>> GetPermitAuthorizationsReportByPaginationParameters(PaginationParameter paginationParameter, ClaimsPrincipal user = null)
        {
            return await _scalePermitAuthorizationRepository.GetPermitAuthorizationsReportByPaginationParameters(paginationParameter,user);
        }

        public async Task<List<ScalePermitAuthorization>> GetPermitAuthorizationsReport(string search, ClaimsPrincipal user = null)
        {
            return await _scalePermitAuthorizationRepository.GetPermitAuthorizationsReport(search,user);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Scale.Templates.Resolution;
using AKDEMIC.SERVICE.Services.Scale.Interfaces;

namespace AKDEMIC.SERVICE.Services.Scale.Implementations
{
    public class ScaleResolutionService : IScaleResolutionService
    {
        private readonly IScaleResolutionRepository _scaleResolutionRepository;

        public ScaleResolutionService(IScaleResolutionRepository scaleResolutionRepository)
        {
            _scaleResolutionRepository = scaleResolutionRepository;
        }

        public async Task<ScaleResolution> Get(Guid scaleResolutionId)
        {
            return await _scaleResolutionRepository.Get(scaleResolutionId);
        }

        public async Task Add(ScaleResolution scaleResolution)
        {
            await _scaleResolutionRepository.Add(scaleResolution);
        }

        public async Task Insert(ScaleResolution scaleResolution)
        {
            await _scaleResolutionRepository.Insert(scaleResolution);
        }

        public async Task Update(ScaleResolution scaleResolution)
        {
            await _scaleResolutionRepository.Update(scaleResolution);
        }

        public async Task Delete(ScaleResolution scaleResolution)
        {
            await _scaleResolutionRepository.Delete(scaleResolution);
        }

        public async Task<int> GetScaleResolutionsQuantityBySectionResolutionType(Guid sectionResolutionTypeId)
        {
            return await _scaleResolutionRepository.GetScaleResolutionsQuantityBySectionResolutionType(sectionResolutionTypeId);
        }

        public async Task<int> GetScaleResolutionsQuantity(string userId, Guid sectionId, Guid resolutionTypeId, string search)
        {
            return await _scaleResolutionRepository.GetScaleResolutionsQuantity(userId, sectionId, resolutionTypeId, search);
        }

        public async Task<List<ScaleResolutionInvestigationTemplate>> GetInvestigationScaleResolutionsByPaginationParameters(string userId, Guid sectionId, Guid resolutionTypeId, string search, PaginationParameter paginationParameter)
        {
            return await _scaleResolutionRepository.GetInvestigationScaleResolutionsByPaginationParameters(userId,sectionId,resolutionTypeId,search,paginationParameter);
        }

        public async Task<List<ScaleResolution>> GetScaleResolutionsByPaginationParameters(string userId, Guid sectionId, Guid resolutionTypeId, string search, PaginationParameter paginationParameter)
        {
            return await _scaleResolutionRepository.GetScaleResolutionsByPaginationParameters(userId, sectionId, resolutionTypeId, search, paginationParameter);
        }

        public async Task<List<ScaleResolution>> GetScaleResolutionsBySectionUserId(string userId, Guid sectionId)
        {
            return await _scaleResolutionRepository.GetScaleResolutionsBySectionUserId(userId, sectionId);
        }

        public async Task<List<ScaleResolution>> GetQuinquenniumResolutionsBySectionUserId(string userId, Guid sectionId)
        {
            return await _scaleResolutionRepository.GetQuinquenniumResolutionsBySectionUserId(userId, sectionId);
        }

        public async Task<Tuple<int, List<Tuple<ApplicationUser, int>>>> GetUserResolutionsQuantityReportByPaginationParameters(PaginationParameter paginationParameter , Guid? dedicationId = null , Guid? conditionId = null)
        {
            return await _scaleResolutionRepository.GetUserResolutionsQuantityReportByPaginationParameters(paginationParameter,dedicationId,conditionId);
        }

        public async Task<List<Tuple<ApplicationUser, int>>> GetUserResolutionsQuantityReport(string search)
        {
            return await _scaleResolutionRepository.GetUserResolutionsQuantityReport(search);
        }

        public async Task<List<ScaleResolution>> GetContractsResolutionsBySectionUserId(string userId, Guid sectionId)
        {
            return await _scaleResolutionRepository.GetContractsResolutionsBySectionUserId(userId, sectionId);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllTeacherReportDatatable(DataTablesStructs.SentParameters sentParameters, string startDate = null, string endDate = null, Guid? scaleResolutionTypeId = null)
            => await _scaleResolutionRepository.GetAllTeacherReportDatatable(sentParameters,startDate,endDate,scaleResolutionTypeId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllUserReportDatatable(DataTablesStructs.SentParameters sentParameters, string userId = null)
            => await _scaleResolutionRepository.GetAllUserReportDatatable(sentParameters,userId);

        public async Task<string> GetWokerStatusDescriptionBySectionNumber(byte contracts)
        {
            return await _scaleResolutionRepository.GetWokerStatusDescriptionBySectionNumber(contracts);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetContractsByUserDatatable(DataTablesStructs.SentParameters sentParameters, string userId)
            => await _scaleResolutionRepository.GetContractsByUserDatatable(sentParameters,userId);

        public async Task<bool> DeleteBySection(Guid Id, byte sectionNumber) => await _scaleResolutionRepository.DeleteBySection(Id,sectionNumber);

        public Task<DataTablesStructs.ReturnedData<object>> GetBenefitsByUserDatatable(DataTablesStructs.SentParameters sentParameters, string userId)
            => _scaleResolutionRepository.GetBenefitsByUserDatatable(sentParameters,userId);
    }
}

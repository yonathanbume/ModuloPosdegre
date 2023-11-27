using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Implementations
{
    public class RequirementService : IRequirementService
    {
        private readonly IRequirementRepository _requirementRepository;

        public RequirementService(IRequirementRepository requirementRepository)
        {
            _requirementRepository = requirementRepository;
        }

        public async Task<int> Count()
        {
            return await _requirementRepository.Count();
        }

        public async Task<Requirement> Get(Guid id)
        {
            return await _requirementRepository.Get(id);
        }

        public async Task<List<Requirement>> GetList(Guid id)
            => await _requirementRepository.GetList(id);

        public async Task<DataTablesStructs.ReturnedData<object>> GetRequirementOrderDatatable(DataTablesStructs.SentParameters sentParameters,int userRequirementIndex, string searchValue = null, int? filterValue = null)
        {
            return await _requirementRepository.GetRequirementOrderDatatable(sentParameters , userRequirementIndex,searchValue, filterValue);
        }

        public async Task<object> GetRequirementReportByDependencyAndOrderTypeChart(Guid? centerId = null, int? type = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            return await _requirementRepository.GetRequirementReportByDependencyAndOrderTypeChart(centerId, type, startDate, endDate);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetRequirementReportByDependencyAndOrderTypeDatatable(DataTablesStructs.SentParameters sentParameters, Guid? centerId = null, int? type = null, DateTime? startDate = null, DateTime? endDate  = null)
        {
            return await _requirementRepository.GetRequirementReportByDependencyAndOrderTypeDatatable(sentParameters, centerId, type, startDate, endDate);
        }

        public async Task<object> GetRequirementReportByFundingSourceAndDependencyChart()
        {
            return await _requirementRepository.GetRequirementReportByFundingSourceAndDependencyChart();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetRequirementReportByFundingSourceAndDependencyDatatable(DataTablesStructs.SentParameters sentParameters)
        {
            return await _requirementRepository.GetRequirementReportByFundingSourceAndDependencyDatatable(sentParameters);
        }

        public async Task<DataTablesStructs.ReturnedData<Requirement>> GetRequirementsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await _requirementRepository.GetRequirementsDatatable(sentParameters, searchValue);
        }

        public async Task<UserRequirement> GetWithIncludes(Guid id)
        {
            return await _requirementRepository.GetWithIncludes(id);
        }

        public async Task Insert(Requirement requirement) =>
            await _requirementRepository.Insert(requirement);

        public async Task Update(Requirement requirement) =>
            await _requirementRepository.Update(requirement);

        public async Task<List<UserRequirement>> GetOrderDetailIncludeOrderUser(Guid id)
            => await _requirementRepository.GetOrderDetailIncludeOrderUser(id);
        public async Task<Requirement> GetRequirementWithDataById(Guid id)
            => await _requirementRepository.GetRequirementWithDataById(id);
        public async Task<object> GetRequirementDetail(Guid id)
            => await _requirementRepository.GetRequirementDetail(id);
        public async Task<bool> AnyAsync(string codeNumber)
            => await _requirementRepository.AnyAsync(codeNumber);
        public async Task AddAsync(Requirement requirement)
            => await _requirementRepository.Add(requirement);
        public async Task<bool> FindByCodeNumberAndCodeNumber(string codeNumber, string codeNumber2)
            => await _requirementRepository.FindByCodeNumberAndCodeNumber(codeNumber, codeNumber2);
    }
}

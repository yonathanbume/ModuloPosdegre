using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Investigation;
using AKDEMIC.REPOSITORY.Repositories.Investigation.Interfaces;
using AKDEMIC.SERVICE.Services.Investigation.Interfaces;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Investigation.Implementations
{
    public class CompanyInterestService : ICompanyInterestService
    {
        private readonly ICompanyInterestRepository _companyInterestRepository;

        public CompanyInterestService(ICompanyInterestRepository companyInterestRepository)
        {
            _companyInterestRepository = companyInterestRepository;
        }

        public async Task<CompanyInterest> GetByCompanyIdAndProjectId(Guid companyId, Guid projectId)
            => await _companyInterestRepository.GetByCompanyIdAndProjectId(companyId, projectId);

        public async Task<DataTablesStructs.ReturnedData<CompanyInterest>> GetCompanyInterestDatatable(DataTablesStructs.SentParameters sentParameters, string company, string startDate, string endDate, Guid? lineId)
            => await _companyInterestRepository.GetCompanyInterestDatatable(sentParameters, company, startDate, endDate, lineId);

        public async Task<DataTablesStructs.ReturnedData<CompanyInterest>> GetCompanyInterestProjectDatatable(DataTablesStructs.SentParameters sentParameters, Guid projectId)
            => await _companyInterestRepository.GetCompanyInterestProjectDatatable(sentParameters, projectId);

        public async Task Insert(CompanyInterest entity)
            => await _companyInterestRepository.Insert(entity);
    }
}

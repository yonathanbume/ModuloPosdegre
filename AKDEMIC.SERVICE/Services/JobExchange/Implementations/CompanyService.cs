using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Templates;
using AKDEMIC.SERVICE.Services.JobExchange.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.JobExchange.Implementations
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;

        public CompanyService(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task<bool> AnyCompanyExistByName(Guid companyId, string Name)
        {
            return await _companyRepository.AnyCompanyExistByName(companyId, Name);
        }

        public async Task<bool> AnyCompanyExistByRUC(Guid companyId, string RUC)
        {
            return await _companyRepository.AnyCompanyExistByRUC(companyId, RUC);
        }

        public async Task<object> CompaniesReport1(byte type, List<Guid> careers)
        {
            return await _companyRepository.CompaniesReport1(type, careers);
        }

        public async Task<object> CompaniesReport2(bool isCoordinator, List<Guid> careers)
        {
            return await _companyRepository.CompaniesReport2(isCoordinator, careers);
        }

        public async Task<decimal> CompaniesReport3(bool isCoordinator, List<Guid> careers, Guid? companyId)
        {
            return await _companyRepository.CompaniesReport3(isCoordinator, careers, companyId);
        }

        public async Task<Company> Get(Guid id)
        {
            return await _companyRepository.Get(id);
        }

        public async Task<IEnumerable<Company>> GetAllWithIncludes()
        {
            return await _companyRepository.GetAllWithIncludes();
        }

        public async Task Update(Company company)
        {
            await _companyRepository.Update(company);
        }

        public async Task<Company> GetByUserId(string userId)
        {
            return await _companyRepository.GetByUserId(userId);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetCompaniesDatatable(DataTablesStructs.SentParameters sentParameters, bool IsExternalRequest, string searchValue = null)
        {
            return await _companyRepository.GetCompaniesDatatable(sentParameters, IsExternalRequest, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetCompaniesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await _companyRepository.GetCompaniesDatatable(sentParameters, searchValue);
        }

        public async Task<Company> GetWithIncludes(Guid id)
        {
            return await _companyRepository.GetWithIncludes(id);
        }

        public async Task<bool> AnyByRUC(string RUC, Guid? id = null)
        {
            return await _companyRepository.AnyByRUC(RUC, id);
        }

        public async Task Insert(Company company)
        {
            await _companyRepository.Insert(company);
        }

        public async Task<object> GetCompaniesSelect2()
        {
            return await _companyRepository.GetCompaniesSelect2();
        }

        public async Task<DataTablesStructs.ReturnedData<Company>> GetCompaniesDatatable(DataTablesStructs.SentParameters sentParameters, Guid? sectorId = null, string searchValue = null)
        {
            return await _companyRepository.GetCompaniesDatatable(sentParameters, sectorId, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllCompaniesDatatable(DataTablesStructs.SentParameters sentParameters, Guid? sectorId, Guid? lineId, string company)
        {
            return await _companyRepository.GetAllCompaniesDatatable(sentParameters, sectorId, lineId, company);
        }

        public IQueryable<Company> GetQueryable()
        {
            return _companyRepository.GetQueryable();
        }

        public async Task<object> CustomDetail(Guid id)
        {
            return await _companyRepository.CustomDetail(id);
        }

        public async Task<object> GetCompaniesByUserSelect2ClientSide(ClaimsPrincipal user)
        {
            return await _companyRepository.GetCompaniesByUserSelect2ClientSide(user);
        }

        public async Task<object> GetCompaniesByStudentExperiencesUserSelect2ClientSide(string userId)
        {
            return await _companyRepository.GetCompaniesByStudentExperiencesUserSelect2ClientSide(userId);
        }

        public async Task<object> GetCompaniesByStudentJobOfferApplicationAceptedSelect2ClientSide(string userId)
        {
            return await _companyRepository.GetCompaniesByStudentJobOfferApplicationAceptedSelect2ClientSide(userId);
        }

        public async Task<bool> GetByCompanySize(Guid companySizeId)
        {
            return await _companyRepository.GetByCompanySize(companySizeId);
        }

        public async Task<bool> GetByCompanyType(Guid companyTypeId)
        {
            return await _companyRepository.GetByCompanyType(companyTypeId);
        }

        public async Task<bool> GetBySector(Guid sectorId)
        {
            return await _companyRepository.GetBySector(sectorId);
        }

        public async Task<bool> GetByEconomicActivity(Guid economicActivityId)
        {
            return await _companyRepository.GetByEconomicActivity(economicActivityId);
        }

        public Task<object> GetCompaniesWithOutAgreement()
            => _companyRepository.GetCompaniesWithOutAgreement();

        public Task<IEnumerable<Company>> GetAll()
            => _companyRepository.GetAll();

        public Task<IEnumerable<Company>> GetAllWithOutAgreement()
            => _companyRepository.GetAllWithOutAgreement();

        public Task Save()
            => _companyRepository.Save();

        public Task<Company> GetCompanyByUserId(string userId, bool isExternalRequest = false)
            => _companyRepository.GetCompanyByUserId(userId,isExternalRequest);

        public Task<Select2Structs.ResponseParameters> GetCompaniesSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null)
            => _companyRepository.GetCompaniesSelect2(requestParameters, searchValue);

        public Task<Company> GetByRuc(string ruc)
            => _companyRepository.GetByRuc(ruc);

        public Task<bool> IsExternalRequest(string userId)
            => _companyRepository.IsExternalRequest(userId);

        public Task<List<CompanyTemplate>> GetAllCompanyTemplateData(bool? isExternalRequest = null)
            => _companyRepository.GetAllCompanyTemplateData(isExternalRequest);

        public Task<DataTablesStructs.ReturnedData<object>> GetHomePendingCompaniesDatatable(DataTablesStructs.SentParameters sentParameters)
            => _companyRepository.GetHomePendingCompaniesDatatable(sentParameters);

        public Task<object> GetJobExchangeJobOfferReportChart(int jobOfferStatus = 0, List<Guid> careers = null)
            => _companyRepository.GetJobExchangeJobOfferReportChart(jobOfferStatus, careers);

        public Task<DataTablesStructs.ReturnedData<object>> GetJobExchangeJobOfferReportDatatable(DataTablesStructs.SentParameters sentParameters, int jobOfferStatus = 0, List<Guid> careers = null)
            => _companyRepository.GetJobExchangeJobOfferReportDatatable(sentParameters, jobOfferStatus, careers);

        public Task<object> GetJobExchangeJobOfferApplicationReportChart()
            => _companyRepository.GetJobExchangeJobOfferApplicationReportChart();

        public Task<DataTablesStructs.ReturnedData<object>> GetJobExchangeJobOfferApplicationReportDatatable(DataTablesStructs.SentParameters sentParameters)
            => _companyRepository.GetJobExchangeJobOfferApplicationReportDatatable(sentParameters);
        public Task<decimal> GetJobExchangeJobOfferIndicatorReport(Guid? companyId = null)
            => _companyRepository.GetJobExchangeJobOfferIndicatorReport(companyId);
    }

}

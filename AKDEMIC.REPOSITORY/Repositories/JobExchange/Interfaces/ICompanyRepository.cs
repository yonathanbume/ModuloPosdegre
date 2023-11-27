using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces
{
    public interface ICompanyRepository : IRepository<Company>
    {
        Task<IEnumerable<Company>> GetAllWithIncludes();
        Task<Company> GetWithIncludes(Guid id);
        Task<bool> IsExternalRequest(string userId);
        Task<List<CompanyTemplate>> GetAllCompanyTemplateData(bool? isExternalRequest = null);
        Task<Company> GetByRuc(string ruc);
        Task<Company> GetCompanyByUserId(string userId, bool isExternalRequest = false);
        Task<IEnumerable<Company>> GetAllWithOutAgreement();
        Task Save();
        Task<object> CustomDetail(Guid id);
        IQueryable<Company> GetQueryable();
        Task<Company> GetByUserId(string userId);
        Task<object> CompaniesReport1(byte type, List<Guid> careers);
        Task<object> CompaniesReport2(bool isCoordinator, List<Guid> careers);
        Task<decimal> CompaniesReport3(bool isCoordinator, List<Guid> careers, Guid? companyId);
        Task<bool> AnyCompanyExistByName(Guid companyId, string Name);
        Task<bool> AnyCompanyExistByRUC(Guid companyId, string RUC);
        Task<object> GetCompaniesSelect2();
        Task<DataTablesStructs.ReturnedData<Company>> GetCompaniesDatatable(DataTablesStructs.SentParameters sentParameters, Guid? sectorId = null, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetAllCompaniesDatatable(DataTablesStructs.SentParameters sentParameters, Guid? sectorId, Guid? lineId, string company);
        Task<DataTablesStructs.ReturnedData<object>> GetCompaniesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<bool> AnyByRUC(string RUC, Guid? id = null);
        Task<DataTablesStructs.ReturnedData<object>> GetCompaniesDatatable(DataTablesStructs.SentParameters sentParameters, bool IsExternalRequest, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetHomePendingCompaniesDatatable(DataTablesStructs.SentParameters sentParameters);
        Task<object> GetCompaniesByUserSelect2ClientSide(ClaimsPrincipal user);
        Task<object> GetCompaniesByStudentExperiencesUserSelect2ClientSide(string userId);
        Task<object> GetCompaniesByStudentJobOfferApplicationAceptedSelect2ClientSide(string userId);
        Task<object> GetCompaniesWithOutAgreement();
        Task<bool> GetByCompanySize(Guid companySizeId);
        Task<bool> GetByCompanyType(Guid companyTypeId);
        Task<bool> GetBySector(Guid sectorId);
        Task<bool> GetByEconomicActivity(Guid economicActivityId);
        Task<Select2Structs.ResponseParameters> GetCompaniesSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null);
        Task<object> GetJobExchangeJobOfferReportChart(int jobOfferStatus = 0, List<Guid> careers = null);
        Task<DataTablesStructs.ReturnedData<object>> GetJobExchangeJobOfferReportDatatable(DataTablesStructs.SentParameters sentParameters, int jobOfferStatus = 0, List<Guid> careers = null);

        Task<object> GetJobExchangeJobOfferApplicationReportChart();
        Task<DataTablesStructs.ReturnedData<object>> GetJobExchangeJobOfferApplicationReportDatatable(DataTablesStructs.SentParameters sentParameters);
        Task<decimal> GetJobExchangeJobOfferIndicatorReport(Guid? companyId = null);
    }
}

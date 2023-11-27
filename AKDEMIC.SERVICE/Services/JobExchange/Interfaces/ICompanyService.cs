using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.JobExchange.Interfaces
{
    public interface ICompanyService
    {
        Task Insert(Company entity);
        Task<bool> IsExternalRequest(string userId);
        Task<List<CompanyTemplate>> GetAllCompanyTemplateData(bool? isExternalRequest = null); //Solicitud externa son los pendientes por aceptar pero registrados
        Task<Company> Get(Guid id);
        Task<Company> GetByRuc(string ruc);
        Task<object> CustomDetail(Guid id);
        IQueryable<Company> GetQueryable();
        Task<Company> GetWithIncludes(Guid id);
        Task<Company> GetCompanyByUserId(string userId, bool isExternalRequest = false);
        Task<Company> GetByUserId(string userId);
        Task<IEnumerable<Company>> GetAll();
        Task<IEnumerable<Company>> GetAllWithOutAgreement();
        Task<IEnumerable<Company>> GetAllWithIncludes();
        Task<object> CompaniesReport1(byte type, List<Guid> careers);
        Task<object> CompaniesReport2(bool isCoordinator, List<Guid> careerId);
        Task<decimal> CompaniesReport3(bool isCoordinator, List<Guid> careerId, Guid? companyId);
        Task<bool> AnyCompanyExistByRUC(Guid companyId, string RUC);
        Task<bool> AnyCompanyExistByName(Guid companyId, string Name);
        Task<bool> AnyByRUC(string RUC, Guid? id = null);
        Task Update(Company company);
        Task Save();
        Task<DataTablesStructs.ReturnedData<object>> GetCompaniesDatatable(DataTablesStructs.SentParameters sentParameters, bool IsExternalRequest, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetHomePendingCompaniesDatatable(DataTablesStructs.SentParameters sentParameters);
        Task<DataTablesStructs.ReturnedData<object>> GetCompaniesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<object> GetCompaniesSelect2();
        Task<DataTablesStructs.ReturnedData<object>> GetAllCompaniesDatatable(DataTablesStructs.SentParameters sentParameters, Guid? sectorId, Guid? lineId, string company);
        Task<DataTablesStructs.ReturnedData<Company>> GetCompaniesDatatable(DataTablesStructs.SentParameters sentParameters, Guid? sectorId = null, string searchValue = null);
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

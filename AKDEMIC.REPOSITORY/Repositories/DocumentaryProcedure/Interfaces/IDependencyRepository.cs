using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Template;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Templates.Dependency;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces
{
    public interface IDependencyRepository : IRepository<Dependency>
    {
        Task<List<Dependency>> GetDependenciesByFaculty(Guid facultyId);
        Task Update();
        Task<object> GetAllAsSelect2ClientSide();
        Task<bool> AnyDependencyByName(string name, Guid? ignoredId = null);
        Task<IEnumerable<Dependency>> GetDependencies();
        Task<IEnumerable<Dependency>> GetDependenciesByProcedure(Guid procedureId);
        Task<IEnumerable<Dependency>> GetDependenciesByUserDependencyUser(string userDependencyUserId);
        Task<DataTablesStructs.ReturnedData<Dependency>> GetDependenciesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<Dependency>> GetDependenciesDatatableByUser(DataTablesStructs.SentParameters sentParameters, string userId, string searchValue = null);
        Task<List<DependencyExcelTemplate>> GetDependenciesDatatableExcel(string userId = null, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<Dependency>> GetProcedureDependenciesDatatableByProcedure(DataTablesStructs.SentParameters sentParameters, Guid procedureId, string searchValue = null);
        
        Task<Select2Structs.ResponseParameters> GetDependenciesSelect2ByUser(Select2Structs.RequestParameters requestParameters, string userId, string searchValue = null);
        Task<Select2Structs.ResponseParameters> GetExternalProcedureDependenciesSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null);
        Task<Select2Structs.ResponseParameters> GetProcedureDependenciesSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null);
        Task<Select2Structs.ResponseParameters> GetUserInternalProcedureDependenciesSelect2ByInternalProcedure(Select2Structs.RequestParameters requestParameters, Guid internalProcedureId, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetReportBalanceByCostOfCenterDatatable(DataTablesStructs.SentParameters sentParameters);
        Task<List<BalanceByCostOfCenterTemplate>> GetReportBalanceByCostOfCenter();
        Task<DataTablesStructs.ReturnedData<object>> GetReportIncomeByCostOfCenterDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetDependencyDatatableReport(DataTablesStructs.SentParameters sentParameters, string search, DateTime? startDate = null, DateTime? endDate = null, bool showAll = false);
        Task<decimal> GetDependencyDatatableReportTotalAmount(string search, DateTime? startDate = null, DateTime? endDate = null);
        Task<DataTablesStructs.ReturnedData<object>> GetDependencyDatatableReportManual(DataTablesStructs.SentParameters sentParameters, int? year = null);
        Task<object> GetBudgetBalance();
        Task<List<Dependency>> GetAllWithData();
        Task<Dependency> GetDependencyByAcronym(string acronym);
        Task<object> GetCostCenterSelect();
        Task<object> GetDependenciesJson();
        Task<Select2Structs.ResponseParameters> GetDependenciesSelect2Configuration(Select2Structs.RequestParameters requestParameters, List<Guid> userDependencies = null, bool derivationConfiguration = false, string searchValue = null, Guid? userDependencyId = null);
        Task<Select2Structs.ResponseParameters> GetDependenciesSelect2(Select2Structs.RequestParameters requestParameters, string searchValue);
        Task<Select2Structs.ResponseParameters> GetDependeciesHigher(Select2Structs.RequestParameters requestParameters, string searchValue = null);
        Task<object> GetDependeciesAllJson();

        Task<DataTablesStructs.ReturnedData<object>> GetDependenciesObjectDatatable(DataTablesStructs.SentParameters sentParameters, string search = null);
        Task<Dependency> GetDependencyByName(string name);
        Task<bool> AnyDependencyByAcronym(string acronym);
        Task<bool> AnyDependencyByAcronymIgnoreQueryFilters(string acronym);
        Task<DataTablesStructs.ReturnedData<object>> GetBudgetBalanceReport(Guid? dependencyId = null, DateTime? month = null, ClaimsPrincipal user = null);
        Task<decimal> GetAvailableAmount(Guid dependencyId);
        Task<DataTablesStructs.ReturnedData<object>> GetBudgetBalanceKardexReport(Guid? dependencyId = null, DateTime? startDate = null, DateTime? endDate = null, ClaimsPrincipal user = null);
        Task<List<UserDependency>> GetByUser(string userId);
        Task<Dependency> GetHierarchicalTree(Guid dependencyId);
        Task<List<Dependency>> GetDependenciesByCareer(Guid careerId);
        Task<Select2Structs.ResponseParameters> GetDependencyDirectorsSelect2(Select2Structs.RequestParameters requestParameters, string search);
    }
}
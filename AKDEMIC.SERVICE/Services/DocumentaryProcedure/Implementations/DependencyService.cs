using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Template;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Templates.Dependency;
using AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Implementations
{
    public class DependencyService : IDependencyService
    {
        private readonly IDependencyRepository _dependencyRepository;

        public DependencyService(IDependencyRepository dependencyRepository)
        {
            _dependencyRepository = dependencyRepository;
        }

        public async Task<bool> AnyDependencyByName(string name, Guid? ignoredId = null)
        {
            return await _dependencyRepository.AnyDependencyByName(name, ignoredId);
        }

        public async Task<bool> AnyDependencyByAcronymIgnoreQueryFilters(string acronym)
        {
            return await _dependencyRepository.AnyDependencyByAcronymIgnoreQueryFilters(acronym);
        }
        
        public async Task<int> Count()
        {
            return await _dependencyRepository.Count();
        }

        public async Task<Dependency> Get(Guid id)
        {
            return await _dependencyRepository.Get(id);
        }

        public async Task<IEnumerable<Dependency>> GetAll()
        {
            return await _dependencyRepository.GetAll();
        }

        public async Task<IEnumerable<Dependency>> GetDependencies()
        {
            return await _dependencyRepository.GetDependencies();
        }

        public async Task<IEnumerable<Dependency>> GetDependenciesByProcedure(Guid procedureId)
        {
            return await _dependencyRepository.GetDependenciesByProcedure(procedureId);
        }

        public async Task<IEnumerable<Dependency>> GetDependenciesByUserDependencyUser(string userDependencyUserId)
        {
            return await _dependencyRepository.GetDependenciesByUserDependencyUser(userDependencyUserId);
        }

        public async Task<DataTablesStructs.ReturnedData<Dependency>> GetDependenciesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await _dependencyRepository.GetDependenciesDatatable(sentParameters, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<Dependency>> GetDependenciesDatatableByUser(DataTablesStructs.SentParameters sentParameters, string userId, string searchValue = null)
        {
            return await _dependencyRepository.GetDependenciesDatatableByUser(sentParameters, userId, searchValue);
        }
        public async Task<List<DependencyExcelTemplate>> GetDependenciesDatatableExcel(string userId = null, string searchValue = null)
            => await _dependencyRepository.GetDependenciesDatatableExcel(userId, searchValue);
        public async Task<DataTablesStructs.ReturnedData<Dependency>> GetProcedureDependenciesDatatableByProcedure(DataTablesStructs.SentParameters sentParameters, Guid procedureId, string searchValue = null)
        {
            return await _dependencyRepository.GetProcedureDependenciesDatatableByProcedure(sentParameters, procedureId, searchValue);
        }

        public async Task<Select2Structs.ResponseParameters> GetDependenciesSelect2Configuration(Select2Structs.RequestParameters requestParameters, List<Guid> userDependencies = null, bool derivationTypeConfiguration = false, string searchValue = null, Guid? userDependencyId = null)
        {
            return await _dependencyRepository.GetDependenciesSelect2Configuration(requestParameters, userDependencies, derivationTypeConfiguration, searchValue, userDependencyId);
        }

        public async Task<Select2Structs.ResponseParameters> GetDependenciesSelect2ByUser(Select2Structs.RequestParameters requestParameters, string userId, string searchValue = null)
        {
            return await _dependencyRepository.GetDependenciesSelect2ByUser(requestParameters, userId, searchValue);
        }

        public async Task<Select2Structs.ResponseParameters> GetExternalProcedureDependenciesSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null)
        {
            return await _dependencyRepository.GetExternalProcedureDependenciesSelect2(requestParameters, searchValue);
        }

        public async Task<Select2Structs.ResponseParameters> GetProcedureDependenciesSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null)
        {
            return await _dependencyRepository.GetProcedureDependenciesSelect2(requestParameters, searchValue);
        }

        public async Task<Select2Structs.ResponseParameters> GetUserInternalProcedureDependenciesSelect2ByInternalProcedure(Select2Structs.RequestParameters requestParameters, Guid internalProcedureId, string searchValue = null)
        {
            return await _dependencyRepository.GetUserInternalProcedureDependenciesSelect2ByInternalProcedure(requestParameters, internalProcedureId, searchValue);
        }

        public async Task Delete(Dependency dependency)
        {
            await _dependencyRepository.Delete(dependency);
        }

        public async Task Insert(Dependency dependency)
        {
            await _dependencyRepository.Insert(dependency);
        }

        public async Task Update(Dependency dependency)
        {
            await _dependencyRepository.Update(dependency);
        }

        public async Task<object> GetAllAsSelect2ClientSide()
            => await _dependencyRepository.GetAllAsSelect2ClientSide();

        public async Task<DataTablesStructs.ReturnedData<object>> GetReportBalanceByCostOfCenterDatatable(DataTablesStructs.SentParameters sentParameters)
        {
            return await _dependencyRepository.GetReportBalanceByCostOfCenterDatatable(sentParameters);
        }
        public async Task<List<BalanceByCostOfCenterTemplate>> GetReportBalanceByCostOfCenter()
            => await _dependencyRepository.GetReportBalanceByCostOfCenter();
        public async Task<DataTablesStructs.ReturnedData<object>> GetReportIncomeByCostOfCenterDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await _dependencyRepository.GetReportIncomeByCostOfCenterDatatable(sentParameters ,searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDependencyDatatableReport(DataTablesStructs.SentParameters sentParameters, string search, DateTime? startDate = null, DateTime? endDate = null, bool showAll = false)
            => await _dependencyRepository.GetDependencyDatatableReport(sentParameters, search, startDate, endDate, showAll);
        public async Task<DataTablesStructs.ReturnedData<object>> GetDependencyDatatableReportManual(DataTablesStructs.SentParameters sentParameters, int? year = null)
            => await _dependencyRepository.GetDependencyDatatableReportManual(sentParameters, year);
        public async Task<object> GetBudgetBalance()
            => await _dependencyRepository.GetBudgetBalance();
        public async Task<List<Dependency>> GetAllWithData()
            => await _dependencyRepository.GetAllWithData();

        public async Task<Dependency> GetDependencyByAcronym(string acronym)
            => await _dependencyRepository.GetDependencyByAcronym(acronym);
        public async Task<object> GetCostCenterSelect()
            => await _dependencyRepository.GetCostCenterSelect();
        public async Task<object> GetDependenciesJson()
            => await _dependencyRepository.GetDependenciesJson();
        public async Task<object> GetDependeciesAllJson()
            => await _dependencyRepository.GetDependeciesAllJson();

        public async Task<bool> Any(Guid id)
            => await _dependencyRepository.Any(id);

        public async Task<DataTablesStructs.ReturnedData<object>> GetDependenciesObjectDatatable(DataTablesStructs.SentParameters sentParameters, string search = null)
            => await _dependencyRepository.GetDependenciesObjectDatatable(sentParameters, search);

        public async Task InsertRange(IEnumerable<Dependency> dependencies)
            => await _dependencyRepository.InsertRange(dependencies);

        public async Task Add(Dependency dependency)
            => await _dependencyRepository.Add(dependency);

        public async Task Update()
            => await _dependencyRepository.Update();

        public async Task<Select2Structs.ResponseParameters> GetDependenciesSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null)
        {
            return await _dependencyRepository.GetDependenciesSelect2(requestParameters, searchValue);
        }
        public async Task<Select2Structs.ResponseParameters> GetDependeciesHigher(Select2Structs.RequestParameters requestParameters, string searchValue = null)
            => await _dependencyRepository.GetDependeciesHigher(requestParameters, searchValue);

        public async Task<Dependency> GetDependencyByName(string name)
            => await _dependencyRepository.GetDependencyByName(name);

        public async Task<bool> AnyDependencyByAcronym(string acronym)
            => await _dependencyRepository.AnyDependencyByAcronym(acronym);

        public async Task UpdateRange(IEnumerable<Dependency> entities)
            => await _dependencyRepository.UpdateRange(entities);

        public async Task<decimal> GetDependencyDatatableReportTotalAmount(string search, DateTime? startDate = null, DateTime? endDate = null)
            => await _dependencyRepository.GetDependencyDatatableReportTotalAmount(search, startDate, endDate);

        public async Task<DataTablesStructs.ReturnedData<object>> GetBudgetBalanceReport(Guid? dependencyId = null, DateTime? month = null, ClaimsPrincipal user = null)
            => await _dependencyRepository.GetBudgetBalanceReport(dependencyId, month, user);

        public async Task<decimal> GetAvailableAmount(Guid dependencyId)
            => await _dependencyRepository.GetAvailableAmount(dependencyId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetBudgetBalanceKardexReport(Guid? dependencyId = null, DateTime? startDate = null, DateTime? endDate = null, ClaimsPrincipal user = null)
            => await _dependencyRepository.GetBudgetBalanceKardexReport(dependencyId, startDate, endDate, user);
        public async Task<List<UserDependency>> GetByUser(string userId)
            => await _dependencyRepository.GetByUser(userId);
        
        public async Task<Dependency> GetHierarchicalTree(Guid dependencyId)
            => await _dependencyRepository.GetHierarchicalTree(dependencyId);

        public async Task<List<Dependency>> GetDependenciesByCareer(Guid careerId)
            => await _dependencyRepository.GetDependenciesByCareer(careerId);

        public async Task<Select2Structs.ResponseParameters> GetDependencyDirectorsSelect2(Select2Structs.RequestParameters requestParameters, string search)
            => await _dependencyRepository.GetDependencyDirectorsSelect2(requestParameters, search);

        public async Task<List<Dependency>> GetDependenciesByFaculty(Guid facultyId)
            => await _dependencyRepository.GetDependenciesByFaculty(facultyId);
    }
}
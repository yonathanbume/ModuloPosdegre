using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Template;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces
{
    public interface IProcedureService
    {
        Task<IEnumerable<Procedure>> GetAll();
        Task<bool> AnyProcedureByCategory(Guid procedureCategoryId);
        Task<bool> AnyProcedureBySubcategory(Guid procedureSubcategoryId);
        Task<DataTablesStructs.ReturnedData<object>> GetConsolidatedReportDatatable(DataTablesStructs.SentParameters parameters, Guid? categoryId, int? year , string roleId);
        Task<int> Count();
        Task<Procedure> Get(Guid id);
        Task<Procedure> GetProcedure(Guid id);
        Task<IEnumerable<Procedure>> GetProcedures();
        Task<IEnumerable<Procedure>> GetProceduresByProcedureCategory(Guid procedureCategoryId);
        Task<IEnumerable<Procedure>> GetProceduresByProcedureCategory(Guid? procedureCategoryId, Guid? procedureSubcategoryId, string roleId);
        Task<IEnumerable<Procedure>> GetProceduresByProcedureCategory(Guid procedureCategoryId, Guid procedureSubcategoryId);
        Task<IEnumerable<Procedure>> GetProceduresByUser(string userId);
        Task<IEnumerable<Procedure>> GetProceduresByUser(string userId, Guid procedureCategoryId);
        Task<IEnumerable<Procedure>> GetProceduresByUser(string userId, Guid procedureCategoryId, Guid procedureSubcategoryId);
        Task<DataTablesStructs.ReturnedData<Procedure>> GetProceduresDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<Procedure>> GetProceduresDatatableByFaculty(DataTablesStructs.SentParameters sentParameters, Guid facultyId, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<Procedure>> GetProceduresDatatableByFaculty(DataTablesStructs.SentParameters sentParameters, Guid facultyId, Guid procedureCategoryId, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<Procedure>> GetProceduresDatatableByFaculty(DataTablesStructs.SentParameters sentParameters, Guid facultyId, Guid procedureCategoryId, Guid procedureSubcategoryId, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<Procedure>> GetProceduresDatatableByProcedureCategory(DataTablesStructs.SentParameters sentParameters, Guid procedureCategoryId, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<Procedure>> GetProceduresDatatableByProcedureCategory(DataTablesStructs.SentParameters sentParameters, Guid procedureCategoryId, Guid procedureSubcategoryId, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<Procedure>> GetProceduresDatatableByUser(DataTablesStructs.SentParameters sentParameters, string userId, string searchValue = null);
        Task Delete(Procedure procedure);
        Task Insert(Procedure procedure);
        Task Update(Procedure procedure);
        Task<List<Procedure>> GetProceduresBySearchValue(string searchValue);
        Task<decimal> GetEnrollmentFeeCost(Guid procedureId);
        Task<object> GetProcedureJson(string term);
        Task<Procedure> GetByStaticType(int staticType);
        Task<DataTablesStructs.ReturnedData<object>> GetUserProceduresDatatable(DataTablesStructs.SentParameters sentParameters, string code, string search);
        Task<Procedure> GetProcedureByConceptId(Guid conceptId);
        Task<ProcedureValidationResult> ValidateSystemRequirements(Guid procedureId, ClaimsPrincipal user);
        Task<DataTablesStructs.ReturnedData<object>> GetAvailableProceduresByUserDatatable(DataTablesStructs.SentParameters parameters, ClaimsPrincipal User, string search);
    }
}

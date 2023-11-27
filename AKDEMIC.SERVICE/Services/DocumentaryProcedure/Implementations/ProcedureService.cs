using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Template;
using AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Implementations
{
    public class ProcedureService : IProcedureService
    {
        private readonly IProcedureRepository _procedureRepository;

        public ProcedureService(IProcedureRepository procedureRepository)
        {
            _procedureRepository = procedureRepository;
        }

        public async Task<IEnumerable<Procedure>> GetAll()
            => await _procedureRepository.GetAll();
        public async Task<bool> AnyProcedureByCategory(Guid procedureCategoryId)
        {
            return await _procedureRepository.AnyProcedureByCategory(procedureCategoryId);
        }

        public async Task<bool> AnyProcedureBySubcategory(Guid procedureSubcategoryId)
        {
            return await _procedureRepository.AnyProcedureBySubcategory(procedureSubcategoryId);
        }

        public async Task<int> Count()
        {
            return await _procedureRepository.Count();
        }

        public async Task<Procedure> Get(Guid id)
        {
            return await _procedureRepository.Get(id);
        }

        public async Task<Procedure> GetProcedure(Guid id)
        {
            return await _procedureRepository.GetProcedure(id);
        }

        public async Task<IEnumerable<Procedure>> GetProcedures()
        {
            return await _procedureRepository.GetProcedures();
        }

        public async Task<IEnumerable<Procedure>> GetProceduresByProcedureCategory(Guid procedureCategoryId)
        {
            return await _procedureRepository.GetProceduresByProcedureCategory(procedureCategoryId);
        }

        public async Task<IEnumerable<Procedure>> GetProceduresByProcedureCategory(Guid procedureCategoryId, Guid procedureSubcategoryId)
        {
            return await _procedureRepository.GetProceduresByProcedureCategory(procedureCategoryId, procedureSubcategoryId);
        }

        public async Task<IEnumerable<Procedure>> GetProceduresByUser(string userId)
        {
            return await _procedureRepository.GetProceduresByUser(userId);
        }

        public async Task<IEnumerable<Procedure>> GetProceduresByUser(string userId, Guid procedureCategoryId)
        {
            return await _procedureRepository.GetProceduresByUser(userId, procedureCategoryId);
        }

        public async Task<IEnumerable<Procedure>> GetProceduresByUser(string userId, Guid procedureCategoryId, Guid procedureSubcategoryId)
        {
            return await _procedureRepository.GetProceduresByUser(userId, procedureCategoryId, procedureSubcategoryId);
        }

        public async Task<DataTablesStructs.ReturnedData<Procedure>> GetProceduresDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await _procedureRepository.GetProceduresDatatable(sentParameters, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<Procedure>> GetProceduresDatatableByFaculty(DataTablesStructs.SentParameters sentParameters, Guid facultyId, string searchValue = null)
        {
            return await _procedureRepository.GetProceduresDatatableByFaculty(sentParameters, facultyId, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<Procedure>> GetProceduresDatatableByFaculty(DataTablesStructs.SentParameters sentParameters, Guid facultyId, Guid procedureCategoryId, string searchValue = null)
        {
            return await _procedureRepository.GetProceduresDatatableByFaculty(sentParameters, facultyId, procedureCategoryId, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<Procedure>> GetProceduresDatatableByFaculty(DataTablesStructs.SentParameters sentParameters, Guid facultyId, Guid procedureCategoryId, Guid procedureSubcategoryId, string searchValue = null)
        {
            return await _procedureRepository.GetProceduresDatatableByFaculty(sentParameters, facultyId, procedureCategoryId, procedureSubcategoryId, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<Procedure>> GetProceduresDatatableByProcedureCategory(DataTablesStructs.SentParameters sentParameters, Guid procedureCategoryId, string searchValue = null)
        {
            return await _procedureRepository.GetProceduresDatatableByProcedureCategory(sentParameters, procedureCategoryId, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<Procedure>> GetProceduresDatatableByProcedureCategory(DataTablesStructs.SentParameters sentParameters, Guid procedureCategoryId, Guid procedureSubcategoryId, string searchValue = null)
        {
            return await _procedureRepository.GetProceduresDatatableByProcedureCategory(sentParameters, procedureCategoryId, procedureSubcategoryId, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<Procedure>> GetProceduresDatatableByUser(DataTablesStructs.SentParameters sentParameters, string userId, string searchValue = null)
        {
            return await _procedureRepository.GetProceduresDatatableByUser(sentParameters, userId, searchValue);
        }

        public async Task Delete(Procedure procedure) =>
            await _procedureRepository.Delete(procedure);

        public async Task Insert(Procedure procedure) =>
            await _procedureRepository.Insert(procedure);

        public async Task Update(Procedure procedure) =>
            await _procedureRepository.Update(procedure);















        /*public async Task<IEnumerable<Procedure>> GetProceduresDatatable(DataTablesStructs.SentParameters sentParameters, Tuple<Func<Procedure, string[]>, string> searchValuePredicate = null)
        {
            return await _procedureRepository.GetProceduresDatatable(sentParameters, searchValuePredicate);
        }

        public async Task<IEnumerable<Procedure>> GetProceduresDatatableByProcedureCategory(DataTablesStructs.SentParameters sentParameters, Guid procedureCategoryId, Tuple<Func<Procedure, string[]>, string> searchValuePredicate = null)
        {
            return await _procedureRepository.GetProceduresDatatableByProcedureCategory(sentParameters, procedureCategoryId, searchValuePredicate);
        }

        public async Task<IEnumerable<Procedure>> GetProceduresDatatableByProcedureCategory(DataTablesStructs.SentParameters sentParameters, Guid procedureCategoryId, Guid procedureSubcategoryId, Tuple<Func<Procedure, string[]>, string> searchValuePredicate = null)
        {
            return await _procedureRepository.GetProceduresDatatableByProcedureCategory(sentParameters, procedureCategoryId, procedureSubcategoryId, searchValuePredicate);
        }

        public async Task<IEnumerable<Procedure>> GetProceduresDatatableByUser(DataTablesStructs.SentParameters sentParameters, string userId, Tuple<Func<Procedure, string[]>, string> searchValuePredicate = null)
        {
            return await _procedureRepository.GetProceduresDatatableByUser(sentParameters, userId, searchValuePredicate);
        }

        public async Task<IEnumerable<Procedure>> GetProceduresDatatableByUser(DataTablesStructs.SentParameters sentParameters, string userId, Guid procedureCategoryId, Tuple<Func<Procedure, string[]>, string> searchValuePredicate = null)
        {
            return await _procedureRepository.GetProceduresDatatableByUser(sentParameters, userId, procedureCategoryId, searchValuePredicate);
        }

        public async Task<IEnumerable<Procedure>> GetProceduresDatatableByUser(DataTablesStructs.SentParameters sentParameters, string userId, Guid procedureCategoryId, Guid procedureSubcategoryId, Tuple<Func<Procedure, string[]>, string> searchValuePredicate = null)
        {
            return await _procedureRepository.GetProceduresDatatableByUser(sentParameters, userId, procedureCategoryId, procedureSubcategoryId, searchValuePredicate);
        }*/

        public async Task<List<Procedure>> GetProceduresBySearchValue(string searchValue)
        {
            return await _procedureRepository.GetProceduresBySearchValue(searchValue);
        }

        public async Task<decimal> GetEnrollmentFeeCost(Guid procedureId)
            => await _procedureRepository.GetEnrollmentFeeCost(procedureId);

        public async Task<object> GetProcedureJson(string term)
            => await _procedureRepository.GetProcedureJson(term);
        public async Task<DataTablesStructs.ReturnedData<object>> GetUserProceduresDatatable(DataTablesStructs.SentParameters sentParameters, string code, string search)
            => await _procedureRepository.GetUserProceduresDatatable(sentParameters, code, search);

        public async Task<Procedure> GetByStaticType(int staticType)
            => await _procedureRepository.GetByStaticType(staticType);

        public async Task<Procedure> GetProcedureByConceptId(Guid conceptId)
            => await _procedureRepository.GetProcedureByConceptId(conceptId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetAvailableProceduresByUserDatatable(DataTablesStructs.SentParameters parameters, ClaimsPrincipal User, string search)
            => await _procedureRepository.GetAvailableProceduresByUserDatatable(parameters, User, search);

        public async Task<ProcedureValidationResult> ValidateSystemRequirements(Guid procedureId, ClaimsPrincipal user)
            => await _procedureRepository.ValidateSystemRequirements(procedureId, user);

        public async Task<IEnumerable<Procedure>> GetProceduresByProcedureCategory(Guid? procedureCategoryId, Guid? procedureSubcategoryId, string roleId)
            => await _procedureRepository.GetProceduresByProcedureCategory(procedureCategoryId, procedureSubcategoryId, roleId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetConsolidatedReportDatatable(DataTablesStructs.SentParameters parameters, Guid? categoryId, int? year ,string roleId)
            => await _procedureRepository.GetConsolidatedReportDatatable(parameters, categoryId, year, roleId);
    }
}

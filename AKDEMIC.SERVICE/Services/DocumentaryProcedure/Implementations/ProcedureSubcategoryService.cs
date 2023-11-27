using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces;
using AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Implementations
{
    public class ProcedureSubcategoryService : IProcedureSubcategoryService
    {
        private readonly IProcedureSubcategoryRepository _procedureSubcategoryRepository;

        public ProcedureSubcategoryService(IProcedureSubcategoryRepository procedureSubcategoryRepository)
        {
            _procedureSubcategoryRepository = procedureSubcategoryRepository;
        }

        public async Task<bool> AnyProcedureSubcategory(Guid id)
        {
            return await _procedureSubcategoryRepository.AnyProcedureSubcategory(id);
        }

        public async Task<bool> AnyProcedureSubcategory(Guid id, Guid procedureCategoryId)
        {
            return await _procedureSubcategoryRepository.AnyProcedureSubcategory(id, procedureCategoryId);
        }

        public async Task<bool> AnyProcedureSubcategoryByCategory(Guid procedureCategoryId)
        {
            return await _procedureSubcategoryRepository.AnyProcedureSubcategoryByCategory(procedureCategoryId);
        }

        public async Task<ProcedureSubcategory> Get(Guid id)
        {
            return await _procedureSubcategoryRepository.Get(id);
        }

        public async Task<ProcedureSubcategory> GetProcedureSubcategory(Guid id)
        {
            return await _procedureSubcategoryRepository.GetProcedureSubcategory(id);
        }

        public async Task<IEnumerable<ProcedureSubcategory>> GetProcedureSubcategories()
        {
            return await _procedureSubcategoryRepository.GetProcedureSubcategories();
        }

        public async Task<IEnumerable<ProcedureSubcategory>> GetProcedureSubcategoriesByCategory(Guid categoryId)
        {
            return await _procedureSubcategoryRepository.GetProcedureSubcategoriesByCategory(categoryId);
        }

        public async Task<DataTablesStructs.ReturnedData<ProcedureSubcategory>> GetProcedureSubcategoriesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await _procedureSubcategoryRepository.GetProcedureSubcategoriesDatatable(sentParameters, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<ProcedureSubcategory>> GetProcedureSubcategoriesDatatableByProcedureCategory(DataTablesStructs.SentParameters sentParameters, Guid procedureCategoryId, string searchValue = null)
        {
            return await _procedureSubcategoryRepository.GetProcedureSubcategoriesDatatableByProcedureCategory(sentParameters, procedureCategoryId, searchValue);
        }

        public async Task<Select2Structs.ResponseParameters> GetProcedureSubcategoriesSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null)
        {
            return await _procedureSubcategoryRepository.GetProcedureSubcategoriesSelect2(requestParameters, searchValue);
        }

        public async Task<Select2Structs.ResponseParameters> GetProcedureSubcategoriesSelect2ByProcedureCategory(Select2Structs.RequestParameters requestParameters, Guid procedureCategoryId, string searchValue = null)
        {
            return await _procedureSubcategoryRepository.GetProcedureSubcategoriesSelect2ByProcedureCategory(requestParameters, procedureCategoryId, searchValue);
        }

        public async Task<Select2Structs.ResponseParameters> GetProcedureProcedureSubcategoriesSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null)
        {
            return await _procedureSubcategoryRepository.GetProcedureProcedureSubcategoriesSelect2(requestParameters, searchValue);
        }

        public async Task<Select2Structs.ResponseParameters> GetProcedureProcedureSubcategoriesSelect2ByProcedureCategory(Select2Structs.RequestParameters requestParameters, Guid procedureCategoryId, string searchValue = null)
        {
            return await _procedureSubcategoryRepository.GetProcedureProcedureSubcategoriesSelect2ByProcedureCategory(requestParameters, procedureCategoryId, searchValue);
        }

        public async Task Delete(ProcedureSubcategory procedureSubcategory) =>
            await _procedureSubcategoryRepository.Delete(procedureSubcategory);

        public async Task Insert(ProcedureSubcategory procedureSubcategory) =>
            await _procedureSubcategoryRepository.Insert(procedureSubcategory);

        public async Task Update(ProcedureSubcategory procedureSubcategory) =>
            await _procedureSubcategoryRepository.Update(procedureSubcategory);










        public async Task<IEnumerable<ProcedureSubcategory>> GetProcedureSubcategoriesDatatable(DataTablesStructs.SentParameters sentParameters, Tuple<Func<ProcedureSubcategory, string[]>, string> searchValuePredicate = null)
        {
            return await _procedureSubcategoryRepository.GetProcedureSubcategoriesDatatable(sentParameters, searchValuePredicate);
        }

        public async Task<IEnumerable<ProcedureSubcategory>> GetProcedureSubcategoriesDatatableByCategory(DataTablesStructs.SentParameters sentParameters, Guid categoryId, Tuple<Func<ProcedureSubcategory, string[]>, string> searchValuePredicate = null)
        {
            return await _procedureSubcategoryRepository.GetProcedureSubcategoriesDatatableByCategory(sentParameters, categoryId, searchValuePredicate);
        }
    }
}

using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces;
using AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Implementations
{
    public class ProcedureCategoryService : IProcedureCategoryService
    {
        private readonly IProcedureCategoryRepository _procedureCategoryRepository;

        public ProcedureCategoryService(IProcedureCategoryRepository procedureCategoryRepository)
        {
            _procedureCategoryRepository = procedureCategoryRepository;
        }

        public async Task<ProcedureCategory> Get(Guid id)
        {
            return await _procedureCategoryRepository.Get(id);
        }

        public async Task<IEnumerable<ProcedureCategory>> GetAll()
        {
            return await _procedureCategoryRepository.GetAll();
        }

        public async Task<IEnumerable<ProcedureCategory>> GetProcedureCategories()
        {
            return await _procedureCategoryRepository.GetProcedureCategories();
        }

        public async Task<DataTablesStructs.ReturnedData<ProcedureCategory>> GetProcedureCategoriesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await _procedureCategoryRepository.GetProcedureCategoriesDatatable(sentParameters, searchValue);
        }

        public async Task<Select2Structs.ResponseParameters> GetProcedureCategoriesSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null)
        {
            return await _procedureCategoryRepository.GetProcedureCategoriesSelect2(requestParameters, searchValue);
        }

        public async Task<Select2Structs.ResponseParameters> GetProcedureProcedureCategoriesSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null)
        {
            return await _procedureCategoryRepository.GetProcedureProcedureCategoriesSelect2(requestParameters, searchValue);
        }

        public async Task Delete(ProcedureCategory procedureCategory)
        {
            await _procedureCategoryRepository.Delete(procedureCategory);
        }

        public async Task Insert(ProcedureCategory procedureCategory)
        {
            await _procedureCategoryRepository.Insert(procedureCategory);
        }

        public async Task Update(ProcedureCategory procedureCategory)
        {
            await _procedureCategoryRepository.Update(procedureCategory);
        }










        public async Task<IEnumerable<ProcedureCategory>> GetProcedureCategoriesDatatable(DataTablesStructs.SentParameters sentParameters, Tuple<Func<ProcedureCategory, string[]>, string> searchValuePredicate = null)
        {
            return await _procedureCategoryRepository.GetProcedureCategoriesDatatable(sentParameters, searchValuePredicate);
        }
    }
}

using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces
{
    public interface IProcedureSubcategoryRepository : IRepository<ProcedureSubcategory>
    {
        Task<bool> AnyProcedureSubcategory(Guid id);
        Task<bool> AnyProcedureSubcategory(Guid id, Guid procedureCategoryId);
        Task<bool> AnyProcedureSubcategoryByCategory(Guid procedureCategoryId);
        Task<ProcedureSubcategory> GetProcedureSubcategory(Guid id);
        Task<IEnumerable<ProcedureSubcategory>> GetProcedureSubcategories();
        Task<IEnumerable<ProcedureSubcategory>> GetProcedureSubcategoriesByCategory(Guid categoryId);
        Task<DataTablesStructs.ReturnedData<ProcedureSubcategory>> GetProcedureSubcategoriesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<ProcedureSubcategory>> GetProcedureSubcategoriesDatatableByProcedureCategory(DataTablesStructs.SentParameters sentParameters, Guid procedureCategoryId, string searchValue = null);
        Task<Select2Structs.ResponseParameters> GetProcedureSubcategoriesSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null);
        Task<Select2Structs.ResponseParameters> GetProcedureSubcategoriesSelect2ByProcedureCategory(Select2Structs.RequestParameters requestParameters, Guid procedureCategoryId, string searchValue = null);
        Task<Select2Structs.ResponseParameters> GetProcedureProcedureSubcategoriesSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null);
        Task<Select2Structs.ResponseParameters> GetProcedureProcedureSubcategoriesSelect2ByProcedureCategory(Select2Structs.RequestParameters requestParameters, Guid procedureCategoryId, string searchValue = null);





        Task<IEnumerable<ProcedureSubcategory>> GetProcedureSubcategoriesDatatable(DataTablesStructs.SentParameters sentParameters, Tuple<Func<ProcedureSubcategory, string[]>, string> searchValuePredicate = null);
        Task<IEnumerable<ProcedureSubcategory>> GetProcedureSubcategoriesDatatableByCategory(DataTablesStructs.SentParameters sentParameters, Guid categoryId, Tuple<Func<ProcedureSubcategory, string[]>, string> searchValuePredicate = null);
    }
}

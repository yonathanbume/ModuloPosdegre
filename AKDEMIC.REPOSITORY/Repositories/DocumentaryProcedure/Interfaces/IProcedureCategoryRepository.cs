using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces
{
    public interface IProcedureCategoryRepository : IRepository<ProcedureCategory>
    {
        Task<IEnumerable<ProcedureCategory>> GetProcedureCategories();
        Task<DataTablesStructs.ReturnedData<ProcedureCategory>> GetProcedureCategoriesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<Select2Structs.ResponseParameters> GetProcedureCategoriesSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null);
        Task<Select2Structs.ResponseParameters> GetProcedureProcedureCategoriesSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null);






        Task<IEnumerable<ProcedureCategory>> GetProcedureCategoriesDatatable(DataTablesStructs.SentParameters sentParameters, Tuple<Func<ProcedureCategory, string[]>, string> searchValuePredicate = null);
    }
}

using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces
{
    public interface IProcedureCategoryService
    {
        Task<ProcedureCategory> Get(Guid id);
        Task<IEnumerable<ProcedureCategory>> GetAll();
        Task<IEnumerable<ProcedureCategory>> GetProcedureCategories();
        Task<DataTablesStructs.ReturnedData<ProcedureCategory>> GetProcedureCategoriesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<Select2Structs.ResponseParameters> GetProcedureCategoriesSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null);
        Task<Select2Structs.ResponseParameters> GetProcedureProcedureCategoriesSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null);
        Task Delete(ProcedureCategory procedureCategory);
        Task Insert(ProcedureCategory procedureCategory);
        Task Update(ProcedureCategory procedureCategory);





        Task<IEnumerable<ProcedureCategory>> GetProcedureCategoriesDatatable(DataTablesStructs.SentParameters sentParameters, Tuple<Func<ProcedureCategory, string[]>, string> searchValuePredicate = null);
    }
}

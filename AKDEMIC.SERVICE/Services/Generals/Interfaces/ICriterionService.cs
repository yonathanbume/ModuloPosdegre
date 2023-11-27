using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Generals.Interfaces
{
    public interface ICriterionService
    {
        Task<DataTablesStructs.ReturnedData<Criterion>> GetCriterionsDatatable(DataTablesStructs.SentParameters sentParameters, string name = null, string userId = null, string searchValue = null);
        Task<bool> ExistName(string name, Guid? currentCriterionId = null);
        Task<bool> ExistCode(string code, Guid? currentCriterionId = null);
        Task Insert(Criterion entity);
        Task Update(Criterion entity);
        Task<Criterion> Get(Guid id);
        Task Delete(Criterion entity);
        Task<Select2Structs.ResponseParameters> GetCriterionsByAcademicProgramIdSelect2(Select2Structs.RequestParameters requestParameters, string keyname);
    }
}

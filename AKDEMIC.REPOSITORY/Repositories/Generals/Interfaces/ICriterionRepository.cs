using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces
{
    public interface ICriterionRepository : IRepository<Criterion>
    {
        Task<DataTablesStructs.ReturnedData<Criterion>> GetCriterionsDatatable(DataTablesStructs.SentParameters sentParameters, string name = null, string userId = null,string searchValue = null);
        Task<bool> ExistName(string name,Guid? currentCriterionId = null);
        Task<bool> ExistCode(string code,Guid? currentCriterionId = null);
        Task<Select2Structs.ResponseParameters> GetCriterionsByAcademicProgramIdSelect2(Select2Structs.RequestParameters requestParameters, string keyname);
    }
}

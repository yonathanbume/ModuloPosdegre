using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Evaluation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Evaluation.Interfaces
{
    public interface ICulturalActivityTypeService
    {
        Task<bool> AnyByTypeId(Guid typeId);
        Task Update(CulturalActivityType entity);
        Task Insert(CulturalActivityType entity);
        Task Delete(CulturalActivityType entity);
        Task<CulturalActivityType> Get(Guid id);
        Task<bool> AnyByName(string name, Guid? ignoredId = null);
        Task<DataTablesStructs.ReturnedData<CulturalActivityType>> GetDatatable(DataTablesStructs.SentParameters sentParameters, string search = null);
        Task<IEnumerable<Select2Structs.Result>> GetTypeSelect2ClientSide();
    }
}

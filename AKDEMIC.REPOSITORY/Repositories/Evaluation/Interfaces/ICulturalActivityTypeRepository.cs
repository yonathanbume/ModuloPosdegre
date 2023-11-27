using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Evaluation;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Evaluation.Interfaces
{
    public interface ICulturalActivityTypeRepository : IRepository<CulturalActivityType>
    {
        Task<bool> AnyByTypeId(Guid typeId);
        Task<bool> AnyByName(string name, Guid? ignoredId = null);
        Task<DataTablesStructs.ReturnedData<CulturalActivityType>> GetDatatable(DataTablesStructs.SentParameters sentParameters, string search = null);
        Task<IEnumerable<Select2Structs.Result>> GetTypeSelect2ClientSide();
    }
}

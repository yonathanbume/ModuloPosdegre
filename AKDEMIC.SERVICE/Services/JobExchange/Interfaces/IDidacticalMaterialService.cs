using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.JobExchange.Interfaces
{
    public interface IDidacticalMaterialService
    {
        Task<DidacticalMaterial> Get(Guid id);
        Task<IEnumerable<DidacticalMaterial>> GetAll();
        Task Insert(DidacticalMaterial didacticalMaterial);
        Task Update(DidacticalMaterial didacticalMaterial);
        Task Delete(DidacticalMaterial didacticalMaterial);
        Task<DataTablesStructs.ReturnedData<object>> GetAllDidacticalMaterialsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
    }
}

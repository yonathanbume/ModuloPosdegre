using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.JobExchange.Interfaces
{
    public interface IDidacticalMaterialFileService
    {
        Task<DidacticalMaterialFile> Get(Guid id);
        Task<IEnumerable<DidacticalMaterialFile>> GetAll();
        Task Insert(DidacticalMaterialFile didacticalMaterialFile);
        Task Update(DidacticalMaterialFile didacticalMaterialFile);
        Task Delete(DidacticalMaterialFile didacticalMaterialFile);
        Task<DataTablesStructs.ReturnedData<object>> GetAllDidacticalMaterialFilesDatatable(DataTablesStructs.SentParameters sentParameters, Guid didacticalMaterialId, string searchValue = null);
    }
}

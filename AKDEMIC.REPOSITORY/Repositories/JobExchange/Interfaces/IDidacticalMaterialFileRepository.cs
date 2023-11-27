using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces
{
    public interface IDidacticalMaterialFileRepository: IRepository<DidacticalMaterialFile>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetAllDidacticalMaterialFilesDatatable(DataTablesStructs.SentParameters sentParameters, Guid didacticalMaterialId, string searchValue = null);
    }
}

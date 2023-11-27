using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces
{
    public interface IActivityProcedureRepository : IRepository<ActivityProcedure>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetActivityProcedureDatatable(DataTablesStructs.SentParameters sentParameters, string search);
    }
}

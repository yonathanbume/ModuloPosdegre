using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces
{
    public interface IProcedureFolderRepository : IRepository<ProcedureFolder>
    {
        Task<object> GetSelect2DataClientSide(Guid dependencyId);
        Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters sentParameters, Guid dependencyId, string search = null);
        Task<bool> AnyByCode(string code, Guid dependecyId, Guid? ignoredId = null);
        Task<DataTablesStructs.ReturnedData<object>> GetUserProceduresDatatable(DataTablesStructs.SentParameters sentParameters, Guid procedureFolderId, Guid dependencyId);
    }
}

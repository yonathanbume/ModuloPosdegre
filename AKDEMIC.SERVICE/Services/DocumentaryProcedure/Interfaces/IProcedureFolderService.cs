using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces
{
    public interface IProcedureFolderService
    {
        Task Insert(ProcedureFolder procedureFolder);
        Task Update(ProcedureFolder procedureFolder);
        Task<ProcedureFolder> Get(Guid id);
        Task<object> GetSelect2DataClientSide(Guid dependencyId);
        Task DeleteById(Guid folderId);
        Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters sentParameters, Guid dependencyId, string search = null);
        Task<bool> AnyByCode(string code, Guid dependecyId, Guid? ignoredId = null);
        Task<DataTablesStructs.ReturnedData<object>> GetUserProceduresDatatable(DataTablesStructs.SentParameters sentParameters, Guid procedureFolderId, Guid dependencyId);
    }
}

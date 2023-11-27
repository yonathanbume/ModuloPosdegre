using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces
{
    public interface IInternalProcedureFileService
    {
        Task<InternalProcedureFile> Get(Guid id);
        Task<IEnumerable<InternalProcedureFile>> GetInternalProcedureFilesByInternalProcedure(Guid internalProcedureId);
        Task<DataTablesStructs.ReturnedData<InternalProcedureFile>> GetInternalProcedureFilesDatatableByInternalProcedure(DataTablesStructs.SentParameters sentParameters, Guid internalProcedureId, string searchValue = null);
        Task Delete(InternalProcedureFile internalProcedureFile);
        Task DeleteRange(IEnumerable<InternalProcedureFile> internalProcedureFiles);
        Task Insert(InternalProcedureFile internalProcedureFile);
        Task InsertRange(IEnumerable<InternalProcedureFile> internalProcedureFiles);




        Task<IEnumerable<InternalProcedureFile>> GetInternalProcedureFilesDatatableByInternalProcedure(DataTablesStructs.SentParameters sentParameters, Guid internalProcedureId, Tuple<Func<InternalProcedureFile, string[]>, string> searchValuePredicate = null);
    }
}

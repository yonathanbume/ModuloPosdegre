using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces
{
    public interface IInternalProcedureFileRepository : IRepository<InternalProcedureFile>
    {
        Task<IEnumerable<InternalProcedureFile>> GetInternalProcedureFilesByInternalProcedure(Guid internalProcedureId);
        Task<DataTablesStructs.ReturnedData<InternalProcedureFile>> GetInternalProcedureFilesDatatableByInternalProcedure(DataTablesStructs.SentParameters sentParameters, Guid internalProcedureId, string searchValue = null);




        Task<IEnumerable<InternalProcedureFile>> GetInternalProcedureFilesDatatableByInternalProcedure(DataTablesStructs.SentParameters sentParameters, Guid internalProcedureId, Tuple<Func<InternalProcedureFile, string[]>, string> searchValuePredicate = null);
    }
}

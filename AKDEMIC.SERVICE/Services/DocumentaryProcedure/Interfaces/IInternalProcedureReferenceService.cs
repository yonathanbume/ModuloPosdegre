using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces
{
    public interface IInternalProcedureReferenceService
    {
        Task<InternalProcedureReference> Get(Guid id);
        Task<IEnumerable<InternalProcedureReference>> GetInternalProcedureReferencesByInternalProcedure(Guid internalProcedureId);
        Task<DataTablesStructs.ReturnedData<InternalProcedureReference>> GetInternalProcedureReferencesDatatableByInternalProcedure(DataTablesStructs.SentParameters sentParameters, Guid internalProcedureId, string searchValue = null);
        Task Delete(InternalProcedureReference internalProcedureReference);
        Task DeleteRange(IEnumerable<InternalProcedureReference> internalProcedureReferences);
        Task Insert(InternalProcedureReference internalProcedureReference);
        Task InsertRange(IEnumerable<InternalProcedureReference> internalProcedureReferences);








        Task<IEnumerable<InternalProcedureReference>> GetInternalProcedureReferencesDatatableByInternalProcedure(DataTablesStructs.SentParameters sentParameters, Guid internalProcedureId, Tuple<Func<InternalProcedureReference, string[]>, string> searchValuePredicate = null);
    }
}

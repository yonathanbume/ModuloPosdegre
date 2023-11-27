using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces
{
    public interface IInternalProcedureReferenceRepository : IRepository<InternalProcedureReference>
    {
        Task<IEnumerable<InternalProcedureReference>> GetInternalProcedureReferencesByInternalProcedure(Guid internalProcedureId);
        Task<DataTablesStructs.ReturnedData<InternalProcedureReference>> GetInternalProcedureReferencesDatatableByInternalProcedure(DataTablesStructs.SentParameters sentParameters, Guid internalProcedureId, string searchValue = null);




        Task<IEnumerable<InternalProcedureReference>> GetInternalProcedureReferencesDatatableByInternalProcedure(DataTablesStructs.SentParameters sentParameters, Guid internalProcedureId, Tuple<Func<InternalProcedureReference, string[]>, string> searchValuePredicate = null);
    }
}

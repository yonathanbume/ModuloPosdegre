using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces;
using AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Implementations
{
    public class InternalProcedureReferenceService : IInternalProcedureReferenceService
    {
        private readonly IInternalProcedureReferenceRepository _internalProcedureReferenceRepository;

        public InternalProcedureReferenceService(IInternalProcedureReferenceRepository internalProcedureReferenceRepository)
        {
            _internalProcedureReferenceRepository = internalProcedureReferenceRepository;
        }

        public async Task<InternalProcedureReference> Get(Guid id)
        {
            return await _internalProcedureReferenceRepository.Get(id);
        }

        public async Task<IEnumerable<InternalProcedureReference>> GetInternalProcedureReferencesByInternalProcedure(Guid internalProcedureId)
        {
            return await _internalProcedureReferenceRepository.GetInternalProcedureReferencesByInternalProcedure(internalProcedureId);
        }

        public async Task<DataTablesStructs.ReturnedData<InternalProcedureReference>> GetInternalProcedureReferencesDatatableByInternalProcedure(DataTablesStructs.SentParameters sentParameters, Guid internalProcedureId, string searchValue = null)
        {
            return await _internalProcedureReferenceRepository.GetInternalProcedureReferencesDatatableByInternalProcedure(sentParameters, internalProcedureId, searchValue);
        }

        public async Task Delete(InternalProcedureReference internalProcedureReference) =>
            await _internalProcedureReferenceRepository.Delete(internalProcedureReference);

        public async Task DeleteRange(IEnumerable<InternalProcedureReference> internalProcedureReferences) =>
            await _internalProcedureReferenceRepository.DeleteRange(internalProcedureReferences);

        public async Task Insert(InternalProcedureReference internalProcedureReference) =>
            await _internalProcedureReferenceRepository.Insert(internalProcedureReference);

        public async Task InsertRange(IEnumerable<InternalProcedureReference> internalProcedureReferences) =>
            await _internalProcedureReferenceRepository.InsertRange(internalProcedureReferences);










        public async Task<IEnumerable<InternalProcedureReference>> GetInternalProcedureReferencesDatatableByInternalProcedure(DataTablesStructs.SentParameters sentParameters, Guid internalProcedureId, Tuple<Func<InternalProcedureReference, string[]>, string> searchValuePredicate = null)
        {
            return await _internalProcedureReferenceRepository.GetInternalProcedureReferencesDatatableByInternalProcedure(sentParameters, internalProcedureId, searchValuePredicate);
        }
    }
}

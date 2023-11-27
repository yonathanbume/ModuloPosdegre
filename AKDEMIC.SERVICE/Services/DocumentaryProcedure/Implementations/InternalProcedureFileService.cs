using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces;
using AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Implementations
{
    public class InternalProcedureFileService : IInternalProcedureFileService
    {
        private readonly IInternalProcedureFileRepository _internalProcedureFileRepository;

        public InternalProcedureFileService(IInternalProcedureFileRepository internalProcedureFileRepository)
        {
            _internalProcedureFileRepository = internalProcedureFileRepository;
        }

        public async Task<InternalProcedureFile> Get(Guid id)
        {
            return await _internalProcedureFileRepository.Get(id);
        }

        public async Task<IEnumerable<InternalProcedureFile>> GetInternalProcedureFilesByInternalProcedure(Guid internalProcedureId)
        {
            return await _internalProcedureFileRepository.GetInternalProcedureFilesByInternalProcedure(internalProcedureId);
        }

        public async Task<DataTablesStructs.ReturnedData<InternalProcedureFile>> GetInternalProcedureFilesDatatableByInternalProcedure(DataTablesStructs.SentParameters sentParameters, Guid internalProcedureId, string searchValue = null)
        {
            return await _internalProcedureFileRepository.GetInternalProcedureFilesDatatableByInternalProcedure(sentParameters, internalProcedureId, searchValue);
        }

        public async Task Delete(InternalProcedureFile internalProcedureFile) =>
            await _internalProcedureFileRepository.Delete(internalProcedureFile);

        public async Task DeleteRange(IEnumerable<InternalProcedureFile> internalProcedureFiles) =>
            await _internalProcedureFileRepository.DeleteRange(internalProcedureFiles);

        public async Task Insert(InternalProcedureFile internalProcedureFile) =>
            await _internalProcedureFileRepository.Insert(internalProcedureFile);

        public async Task InsertRange(IEnumerable<InternalProcedureFile> internalProcedureFiles) =>
            await _internalProcedureFileRepository.InsertRange(internalProcedureFiles);













        public async Task<IEnumerable<InternalProcedureFile>> GetInternalProcedureFilesDatatableByInternalProcedure(DataTablesStructs.SentParameters sentParameters, Guid internalProcedureId, Tuple<Func<InternalProcedureFile, string[]>, string> searchValuePredicate = null)
        {
            return await _internalProcedureFileRepository.GetInternalProcedureFilesDatatableByInternalProcedure(sentParameters, internalProcedureId, searchValuePredicate);
        }
    }
}

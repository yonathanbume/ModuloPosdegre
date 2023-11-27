using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces;
using AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Implementations
{
    public class ProcedureFolderService : IProcedureFolderService
    {
        private readonly IProcedureFolderRepository _procedureFolderRepository;
        public ProcedureFolderService(IProcedureFolderRepository procedureFolderRepository)
        {
            _procedureFolderRepository = procedureFolderRepository;
        }

        public async Task<bool> AnyByCode(string code, Guid dependecyId, Guid? ignoredId = null)
            => await _procedureFolderRepository.AnyByCode(code, dependecyId, ignoredId);

        public async Task DeleteById(Guid folderId)
            => await _procedureFolderRepository.DeleteById(folderId);

        public async Task<ProcedureFolder> Get(Guid id)
            => await _procedureFolderRepository.Get(id);

        public async Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters sentParameters, Guid dependencyId, string search = null)
            => await _procedureFolderRepository.GetDatatable(sentParameters, dependencyId, search);

        public async Task<object> GetSelect2DataClientSide(Guid dependencyId)
            => await _procedureFolderRepository.GetSelect2DataClientSide(dependencyId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetUserProceduresDatatable(DataTablesStructs.SentParameters sentParameters, Guid procedureFolderId, Guid dependencyId)
            => await _procedureFolderRepository.GetUserProceduresDatatable(sentParameters, procedureFolderId, dependencyId);

        public async Task Insert(ProcedureFolder procedureFolder)
            => await _procedureFolderRepository.Insert(procedureFolder);


        public async Task Update(ProcedureFolder procedureFolder)
            => await _procedureFolderRepository.Update(procedureFolder);

    }
}

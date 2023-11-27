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
    public class UserExternalProcedureFileService : IUserExternalProcedureFileService
    {
        private readonly IUserExternalProcedureFileRepository _userExternalProcedureFileRepository;

        public UserExternalProcedureFileService(IUserExternalProcedureFileRepository userExternalProcedureFileRepository)
        {
            _userExternalProcedureFileRepository = userExternalProcedureFileRepository;
        }

        public async Task<UserExternalProcedureFile> Get(Guid id)
        {
            return await _userExternalProcedureFileRepository.Get(id);
        }

        public async Task Delete(UserExternalProcedureFile userExternalProcedureFile) =>
            await _userExternalProcedureFileRepository.Delete(userExternalProcedureFile);

        public async Task DeleteRange(IEnumerable<UserExternalProcedureFile> userExternalProcedureFiles) =>
            await _userExternalProcedureFileRepository.DeleteRange(userExternalProcedureFiles);

        public async Task Insert(UserExternalProcedureFile userExternalProcedureFile) =>
            await _userExternalProcedureFileRepository.Insert(userExternalProcedureFile);

        public async Task InsertRange(IEnumerable<UserExternalProcedureFile> userExternalProcedureFiles) =>
            await _userExternalProcedureFileRepository.InsertRange(userExternalProcedureFiles);

        public async Task<DataTablesStructs.ReturnedData<object>> GetRequerimentDocuments(DataTablesStructs.SentParameters sentParameters, Guid id)
            => await _userExternalProcedureFileRepository.GetRequerimentDocuments(sentParameters, id);
        public async Task<List<UserExternalProcedureFile>> GetExternalProcedureFilesByUserExternalProcedure(Guid id)
            => await _userExternalProcedureFileRepository.GetExternalProcedureFilesByUserExternalProcedure(id);
    }
}

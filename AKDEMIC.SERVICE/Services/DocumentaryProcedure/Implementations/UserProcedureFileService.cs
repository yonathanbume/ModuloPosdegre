using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces;
using AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Implementations
{
    public class UserProcedureFileService : IUserProcedureFileService
    {
        private readonly IUserProcedureFileRepository _UserProcedureFileRepository;

        public UserProcedureFileService(IUserProcedureFileRepository UserProcedureFileRepository)
        {
            _UserProcedureFileRepository = UserProcedureFileRepository;
        }

        public async Task<UserProcedureFile> Get(Guid id)
        {
            return await _UserProcedureFileRepository.Get(id);
        }

        public async Task Delete(UserProcedureFile userProcedureFile) =>
            await _UserProcedureFileRepository.Delete(userProcedureFile);

        public async Task DeleteRange(IEnumerable<UserProcedureFile> userProcedureFiles) =>
            await _UserProcedureFileRepository.DeleteRange(userProcedureFiles);

        public async Task Insert(UserProcedureFile userProcedureFile) =>
            await _UserProcedureFileRepository.Insert(userProcedureFile);

        public async Task InsertRange(IEnumerable<UserProcedureFile> userProcedureFiles) =>
            await _UserProcedureFileRepository.InsertRange(userProcedureFiles);

        public async Task<List<UserProcedureFile>> GetUserProcedureFiles(Guid userProcedureId)
            => await _UserProcedureFileRepository.GetUserProcedureFiles(userProcedureId);

        public async Task Update(UserProcedureFile userProcedureFile)
            => await _UserProcedureFileRepository.Update(userProcedureFile);
    }
}

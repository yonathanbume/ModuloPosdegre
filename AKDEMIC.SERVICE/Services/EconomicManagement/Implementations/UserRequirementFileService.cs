using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Implementations
{
    public class UserRequirementFileService : IUserRequirementFileService
    {
        private readonly IUserRequirementFileRepository _userRequirementFileRepository;

        public UserRequirementFileService(IUserRequirementFileRepository userRequirementFileRepository)
        {
            _userRequirementFileRepository = userRequirementFileRepository;
        }

        public async Task<DataTablesStructs.ReturnedData<UserRequirementFile>> GetUserRequirementFilesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await _userRequirementFileRepository.GetUserRequirementFilesDatatable(sentParameters, searchValue);
        }

        public async Task Delete(UserRequirementFile userRequirementFile) =>
            await _userRequirementFileRepository.Delete(userRequirementFile);

        public async Task Insert(UserRequirementFile userRequirementFile) =>
            await _userRequirementFileRepository.Insert(userRequirementFile);
        public IQueryable<UserRequirementFile> GetQueryable(Guid id)
            =>  _userRequirementFileRepository.GetQueryable(id);
        public async Task<object> GetUserRequirementFilesDetail(Guid id)
            => await _userRequirementFileRepository.GetUserRequirementFilesDetail(id);
        public async Task<UserRequirementFile> Get(Guid id)
            => await _userRequirementFileRepository.Get(id);
        public async Task AddRangeAsync(List<UserRequirementFile> userRequirementFiles)
            => await _userRequirementFileRepository.AddRange(userRequirementFiles);
        public async Task<List<UserRequirementFile>> GetLstDeleteds(List<Guid> LstDeleteds)
            => await _userRequirementFileRepository.GetLstDeleteds(LstDeleteds);
        public void RemoveRange(List<UserRequirementFile> userRequirementFiles)
            =>  _userRequirementFileRepository.RemoveRange(userRequirementFiles);
    }
}

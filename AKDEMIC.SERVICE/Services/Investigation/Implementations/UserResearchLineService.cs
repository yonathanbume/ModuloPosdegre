using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Investigation;
using AKDEMIC.REPOSITORY.Repositories.Investigation.Interfaces;
using AKDEMIC.SERVICE.Services.Investigation.Interfaces;

namespace AKDEMIC.SERVICE.Services.Investigation.Implementations
{
    public class UserResearchLineService : IUserResearchLineService
    {
        private readonly IUserResearchLineRepository _userResearchLineRepository;
        public UserResearchLineService(IUserResearchLineRepository userResearchLineRepository)
        {
            _userResearchLineRepository = userResearchLineRepository;
        }
        public async Task<int> Count()
        {
            return await _userResearchLineRepository.Count();
        }
        public async Task<IEnumerable<object>> GetUserResearchLines()
        {
            return await _userResearchLineRepository.GetUserResearchLines();
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetUserResearchLinesDatatable(DataTablesStructs.SentParameters sentParameters, string userId, Guid? careerId, Guid? categoryId, Guid? disciplineId, string searchValue = null)
        {
            return await _userResearchLineRepository.GetUserResearchLinesDatatable(sentParameters, userId, careerId, categoryId, disciplineId, searchValue);
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetTeacherResearchLinesDatatable(DataTablesStructs.SentParameters sentParameters, string userId, string searchValue = null)
        {
            return await _userResearchLineRepository.GetTeacherResearchLinesDatatable(sentParameters, userId, searchValue);
        }
        public async Task Accept(Guid id)
        {
            await _userResearchLineRepository.Accept(id);
        }
        public async Task Deny(Guid id)
        {
            await _userResearchLineRepository.Deny(id);
        }
        public async Task DeleteById(Guid id)
        {
            await _userResearchLineRepository.DeleteById(id);
        }
        public async Task Insert(UserResearchLine userResearchLine)
        {
            await _userResearchLineRepository.Insert(userResearchLine);
        }
    }
}

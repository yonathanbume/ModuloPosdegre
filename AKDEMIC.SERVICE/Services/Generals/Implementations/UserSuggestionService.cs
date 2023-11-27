using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Generals.Implementations
{
    public class UserSuggestionService: IUserSuggestionService
    {
        private readonly IUserSuggestionRepository _userSuggestionRepository;

        public UserSuggestionService(
            IUserSuggestionRepository userSuggestionRepository)
        {
            _userSuggestionRepository = userSuggestionRepository;
        }

        public Task Delete(UserSuggestion userSuggestion)
            => _userSuggestionRepository.Delete(userSuggestion);

        public Task<UserSuggestion> Get(Guid id)
            => _userSuggestionRepository.Get(id);

        public Task<IEnumerable<UserSuggestion>> GetAll()
            => _userSuggestionRepository.GetAll();

        public Task<DataTablesStructs.ReturnedData<object>> GetAllUserSuggestionDatatable(DataTablesStructs.SentParameters sentParameters, string userId = null, string searchValue = null)
            => _userSuggestionRepository.GetAllUserSuggestionDatatable(sentParameters, userId, searchValue);

        public Task Insert(UserSuggestion userSuggestion)
            => _userSuggestionRepository.Insert(userSuggestion);

        public Task Update(UserSuggestion userSuggestion)
            => _userSuggestionRepository.Update(userSuggestion);
    }
}

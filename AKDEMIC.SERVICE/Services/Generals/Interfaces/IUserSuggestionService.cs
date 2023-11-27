using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Generals.Interfaces
{
    public interface IUserSuggestionService
    {
        Task<UserSuggestion> Get(Guid id);
        Task<IEnumerable<UserSuggestion>> GetAll();
        Task Insert(UserSuggestion userSuggestion);
        Task Update(UserSuggestion userSuggestion);
        Task Delete(UserSuggestion userSuggestion);

        Task<DataTablesStructs.ReturnedData<object>> GetAllUserSuggestionDatatable(DataTablesStructs.SentParameters sentParameters,string userId = null, string searchValue = null);
    }
}

using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces
{
    public interface IUserSuggestionRepository: IRepository<UserSuggestion>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetAllUserSuggestionDatatable(DataTablesStructs.SentParameters sentParameters,string userId = null, string searchValue = null);
    }
}

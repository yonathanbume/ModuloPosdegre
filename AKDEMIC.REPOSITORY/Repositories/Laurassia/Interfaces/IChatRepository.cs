using AKDEMIC.ENTITIES.Models.Laurassia;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Laurassia.Templates;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Laurassia.Interfaces
{
    public interface IChatRepository:IRepository<Chat>
    {
        Task<Chat> SingleOrDefaultByConditions(string emisorId = null, string receptorId = null);
        Task<Chat> GetFromUsers(string receptorId, string emisorId);
        Task<Chat> GetWithIncludes(Guid id);
        Task<List<ChatTemplate>> HasUnreadMessages(string receptorId);
        Task<object> ChatList(string userId, List<string> ConnectedUsers, string name);
    }
}

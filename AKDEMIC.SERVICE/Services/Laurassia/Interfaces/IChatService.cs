using AKDEMIC.ENTITIES.Models.Laurassia;
using AKDEMIC.REPOSITORY.Repositories.Laurassia.Templates;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Laurassia.Interfaces
{
    public interface IChatService
    {
        Task<Chat> Get(Guid id);
        Task<Chat> GetWithIncludes(Guid id);
        Task<Chat> GetFromUsers(string receptorId , string emisorId);
        Task Insert(Chat chat);
        Task Delete(Chat chat);
        Task Update(Chat chat);
        Task<Chat> SingleOrDefaultByConditions(string emisorId = null, string receptorId = null);
        Task<List<ChatTemplate>> HasUnreadMessages(string receptorId);
        Task<object> ChatList(string userId, List<string> ConnectedUsers, string name);
    }
}

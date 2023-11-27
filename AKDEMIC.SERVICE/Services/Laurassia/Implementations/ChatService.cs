using AKDEMIC.ENTITIES.Models.Laurassia;
using AKDEMIC.REPOSITORY.Repositories.Laurassia.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Laurassia.Templates;
using AKDEMIC.SERVICE.Services.Laurassia.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Laurassia.Implementations
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;

        public ChatService(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        public async Task<object> ChatList(string userId, List<string> ConnectedUsers, string name)
        {
            return await _chatRepository.ChatList(userId, ConnectedUsers, name);
        }

        public async Task Delete(Chat chat)
        {
            await _chatRepository.Delete(chat);
        }

        public async Task<Chat> Get(Guid id)
        {
            return await _chatRepository.Get(id);
        }

        public async Task<Chat> GetFromUsers(string receptorId, string emisorId)
        {
            return await _chatRepository.GetFromUsers(receptorId, emisorId);
        }

        public async Task<Chat> GetWithIncludes(Guid id)
        {
            return await _chatRepository.GetWithIncludes(id);
        }

        public async Task<List<ChatTemplate>> HasUnreadMessages(string receptorId)
        {
            return await _chatRepository.HasUnreadMessages(receptorId);
        }

        public async Task Insert(Chat chat)
        {
            await _chatRepository.Insert(chat);
        }

        public async Task<Chat> SingleOrDefaultByConditions(string emisorId = null, string receptorId = null)
        {
            return await _chatRepository.SingleOrDefaultByConditions(emisorId, receptorId);
        }

        public async Task Update(Chat chat)
        {
            await _chatRepository.Update(chat);
        }
    }
}

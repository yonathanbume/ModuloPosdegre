using AKDEMIC.ENTITIES.Models.Laurassia;
using AKDEMIC.REPOSITORY.Repositories.Laurassia.Interfaces;
using AKDEMIC.SERVICE.Services.Laurassia.Interfaces;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Laurassia.Implementations
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;

        public MessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task Delete(Message message)
        {
            await _messageRepository.Delete(message);
        }

        public async Task<Message> Get(Guid id)
        {
            return await _messageRepository.Get(id);
        }

        public async Task Insert(Message message)
        {
            await _messageRepository.Insert(message);
        }

        public async Task Update(Message message)
        {
            await _messageRepository.Update(message);
        }
    }
}

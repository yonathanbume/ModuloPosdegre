using AKDEMIC.ENTITIES.Models.Laurassia;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Laurassia.Interfaces
{
    public interface IMessageService
    {
        Task<Message> Get(Guid id);
        Task Insert(Message message);
        Task Delete(Message message);
        Task Update(Message message);
    }
}

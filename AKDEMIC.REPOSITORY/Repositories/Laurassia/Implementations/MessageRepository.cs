using AKDEMIC.ENTITIES.Models.Laurassia;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Laurassia.Interfaces;

namespace AKDEMIC.REPOSITORY.Repositories.Laurassia.Implementations
{
    public class MessageRepository:Repository<Message> , IMessageRepository
    {
        public MessageRepository(AkdemicContext context) : base(context) { }
    }
}

using AKDEMIC.ENTITIES.Models;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Geo.Interfaces;

namespace AKDEMIC.REPOSITORY.Repositories.Geo.Implementations
{
    public class ConnectionRepository : Repository<Connection>, IConnectionRepository
    {
        public ConnectionRepository(AkdemicContext context) : base(context) { }
    }
}

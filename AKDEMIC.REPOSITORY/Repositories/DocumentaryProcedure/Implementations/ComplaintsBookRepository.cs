using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces;

namespace AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Implementations
{
    public class ComplaintsBookRepository : Repository<Complaint>, IComplaintsBookRepository
    {
        public ComplaintsBookRepository(AkdemicContext context) : base(context)
        {
        }
    }
}

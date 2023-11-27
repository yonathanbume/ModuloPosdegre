using AKDEMIC.ENTITIES.Models.VisitManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.VisitManagement.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.VisitManagement.Implementations
{
    public class VisitInformationRepository : Repository<VisitorInformation>, IVisitInformationRepository
    {
        public VisitInformationRepository(AkdemicContext context) : base(context) { }

        public async Task<object> GetVisitsByVisitorsChart()
        {
            var visits = await _context.VisitorInformations
                .Select(x => new
                {
                    externalUserId = x.ExternalUser.Id,
                    fullname = $"{x.ExternalUser.DocumentNumber}-{x.ExternalUser.FullName}",
                }).ToArrayAsync();

            var result = visits
                .GroupBy(x => x.externalUserId)
                .Select(x => new
                {
                    name = x.Select(y => y.fullname).FirstOrDefault(),
                    y = x.Count()
                })
                .ToList();

            return result;
        }
    }
}

using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Portal;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Implementations
{
    public class TransparencyPortalRegulationRepository : Repository<TransparencyPortalRegulation>, ITransparencyPortalRegulationRepository
    {
        public TransparencyPortalRegulationRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<object> GetDataTable(PaginationParameter paginationParameter)
        {
            var query = _context.TransparencyPortalRegulations.AsQueryable();


            switch (paginationParameter.SortField)
            {
                default:
                    query = paginationParameter.SortOrder.Equals(paginationParameter.BaseOrder) ? query.OrderByDescending(q => q.Id) : query.OrderBy(q => q.Id);
                    break;
            }
            var filterRecords = await query.CountAsync();

            var pagedList = await query
                    .Skip(paginationParameter.CurrentNumber)
                    .Take(paginationParameter.RecordsPerPage)
                    .Select(x => new
                    {
                        id = x.Id,
                        title = x.Title,
                        url = x.Url,
                        x.IsLink
                    })
                    .ToListAsync();

            return new
            {
                draw = ConstantHelpers.DATATABLE.SERVER_SIDE.SENT_PARAMETERS.DRAW_COUNTER,
                recordsTotal = filterRecords,
                recordsFiltered = filterRecords,
                data = pagedList
            };
        }

        public IQueryable<TransparencyPortalRegulation> GetIQueryable()
            => _context.TransparencyPortalRegulations.AsQueryable();

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Scale.Templates.Resolution;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Implementations
{
    public class ScaleSectionAnnexRepository : Repository<ScaleSectionAnnex>, IScaleSectionAnnexRepository
    {
        public ScaleSectionAnnexRepository(AkdemicContext context) : base(context) { }

        public async Task<int> GetScaleSectionAnnexesQuantity(string userId, Guid sectionId)
        {
            var records = await _context.ScaleSectionAnnexes
                .Where(x => x.UserId == userId && x.ScaleSectionId == sectionId)
                .CountAsync();

            return records;
        }

        public async Task<List<ScaleSectionAnnexTemplate>> GetScaleSectionAnnexesByPaginationParameters(string userId, Guid sectionId, PaginationParameter paginationParameter)
        {
            var query = _context.ScaleSectionAnnexes
                .Where(x => x.UserId == userId && x.ScaleSectionId == sectionId)
                .AsQueryable();

            switch (paginationParameter.SortField)
            {
                case "0":
                    query = paginationParameter.SortOrder.Equals(paginationParameter.BaseOrder) ? query.OrderByDescending(q => q.Name) : query.OrderBy(q => q.Name);
                    break;
                case "1":
                    query = paginationParameter.SortOrder.Equals(paginationParameter.BaseOrder) ? query.OrderByDescending(q => q.ExpeditionDate) : query.OrderBy(q => q.ExpeditionDate);
                    break;
                default:
                    query = paginationParameter.SortOrder.Equals(paginationParameter.BaseOrder) ? query.OrderByDescending(q => q.Name) : query.OrderBy(q => q.Name);
                    break;
            }

            var pagedList = await query.Skip(paginationParameter.CurrentNumber).Take(paginationParameter.RecordsPerPage)
                .Select(x => new ScaleSectionAnnexTemplate
                {
                    Id = x.Id,
                    Name = x.Name,
                    ExpeditionFormattedDate = (x.ExpeditionDate.HasValue ? x.ExpeditionDate.Value.ToLocalDateFormat() : null),
                    AnnexDocument = x.AnnexDocument,
                    Type = x.Type,
                    Condition = x.Condition
                }).ToListAsync();

            return pagedList;
        }
    }
}

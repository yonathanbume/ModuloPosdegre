using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Generals.Templates.PortalNew;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Implementations
{
    public class PortalNewRepository : Repository<PortalNew>, IPortalNewRepository
    {
        public PortalNewRepository(AkdemicContext context) : base(context) { }

        public async Task<List<PortalNewTemplate>> GeNextUpcomingNews(int newsCount = 5)
        {
            var portalNews = await _context.PortalNews
                .Select(x => new PortalNewTemplate
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    PublishDate = x.PublishDate.ToLocalDateFormat(),
                    PathPicture = x.Picture
                })
                .Take(newsCount)
                .ToListAsync();

            return portalNews;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetPortalNewsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            Expression<Func<PortalNew, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Title); break;
                case "1":
                    orderByPredicate = ((x) => x.Description); break;
                case "2":
                    orderByPredicate = ((x) => x.PublishDate); break;
            }

            var query = _context.PortalNews.AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Title.ToUpper().Contains(searchValue.ToUpper()));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.Title,
                    x.Description,
                    publishDate = x.PublishDate.ToLocalDateFormat()
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }
    }
}

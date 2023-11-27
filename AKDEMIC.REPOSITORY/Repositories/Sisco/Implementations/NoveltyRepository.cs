using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Sisco;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Sisco.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Sisco.Template;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Sisco.Implementations
{
    public class NoveltyRepository : Repository<Novelty>, INoveltyRepository
    {
        public NoveltyRepository(AkdemicContext context) : base(context) { }
        public async Task<DataTablesStructs.ReturnedData<NoveltyTemplate>> GetAllNoveltyDatatable(DataTablesStructs.SentParameters sentParameters, string name = null)
        {
            Expression<Func<Novelty, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Title;
                    break;
                case "2":
                    orderByPredicate = (x) => x.PublicationDate;
                    break;
                case "4":
                    orderByPredicate = (x) => x.NoveltyDate;
                    break;
                default:
                    orderByPredicate = (x) => x.PublicationDate;
                    break;
            }


            var query = _context.Novelties
                .AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(x => x.Title.Contains(name));
            }

            query = query.AsQueryable();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new NoveltyTemplate
                {
                    Id = x.Id,
                    Title = x.Title,
                    PublicationDate = $"{x.PublicationDate:dd-MM-yyyy hh:mm:ss tt}",
                    UrlImage = x.UrlImage,
                    UrlVideo = x.UrlVideo,
                    NoveltyDate = x.NoveltyDate
                }).ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<NoveltyTemplate>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<List<NoveltyTemplate>> GetNoveltyToHome()
        {
            var novelties = await _context.Novelties
                .Select(x => new NoveltyTemplate
                {
                    Id = x.Id,
                    UrlImage = x.UrlImage,
                    Title = x.Title,
                    NoveltyDate = x.NoveltyDate,
                    Description = x.Description
                }).ToListAsync();

            return novelties;
        }

        public async Task<List<NewTemplate>> GetNewToHome()
        {

            var news = await _context.Novelties
                .OrderByDescending(x => x.PublicationDate)
                .Select(x => new NewTemplate
                {
                    Id = x.Id,
                    UrlPicture = x.UrlImage,
                    Title = x.Title,
                    Date = x.NoveltyDate,
                    Summary = x.Description
                }).ToListAsync();

            return news;
        }
    }
}

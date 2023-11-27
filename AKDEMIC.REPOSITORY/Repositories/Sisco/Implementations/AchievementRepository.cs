using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
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
    public class AchievementRepository : Repository<Achievement>, IAchievementRepository
    {
        public AchievementRepository(AkdemicContext context) : base(context) { }
        public async Task<DataTablesStructs.ReturnedData<AchievementTemplate>> GetAllAchievementDatatable(DataTablesStructs.SentParameters sentParameters, string headline = null, byte? status = null)
        {
            Expression<Func<Achievement, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Headline;
                    break;
                case "2":
                    orderByPredicate = (x) => x.PublicationDate;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Status;
                    break;
                default:
                    orderByPredicate = (x) => x.PublicationDate;
                    break;
            }


            var query = _context.Achievements
                .AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            if (!string.IsNullOrEmpty(headline))
            {
                query = query.Where(q => q.Headline.Contains(headline));
            }

            if (status != 0)
                query = query.Where(q => q.Status == status);

            query = query.AsQueryable();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new AchievementTemplate
                {
                    Id = x.Id,
                    Headline = x.Headline,
                    UrlImage = x.UrlImage,
                    PublicationDate = $"{x.PublicationDate:dd-MM-yyyy hh:mm:ss tt}",
                    Status = ConstantHelpers.Galery.Status.STATUS[x.Status]
                }).ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<AchievementTemplate>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }
        public async Task<AchievementTemplate> GetAchievementById(Guid id)
        {
            var model = await _context.Achievements
                .Where(x => x.Id == id)
                .Select(x => new AchievementTemplate
                {
                    Id = x.Id,
                    Headline = x.Headline,
                    Description = x.Description,
                    UrlImage = x.UrlImage,
                    Status = ConstantHelpers.Galery.Status.STATUS[x.Status],
                    StatusId = x.Status == 1 ? true : false
                }).FirstAsync();

            return model;
        }

        public async Task<List<AchievementTemplate>> GetAchievementToHome()
        {
            var achievements = await _context.Achievements
                .Select(x => new AchievementTemplate
                {
                    Id = x.Id,
                    UrlImage = x.UrlImage,
                    StatusId = x.Status == 1 ? true : false,
                    Headline = x.Headline,
                    Description = x.Description
                }).ToListAsync();

            return achievements;
        }

    }
}

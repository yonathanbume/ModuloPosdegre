using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Portal;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Implementations
{
    public class TransparencyCompetitionRepository : Repository<TransparencyCompetition>, ITransparencyCompetitionRepository
    {
        public TransparencyCompetitionRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetListCompetition(DataTablesStructs.SentParameters paginationParameter, int year)
        {
            var query = _context.TransparencyCompetitions
                .Include(x=>x.TransparencyCompetitionFiles)
                .Include(x=>x.TransparencyCompetitionFiles)
               .AsQueryable();

            switch (paginationParameter.OrderColumn)
            {
                default:
                    query = query.OrderByDescendingCondition(paginationParameter.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, q => q.Id);
                    break;
            }

            if (year != 0)
            {
                query = query.Where(x => x.PublishDate.Year == year);
            }
            else
            {
                query = query.Where(x => x.PublishDate.Year == DateTime.UtcNow.Year);
            }

            var queryclient = await query.ToListAsync();
            var filterRecordsAsync = query.Count();
            var pagedList = queryclient
                        .Skip(paginationParameter.PagingFirstRecord)
                        .Take(paginationParameter.RecordsPerDraw)
                        .Select(x => new
                        {
                            announcement = x.Announcement,
                            title = x.Title,
                            state = ConstantHelpers.COMPETITION.State.VALUES.ContainsKey(x.State)
                                    ? ConstantHelpers.COMPETITION.State.VALUES[x.State]
                                    : "--",
                            type = ConstantHelpers.COMPETITION.Type.VALUES.ContainsKey(x.Type)
                                    ? ConstantHelpers.COMPETITION.Type.VALUES[x.Type]
                                    : "otros",
                            publishDate = x.ParsedPublishDate,
                            Base1 = x.TransparencyCompetitionFiles
                                        .Where(y => y.Type == ConstantHelpers.COMPETITION.FileType.BASE1)
                                        .Select(y => new
                                        {
                                            y.Id,
                                            y.FileExtension
                                        })
                                        .ToList(),
                            Act2 = x.TransparencyCompetitionFiles
                                        .Where(y => y.Type == ConstantHelpers.COMPETITION.FileType.ACT2)
                                        .Select(y => new
                                        {
                                            y.Id,
                                            y.FileExtension
                                        })
                                        .ToList(),
                            Act3 = x.TransparencyCompetitionFiles
                                        .Where(y => y.Type == ConstantHelpers.COMPETITION.FileType.ACT3)
                                        .Select(y => new
                                        {
                                            y.Id,
                                            y.FileExtension
                                        })
                                        .ToList(),
                            Act4 = x.TransparencyCompetitionFiles
                                        .Where(y => y.Type == ConstantHelpers.COMPETITION.FileType.ACT4)
                                        .Select(y => new
                                        {
                                            y.Id,
                                            y.FileExtension
                                        })
                                        .ToList(),
                            Act5 = x.TransparencyCompetitionFiles
                                        .Where(y => y.Type == ConstantHelpers.COMPETITION.FileType.ACT5)
                                        .Select(y => new
                                        {
                                            y.Id,
                                            y.FileExtension
                                        })
                                        .ToList(),
                        })
                        .ToList();

            var filterRecords = pagedList.Count();

            var result = new DataTablesStructs.ReturnedData<object>
            {
                Data = pagedList,
                DrawCounter = paginationParameter.DrawCounter,
                RecordsFiltered = filterRecordsAsync,
                RecordsTotal = filterRecords
            };
            return result;
        }
    }
}

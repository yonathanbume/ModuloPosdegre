using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Investigation;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Investigation.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.REPOSITORY.Repositories.Investigation.Implementations
{
    public class UserResearchLineRepository : Repository<UserResearchLine>, IUserResearchLineRepository
    {
        public UserResearchLineRepository(AkdemicContext context) : base(context) { }

        public async Task<IEnumerable<object>> GetUserResearchLines()
        {
            var query = _context.UserResearchLines.Select(x => new
            {
                id = x.Id,
                name = x.ResearchLine.Name,
                career = x.ResearchLine.Career.Name
            });
            return await query.ToListAsync();
        }

        public async Task Accept(Guid id)
        {
            var line = await _context.UserResearchLines.Where(x => x.Id == id).FirstAsync();
            line.Status = 2;
            line.ResponseDate = DateTime.UtcNow.ToDefaultTimeZone();
            await _context.SaveChangesAsync();
        }

        public async Task Deny(Guid id)
        {
            var line = await _context.UserResearchLines.Where(x => x.Id == id).FirstAsync();
            line.Status = 3;
            line.ResponseDate = DateTime.UtcNow.ToDefaultTimeZone();
            await _context.SaveChangesAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetUserResearchLinesDatatable(DataTablesStructs.SentParameters sentParameters, string userId, Guid? careerId, Guid? categoryId, Guid? disciplineId, string searchValue = null)
        {
            Expression<Func<UserResearchLine, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.ResearchLine.Name); break;
                case "1":
                    orderByPredicate = ((x) => x.ResearchLine.Career.Name); break;
                case "2":
                    orderByPredicate = ((x) => x.ResearchLine.ResearchCategory.Name); break;
                case "3":
                    orderByPredicate = ((x) => x.ResearchLine.ResearchDiscipline.Name); break;
                case "4":
                    orderByPredicate = ((x) => x.ResearchLine.ResearchDiscipline.ResearchSubArea.Name); break;
                case "5":
                    orderByPredicate = ((x) => x.ResearchLine.ResearchDiscipline.ResearchSubArea.ResearchArea.Name); break;
                default:
                    orderByPredicate = ((x) => x.ResearchLine.Name); break;
            }

            IQueryable<UserResearchLine> query = _context.UserResearchLines.AsNoTracking();

            query = query.Where(x => x.UserId == userId);

            if (!String.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.ResearchLine.Name.ToUpper().Contains(searchValue));
            }

            if (careerId.HasValue)
            {
                query = query.Where(x => x.ResearchLine.CareerId == careerId);
            }

            if (categoryId.HasValue)
            {
                query = query.Where(x => x.ResearchLine.ResearchCategoryId == categoryId);
            }

            if (disciplineId.HasValue)
            {
                query = query.Where(x => x.ResearchLine.ResearchDisciplineId == disciplineId);
            }

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    status = x.Status,
                    requestDate = x.RequestDate.ToString("dd/MM/yyyy hh:mm"),
                    responseDate = x.ResponseDate == null ? "-" : x.ResponseDate.Value.ToString("dd/MM/yyyy hh:mm"),
                    name = x.ResearchLine.Name,
                    career = x.ResearchLine.Career.Name,
                    category = x.ResearchLine.ResearchCategory.Name,
                    discipline = x.ResearchLine.ResearchDiscipline.Name,
                    subarea = x.ResearchLine.ResearchDiscipline.ResearchSubArea.Name,
                    area = x.ResearchLine.ResearchDiscipline.ResearchSubArea.ResearchArea.Name
                }).ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetTeacherResearchLinesDatatable(DataTablesStructs.SentParameters sentParameters, string userId, string searchValue = null)
        {
            Expression<Func<UserResearchLine, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.ResearchLine.Name); break;
                case "1":
                    orderByPredicate = ((x) => x.ResearchLine.Career.Name); break;
                default:
                    orderByPredicate = ((x) => x.ResearchLine.Name); break;
            }

            IQueryable<UserResearchLine> query = _context.UserResearchLines.Where(x => x.Status != 0).AsNoTracking();

            if (!String.IsNullOrEmpty(userId))
            {
                query = query.Where(x => x.UserId == userId);
            }

            if (!String.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.ResearchLine.Name.ToUpper().Contains(searchValue));
            }

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    name = x.ResearchLine.Name,
                    career = x.ResearchLine.Career.Name,
                    status = x.Status
                }).ToListAsync();

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

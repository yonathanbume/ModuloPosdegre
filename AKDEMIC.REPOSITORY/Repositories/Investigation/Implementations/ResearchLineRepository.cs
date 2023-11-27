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
    public class ResearchLineRepository : Repository<ResearchLine>, IResearchLineRepository
    {
        public ResearchLineRepository(AkdemicContext context) : base(context) { }

        public async Task<bool> AnyResearchLineByName(string name, Guid? id)
        {
            if (id == null)
            {
                return await _context.ResearchLines.Where(x => x.Name == name).AnyAsync();
            }
            else
            {
                return await _context.ResearchLines.Where(x => x.Name == name && x.Id != id).AnyAsync();
            }
        }

        public async Task<object> GetResearchLine(Guid id)
        {
            var query = _context.ResearchLines.Select(x => new
            {
                id = x.Id,
                name = x.Name,
                active = x.Active,
                careerId = x.CareerId,
                categoryId = x.ResearchCategoryId,
                disciplineId = x.ResearchDisciplineId,
                subareaId = x.ResearchDiscipline.ResearchSubAreaId,
                areaId = x.ResearchDiscipline.ResearchSubArea.ResearchAreaId
            });
            return await query.FirstOrDefaultAsync(x => x.id == id);
        }

        public async Task<IEnumerable<object>> GetResearchLines()
        {
            var query = _context.ResearchLines.Select(x => new
            {
                id = x.Id,
                name = x.Name,
                active = x.Active,
                career = x.Career.Name
            });
            return await query.ToListAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetResearchLinesDatatable(DataTablesStructs.SentParameters sentParameters, Guid? careerId, Guid? categoryId, Guid? disciplineId, string searchValue = null)
        {
            try
            {
                Expression<Func<ResearchLine, dynamic>> orderByPredicate = null;
                switch (sentParameters.OrderColumn)
                {
                    case "0":
                        orderByPredicate = ((x) => x.Name); break;
                    case "1":
                        orderByPredicate = ((x) => x.Career.Name); break;
                    case "2":
                        orderByPredicate = ((x) => x.ResearchCategory.Name); break;
                    case "3":
                        orderByPredicate = ((x) => x.ResearchDiscipline.Name); break;
                    case "4":
                        orderByPredicate = ((x) => x.ResearchDiscipline.ResearchSubArea.Name); break;
                    case "5":
                        orderByPredicate = ((x) => x.ResearchDiscipline.ResearchSubArea.ResearchArea.Name); break;
                    case "6":
                        orderByPredicate = ((x) => x.Active); break;
                    default:
                        orderByPredicate = ((x) => x.Name); break;
                }

                IQueryable<ResearchLine> query = _context.ResearchLines.AsNoTracking();

                if (!String.IsNullOrEmpty(searchValue))
                {
                    query = query.Where(x => x.Name.ToUpper().Contains(searchValue));
                }

                if (careerId.HasValue)
                {
                    query = query.Where(x => x.CareerId == careerId.Value);
                }

                if (categoryId.HasValue)
                {
                    query = query.Where(x => x.ResearchCategoryId == categoryId.Value);
                }

                if (disciplineId.HasValue)
                {
                    query = query.Where(x => x.ResearchDisciplineId == disciplineId.Value);
                }

                int recordsFiltered = await query.CountAsync();
                var data = await query
                    .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                    .Skip(sentParameters.PagingFirstRecord)
                    .Take(sentParameters.RecordsPerDraw)
                    .Select(x => new
                    {
                        id = x.Id,
                        name = x.Name,
                        active = x.Active,
                        career = x.Career.Name,
                        category = x.ResearchCategory.Name,
                        discipline = x.ResearchDiscipline.Name,
                        subarea = x.ResearchDiscipline.ResearchSubArea.Name,
                        area = x.ResearchDiscipline.ResearchSubArea.ResearchArea.Name
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
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetCompanyResearchLinesDatatable(DataTablesStructs.SentParameters sentParameters, string userId)
        {
            Expression<Func<UserResearchLine, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.ResearchLine.Name); break;
                case "1":
                    orderByPredicate = ((x) => x.ResearchLine.ResearchCategory.Name); break;
                case "2":
                    orderByPredicate = ((x) => x.ResearchLine.ResearchDiscipline.Name); break;
                case "3":
                    orderByPredicate = ((x) => x.ResearchLine.ResearchDiscipline.ResearchSubArea.Name); break;
                case "4":
                    orderByPredicate = ((x) => x.ResearchLine.ResearchDiscipline.ResearchSubArea.ResearchArea.Name); break;
                default:
                    orderByPredicate = ((x) => x.ResearchLine.Name); break;
            }

            IQueryable<UserResearchLine> query = _context.UserResearchLines.Where(x => x.UserId == userId).AsNoTracking();

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    line = x.ResearchLine.Name,
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
    }
}

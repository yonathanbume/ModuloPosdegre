using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Implementations
{
    public class CatalogActivityRepository : Repository<CatalogActivity>, ICatalogActivityRepository
    {
        public CatalogActivityRepository(AkdemicContext context) : base(context) { }

        public async Task<DateTime> GetLastDate()
        {
            var result = await _context.CatalogActivities.OrderByDescending(x => x.CreateAt).Select(x => x.CreateAt).FirstOrDefaultAsync();

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetCatalogActivityDatatable(DataTablesStructs.SentParameters sentParameters, string search = null)
        {
            Expression<Func<CatalogActivity, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Year;
                    break;
                case "1":
                    orderByPredicate = (x) => x.SectionEjec;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Type;
                    break;
                case "3":
                    orderByPredicate = (x) => x.General;
                    break;
                case "4":
                    orderByPredicate = (x) => x.Name;
                    break;
                case "5":
                    orderByPredicate = (x) => x.Group;
                    break;
                case "6":
                    orderByPredicate = (x) => x.Status;
                    break;
                case "7":
                    orderByPredicate = (x) => x.NameGroup;
                    break;
                case "8":
                    orderByPredicate = (x) => x.Level;
                    break;
                case "9":
                    orderByPredicate = (x) => x.TypePpto;
                    break;
                case "10":
                    orderByPredicate = (x) => x.TypeUse;
                    break;
                case "11":
                    orderByPredicate = (x) => x.IdActPoi;
                    break;
                default:
                    orderByPredicate = (x) => x.CreateAt;
                    break;
            }


            var query = _context.CatalogActivities
                .AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            if (!string.IsNullOrEmpty(search))
                query = query
                    .Where(x => x.Name.ToUpper().Contains(search)
                    || x.NameGroup.ToUpper().Contains(search));

            query = query.AsQueryable();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    year = x.Year,
                    sectionEjec = x.SectionEjec,
                    type = x.Type,
                    general = x.General,
                    name = x.Name,
                    group = x.Group,
                    status = x.Status,
                    nameGroup = x.NameGroup,
                    level = x.Level,
                    typePpto = x.TypePpto,
                    typeUse = x.TypeUse,
                    idActPoi = x.IdActPoi
                }).ToListAsync();

            var recordsTotal = data.Count;

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

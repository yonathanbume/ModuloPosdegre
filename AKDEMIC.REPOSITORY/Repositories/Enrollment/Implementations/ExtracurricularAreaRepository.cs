using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Implementations
{
    public class ExtracurricularAreaRepository : Repository<ExtracurricularArea>, IExtracurricularAreaRepository
    {
        public ExtracurricularAreaRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<object> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string search = null)
        {
            Expression<Func<ExtracurricularArea, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Name;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Type;
                    break;
                case "2":
                    orderByPredicate = (x) => x.CreatedAt;
                    break;
                default:
                    break;
            }

            var query = _context.ExtracurricularAreas
                .AsNoTracking();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Name.ToUpper().Contains(search.ToUpper()));

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    name = x.Name,
                    type = x.Type,
                    typeText = x.Type == CORE.Helpers.ConstantHelpers.ExtracurricularArea.Type.COURSE ? "Curso" : "Actividad",
                    createdAt = x.CreatedAt.ToLocalDateFormat()
                }).ToListAsync();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }
    }
}

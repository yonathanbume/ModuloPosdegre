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
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class ExtracurricularCourseRepository : Repository<ExtracurricularCourse>, IExtracurricularCourseRepository
    {
        public ExtracurricularCourseRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<object> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string search = null)
        {
            Expression<Func<ExtracurricularCourse, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Code;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Name;
                    break;
                case "2":
                    orderByPredicate = (x) => x.ExtracurricularArea.Name;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Credits;
                    break;
                case "4":
                    orderByPredicate = (x) => x.Price;
                    break;
                default:
                    break;
            }

            var query = _context.ExtracurricularCourses
                .AsNoTracking();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Name.ToUpper().Contains(search.ToUpper()) || x.Code.ToUpper().Contains(search.ToUpper()));

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    code = x.Code,
                    name = x.Name,
                    credits = x.Credits,
                    area = x.ExtracurricularArea.Name,
                    areaId = x.ExtracurricularAreaId,
                    description = x.Description,
                    price = x.Price,
                }).ToListAsync();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }

        public Task<ExtracurricularCourse> GetByCode(string code)
            => _context.ExtracurricularCourses
                .Where(x => x.Code == code)
                .FirstOrDefaultAsync();

        public async Task<IEnumerable<Select2Structs.Result>> GetExtracurricularCoursesSelect2ClientSide()
        {
            var result = await _context.ExtracurricularCourses.Select(x => new Select2Structs.Result
            {
                Id = x.Id,
                Text = x.Name
            }).ToArrayAsync();

            return result;
        }
    }
}

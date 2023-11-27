using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.ContinuingEducation.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.ContinuingEducation.Implementations
{
    public class FormationCourseAreaRepository:Repository<ENTITIES.Models.ContinuingEducation.CourseArea> , IFormationCourseAreaRepository
    {
        public FormationCourseAreaRepository(AkdemicContext context) : base(context) { }

        public async Task<bool> AnyByName(string name, Guid? id = null)
        {
            return await _context.FormationCourseAreas.AnyAsync(x => x.Name.ToUpper() == name.ToUpper() && x.Id != id);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllFormationCourseAreasDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            Expression<Func<ENTITIES.Models.ContinuingEducation.CourseArea, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Name;
                    break;
            }

            var query = _context.FormationCourseAreas.AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Name.ToUpper().Contains(searchValue.ToUpper()));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
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

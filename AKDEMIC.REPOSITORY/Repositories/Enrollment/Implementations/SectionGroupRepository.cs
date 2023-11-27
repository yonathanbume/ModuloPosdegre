using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Implementations
{
    public class SectionGroupRepository : Repository<SectionGroup> , ISectionGroupRepository
    {
        public SectionGroupRepository(AkdemicContext context) : base(context) { }

        public async Task<bool> AnyByCode(string code, Guid? ignoredId = null)
            => await _context.SectionGroups.AnyAsync(x => x.Code.Trim().ToLower().Equals(code.Trim().ToLower()) && x.Id != ignoredId);

        public async Task<IEnumerable<Select2Structs.Result>> GetSectionGroupsSelect2ClientSide()
        {
            var result = await _context.SectionGroups
                .Select(x => new Select2Structs.Result
                {
                    Id = x.Id,
                    Text = x.Code
                })
                .ToArrayAsync();

            return result;
        }

        public async Task<IEnumerable<Select2Structs.Result>> GetSectionGroupBySectionSelect2lientSide(Guid sectionId)
        {
            
            var result = await _context.ClassSchedules.Where(x=>x.SectionId == sectionId && x.SectionGroupId.HasValue)
                .Select(x => new Select2Structs.Result
                {
                    Id = x.SectionGroupId,
                    Text = x.SectionGroup.Code
                })
                .Distinct()
                .ToArrayAsync();

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetSectionGroupDatatable(DataTablesStructs.SentParameters parameters, string searchValue)
        {
            Expression<Func<SectionGroup, dynamic>> orderByPredicate = null;
            switch (parameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Code); break;
                default:
                    orderByPredicate = ((x) => x.Id); break;
            }

            var query = _context.SectionGroups.AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = searchValue.Trim().ToLower();
                query = query.Where(x => x.Code.ToLower().Contains(searchValue) || x.Description.ToLower().Contains(searchValue));
            }

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByCondition(parameters.OrderDirection, orderByPredicate)
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                  .Select(x => new 
                  {
                      x.Id,
                      description = string.IsNullOrEmpty(x.Description)?"-":x.Description,
                      x.Code
                  })
                  .ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }
    }
}

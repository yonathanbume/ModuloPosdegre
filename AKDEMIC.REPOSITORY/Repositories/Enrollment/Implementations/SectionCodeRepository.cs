using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Implementations
{
    public sealed class SectionCodeRepository : Repository<SectionCode>, ISectionCodeRepository
    {
        public SectionCodeRepository(AkdemicContext context) : base(context){ }

        public async Task<object> GetFakeSelect2()
        {
            var codes = await _context.SectionCodes
                .OrderBy(x=>x.Order)
                .Select(
                    x => new
                    {
                        id = x.Description,
                        text = x.Description
                    }
                )
                .ToListAsync();

            return codes;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetSectionCodeDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            Expression<Func<SectionCode, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                default:
                    orderByPredicate = (x) => x.Id;
                    break;
            }
            var query = _context.SectionCodes.AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Description.ToUpper().Contains(searchValue.ToUpper()));
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                    .Select(x => new
                    {
                        x.Id,
                        x.Description,
                    })
                    .Skip(sentParameters.PagingFirstRecord)
                    .Take(sentParameters.RecordsPerDraw)
                    .ToListAsync();

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
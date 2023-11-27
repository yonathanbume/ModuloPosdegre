using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Implementations
{
    public class InstitutionalWelfareProductRepository : Repository<InstitutionalWelfareProduct>, IInstitutionalWelfareProductRepository
    {
        public InstitutionalWelfareProductRepository(AkdemicContext context) : base(context)
        {

        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetInstitutionalWelfareProductDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            var query = _context.InstitutionalWelfareProducts.AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Name.ToLower().Contains(searchValue.ToLower()));
            }

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new { x.Id, x.Name }).ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<object> InstitutionalWelfareProductSelect2()
        {
            var result = await _context.InstitutionalWelfareProducts
                .Select(x => new
                {
                    id = x.Id,
                    text = x.Name
                })
                .OrderBy(x => x.text)
                .ToListAsync();

            return result;
        }
    }
}

using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class WelfareCategoryRepository : Repository<WelfareCategory>, IWelfareCategoryRepository
    {
        public WelfareCategoryRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetWelfareCategories(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            var query = _context.WelfareCategories.AsQueryable();            
            
            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Name.ToLower().Contains(searchValue.ToLower()));
            }    

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new { x.Id, x.Name, x.ColorRGB}).ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<object> WelfareCategoriesSelect2(bool hasAll = false)
        {
            var result = await _context.WelfareCategories.Select(x => new {
                id = x.Id,
                text = x.Name
            }).ToListAsync();
            if (hasAll == true)
            {
                result.Insert(0, new { id = Guid.Empty, text = "Todas" });
            }
            return result;
        }
    }
}

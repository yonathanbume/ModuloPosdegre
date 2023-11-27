using AKDEMIC.CORE.Extensions;
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
    public class InstitutionalWelfareUserProductRepository : Repository<InstitutionalWelfareUserProduct>, IInstitutionalWelfareUserProductRepository
    {
        public InstitutionalWelfareUserProductRepository(AkdemicContext context) : base(context)
        {

        }

        public async Task<bool> ExistByProduct(Guid productId)
        {
            return await _context.InstitutionalWelfareUserProducts.AnyAsync(x=>x.InstitutionalWelfareProductId == productId);
        }

        public async Task<object> GetReport()
        {
            var query = _context.InstitutionalWelfareUserProducts.AsQueryable();

            var result = await query                
                .Select(x => new
                {
                    Name = (DateTime.UtcNow > x.EndTime) ? "No entregado" : x.WasReturned ? "Devuelto" : "Prestado"                 
                })
                .GroupBy(x => new { x.Name })
                .Select(x => new {
                    x.Key.Name,
                    counter = x.Count()
                })
                .ToListAsync();

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetUserProductDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            var query = _context.InstitutionalWelfareUserProducts.AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.User.FullName.ToLower().Contains(searchValue.ToLower()) || x.InstitutionalWelfareProduct.Name.ToLower().Contains(searchValue.ToLower()));
            }

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x =>
                    new { x.Id,
                        x.WasReturned,
                        x.Quantity,
                        startTime = x.StartTime.ToLocalDateFormat(),
                        endTime = x.EndTime.ToLocalDateFormat(),
                        returnDate = x.ReturnDate.ToLocalDateFormat(),
                        fullName = x.User.FullName,
                        exceedLimitDate = DateTime.UtcNow > x.EndTime,
                        productName = x.InstitutionalWelfareProduct.Name}).ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<InstitutionalWelfareUserProduct> GetWithIncludes(Guid id)
        {
            return await _context.InstitutionalWelfareUserProducts.Include(x => x.InstitutionalWelfareProduct).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}

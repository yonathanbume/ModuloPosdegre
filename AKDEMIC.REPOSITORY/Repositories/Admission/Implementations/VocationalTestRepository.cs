using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Admission.Implementations
{
    public class VocationalTestRepository: Repository<VocationalTest>, IVocationalTestRepository
    {
        public VocationalTestRepository(AkdemicContext context): base(context)
        {

        }

        public async Task<bool> ExistAnyActive(Guid? Id)
        {
            var query = _context.VocationalTests.AsQueryable();
            if (Id.HasValue)
            {
                query = query.Where(x => x.Id != Id);
            }
            return await query.AnyAsync(x => x.IsActive);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> VocationalTestDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
           var query =  _context.VocationalTests.AsQueryable();
            if (!String.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Name.Contains(searchValue));
            }

            var recordsFiltered = 0;

            recordsFiltered = await query.CountAsync();
            var data = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.IsActive
                })
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

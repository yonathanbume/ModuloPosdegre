using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.PreprofesionalPractice;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.PreprofesionalPractice.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.PreprofesionalPractice.Implementations
{
    public class InternshipAspectRepository : Repository<InternshipAspect>, IInternshipAspectRepository
    {
        public InternshipAspectRepository(AkdemicContext context) : base(context)
        {

        }

        public async Task<IEnumerable<InternshipAspect>> GetAllByType(byte? type = null, bool? ignoreQueryFilters = null)
        {
            var query = _context.InternshipAspects.AsNoTracking();

            if (ignoreQueryFilters.HasValue && ignoreQueryFilters.Value)
                query = query.IgnoreQueryFilters();

            if (type.HasValue)
                query = query.Where(x => x.Type == type);

            return await query.ToListAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters parameters, byte type)
        {
            var query = _context.InternshipAspects.Where(x=>x.Type == type).OrderBy(x=>x.CreatedAt).AsNoTracking();
            
            var recordsFiltered = await query.CountAsync();
            
            var data = await query
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Type
                })
                .ToListAsync();

            var recordsTotal = data.Count;

            var result = new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };

            return result;
        }
    }
}

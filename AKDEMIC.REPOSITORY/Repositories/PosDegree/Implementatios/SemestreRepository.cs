using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.PosDegree;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.PosDegree.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.PosDegree.Implementatios
{
    public class SemestreRepository : Repository<Semestre>, ISemestreRepository
    {

        public SemestreRepository(AkdemicContext context) : base(context)
        {

        }

        public async  Task<DataTablesStructs.ReturnedData<object>> GetSemestreDataTable(DataTablesStructs.SentParameters parameters1, string search)
        {
            var query = _context.Semestres.AsNoTracking();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.Name.ToLower().Contains(search.Trim().ToLower()));
            }
            var recorFilter = await query.CountAsync();
            var data = await query.Skip(parameters1.PagingFirstRecord)
                   .Take(parameters1.RecordsPerDraw).Select(x => new {
                       x.id,
                       x.Name,
                       x.StartDate,
                       x.EndDate
                   }).ToListAsync();
            var recordTotal = data.Count();
            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters1.DrawCounter,
                RecordsFiltered = recorFilter,
                RecordsTotal = recordTotal
            };
        }
    }
}

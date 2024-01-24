using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.PosDegree;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.PosDegree.Interfaces;
using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.PosDegree.Implementatios
{
    public class AsignaturaRepository : Repository<Asignatura>, IAsignaturaRepository
    {
        public AsignaturaRepository(AkdemicContext context) : base(context)
        {

        }

        public Task DownloadExcel(IXLWorksheet worksheet)
        {
            throw new NotImplementedException();
        }

        public async  Task<DataTablesStructs.ReturnedData<object>> GetAsignaturaDataTable(DataTablesStructs.SentParameters parameters1, string search)
        {
            var query = _context.Asignaturas.AsNoTracking();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.Code.ToLower().Contains(search.Trim().ToLower()));
            }
            var recorFilter = await query.CountAsync();
              
            var data = await query.Skip(parameters1.PagingFirstRecord)
                    .Take(parameters1.RecordsPerDraw).Select(x => new {
                        x.id,
                        x.Code,
                        x.NameAsignatura,
                        x.Credits,
                        x.PracticalHours,
                        x.TeoricasHours,
                        x.TotalHours,
                        x.Requisito
                       
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

        public Task<DataTablesStructs.ReturnedData<object>> GetMasterDataTable(DataTablesStructs.SentParameters parameters1, string search)
        {
            throw new NotImplementedException();
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Master entity)
        {
            throw new NotImplementedException();
        }

       
    }
}

using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Implementations
{
    public class ExecuteObservationRepository : Repository<ExecuteObservation>, IExecuteObservationRepository
    {
        public ExecuteObservationRepository(AkdemicContext context) :base(context) { }

        public async Task<object> GetExecutionObs(Guid id)
        {
            var data = await _context.ExecuteObservations.Where(x => x.Id == id)
                .Select(x => new
                {
                    x.Type,
                    x.Day,
                    x.Description,
                    endDatetime = $"{x.EndDatetime:dd/MM/yyyy}",
                    x.UrlFile,
                    fileName = x.UrlFile != null ? Path.GetFileName(x.UrlFile) : ""
        }).FirstOrDefaultAsync();

            return data;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetFiles(DataTablesStructs.SentParameters sentParameters, Guid id)
        {
            Expression<Func<ExecuteObservation, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                default:
                    orderByPredicate = (x) => x.UrlFile;
                    break;
            }


            var query = _context.ExecuteObservations.Where(x => x.Id == id).AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            query = query.AsQueryable();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new 
                {
                    Id = x.Id,
                    UrlFile = "Ejecución del Proyecto Archivo"
                }).ToListAsync();

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

using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Implementations
{
    public class WorkerPositionClassificationRepository : Repository<WorkerPositionClassification> , IWorkerPositionClassificationRepository
    {
        public WorkerPositionClassificationRepository(AkdemicContext context) :base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetWorkerPositionClassificationDatatable(DataTablesStructs.SentParameters sentParameters,int? status, string searchvalue = null)
        {
            Expression<Func<WorkerPositionClassification, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name); break;
                case "1":
                    orderByPredicate = ((x) => x.Status); break;
            }

            var query = _context.WorkerPositionClassifications.AsQueryable();

            if (status.HasValue)
                query = query.Where(x => x.Status == status);

            if (!string.IsNullOrEmpty(searchvalue))
                query = query.Where(x => x.Name.ToUpper().Contains(searchvalue.ToUpper()));

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                  .Select(x => new 
                  {
                      id = x.Id,
                      name = x.Name,
                      status = x.Status
                  })
                  .ToListAsync();


            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<IEnumerable<WorkerPositionClassification>> GetAll(byte? status, string search = null)
        {
            var query = _context.WorkerPositionClassifications.AsQueryable();

            if (status.HasValue)
                query = query.Where(x => x.Status == ConstantHelpers.STATES.ACTIVE);

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Name.Trim().ToLower().Contains(search.Trim().ToLower()));

            return await query.ToArrayAsync();
        }

        public async Task<bool> AnyByName(string name, Guid? ignoredId = null)
            => await _context.WorkerPositionClassifications.AnyAsync(x => x.Name == name && x.Id != ignoredId);
    }
}

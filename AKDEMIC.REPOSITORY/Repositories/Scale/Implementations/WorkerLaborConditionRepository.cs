using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Implementations
{
    public class WorkerLaborConditionRepository : Repository<WorkerLaborCondition>, IWorkerLaborConditionRepository
    {
        public WorkerLaborConditionRepository(AkdemicContext context) : base(context) { }

        public async Task<object> GetAllAsSelect2ClientSide(bool includeTitle = false)
        {
            var conditions = await _context.WorkerLaborConditions
                .Select(x => new 
                {
                    Id = $"{x.Id}",
                    Text = x.Name
                })
                .ToListAsync();

            if (includeTitle)
                conditions.Insert(0, new {Id = Guid.Empty.ToString(), Text = "Todos"});

            return conditions;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetWorkerLaborConditionDataTable(DataTablesStructs.SentParameters sentParameters,int? status,string searchValue = null)
        {
            Expression<Func<WorkerLaborCondition, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Name; break;
                case "1":
                    orderByPredicate = (x) => x.WorkerLaborRegime.Name; break;
                case "2":
                    orderByPredicate = (x) => x.Status; break;
            }

            var query = _context.WorkerLaborConditions.AsQueryable();

            if (status.HasValue)
                query = query.Where(x => x.Status == status);

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Name.ToUpper().Contains(searchValue.ToUpper()) 
                            || x.WorkerLaborRegime.Name.ToUpper().Contains(searchValue.ToUpper()));

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    name = x.Name,
                    status = x.Status,
                    workerLaborRegimen = x.WorkerLaborRegime.Name
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

        public async Task<bool> AnyByName(string name, Guid? laborRegimeId, Guid? ignoredId = null)
        {
            var query = _context.WorkerLaborConditions.AsQueryable();

            if (laborRegimeId != null)
                query = query.Where(x => x.WorkerLaborRegimeId == laborRegimeId);
            return await query.AnyAsync(x => x.Name.ToUpper() == name.ToUpper() && x.Id != ignoredId);
        }

        public async Task<IEnumerable<WorkerLaborCondition>> GetAllWithIncludes(int? status = null)
        {
            var query = _context.WorkerLaborConditions
                .Include(x => x.WorkerLaborRegime)
                .AsQueryable();

            if (status != null)
                query = query.Where(x => x.Status == status);

            return await query.ToListAsync();
        }
    }
}
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Implementations
{
    public sealed class WorkerLaborCategoryRepository : Repository<WorkerLaborCategory>, IWorkerLaborCategoryRepository
    {
        public WorkerLaborCategoryRepository(AkdemicContext context) : base(context) { }

        public async Task<bool> AnyByName(string name, Guid? laborRegimeId, Guid? ignoredId = null)
        {
            var query = _context.WorkerLaborCategories.AsQueryable();

            if (laborRegimeId != null)
                query = query.Where(x => x.WorkerLaborRegimeId == laborRegimeId);

            return await query.AnyAsync(x => x.Name == name && x.Id != ignoredId);
        }

        public async Task<object> GetSelect2ClientSide()
        {
            var result = await _context.WorkerLaborCategories
                .Select(x => new
                {
                    id = x.Id,
                    text = $"Reg. {x.WorkerLaborRegime.Name} - {x.Name}"
                })
                .ToListAsync();

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetWorkerLaborCategoryDatatable(DataTablesStructs.SentParameters sentParameters,int? status,string searchvalue = null)
        {
            Expression<Func<WorkerLaborCategory, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name); break;
                case "1":
                    orderByPredicate = ((x) => x.WorkerLaborRegime.Name); break;
                case "2":
                    orderByPredicate = ((x) => x.Status); break;
            }

            var query = _context.WorkerLaborCategories.AsQueryable();

            if (status.HasValue)
                query = query.Where(x => x.Status == status);

            if (!string.IsNullOrEmpty(searchvalue))
                query = query.Where(x => x.Name.ToUpper().Contains(searchvalue.ToUpper())
                                || x.WorkerLaborRegime.Name.ToUpper().Contains(searchvalue.ToUpper()));

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

        public async Task<object> GetWorkerLaborCategorySelect()
        {
            var result = await _context.WorkerLaborCategories
                .Where(x => x.Status == ConstantHelpers.STATES.ACTIVE)
                .Select(x => new
                {
                    id = x.Id,
                    text = $"Reg. {x.WorkerLaborRegime.Name} - {x.Name}"
                }).ToListAsync();

            return result;
        }
    }
}
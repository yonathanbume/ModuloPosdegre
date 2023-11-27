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
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Implementations
{
    public class WorkerRetirementSystemHistoryRepository : Repository<WorkerRetirementSystemHistory>, IWorkerRetirementSystemHistoryRepository
    {
        public WorkerRetirementSystemHistoryRepository(AkdemicContext context) : base(context) {  }

        public async Task<WorkerRetirementSystemHistory> GetActiveRetirementSystem(string userId)
            => await _context.WorkerRetirementSystemHistories.Include(x=>x.PrivateManagementPensionFund).Where(x => x.UserId == userId && x.Active).FirstOrDefaultAsync();

        public async Task<DataTablesStructs.ReturnedData<object>> GetWorkerRetirementSystemHistoryDatatable(DataTablesStructs.SentParameters sentParameters, string userId) 
        {
            Expression<Func<WorkerRetirementSystemHistory, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.RetirementSystem); break;
                case "1":
                    orderByPredicate = ((x) => x.JoiningDate); break;
                case "2":
                    orderByPredicate = ((x) => x.LeavingDate); break;
                case "3":
                    orderByPredicate = ((x) => x.AfpUserCode); break;
                case "4":
                    orderByPredicate = ((x) => x.PrivateManagementPensionFund.Name); break;
                case "5":
                    orderByPredicate = ((x) => x.Active); break;
                default:
                    orderByPredicate = ((x) => x.Active); break;
            }

            var query = _context.WorkerRetirementSystemHistories.Where(x => x.UserId == userId).AsQueryable();

            int recordsFiltered = await query.CountAsync();

            var dataDB = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new 
                {
                    x.Id,
                    x.RetirementSystem,
                    x.JoiningDate,
                    x.LeavingDate,
                    x.AfpUserCode,
                    privateManagementPensionFund = x.PrivateManagementPensionFund.Name,
                    x.Active
                })
                .ToListAsync();

            var data = dataDB
                .Select(x => new
                {
                    x.Id,
                    retirementSystem = ConstantHelpers.RETIREMENT_SYSTEM.VALUES[x.RetirementSystem],
                    joiningDate = x.JoiningDate.ToLocalDateFormat(),
                    levaingDate = x.LeavingDate.HasValue ? x.LeavingDate.ToLocalDateFormat() : "Sin Registrar",
                    afpUserCode = string.IsNullOrEmpty(x.AfpUserCode) ? "-" : x.AfpUserCode,
                    privateManagementPensionFund = string.IsNullOrEmpty(x.privateManagementPensionFund) ? "-" : x.privateManagementPensionFund,
                    status = x.Active ? "Activo" : "Inactivo"
                })
                .ToList();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task InsertWorkerRetirementSystem(WorkerRetirementSystemHistory entity)
        {
            var oldSystemRetirementSystem = await _context.WorkerRetirementSystemHistories.Where(x => x.UserId == entity.UserId).ToListAsync();

            await _context.WorkerRetirementSystemHistories.AddAsync(entity);

            oldSystemRetirementSystem.ForEach(x => { 
                x.Active = false ; 
                x.LeavingDate = (x.LeavingDate is null) ? DateTime.UtcNow : x.LeavingDate; 
            });

            await _context.SaveChangesAsync();
        }

        public async Task<bool> AnyActiveByUser(string userId, Guid? ignoredId = null)
            => await _context.WorkerRetirementSystemHistories.Where(x => x.UserId == userId && x.Id != ignoredId && x.Active).AnyAsync();
    }
}

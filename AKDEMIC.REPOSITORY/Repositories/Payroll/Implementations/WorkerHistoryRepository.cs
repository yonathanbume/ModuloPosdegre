using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Payroll;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Payroll.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Payroll.Implementations
{
    public class WorkerHistoryRepository : Repository<WorkerHistory>, IWorkerHistoryRepository
    {
        public WorkerHistoryRepository(AkdemicContext context) : base(context) { }

        public async Task FireWorkerById(Guid workerId)
        {
            var worker = await _context.Workers
                   .Where(x => x.Id == workerId)
                   .Include(x => x.User)
                   .ThenInclude(x => x.WorkerLaborInformation)
                   .Include(x => x.WorkerHistories)
                   .FirstOrDefaultAsync();

            var lastWorkerHistory = worker.WorkerHistories.Where(x => !x.IsFinished)
                .OrderByDescending(x => x.StartDate).FirstOrDefault();

            var workerHistory = lastWorkerHistory ?? new WorkerHistory
            {
                WorkerId = workerId,
                StartDate = worker.User.WorkerLaborInformation.EntryDate ?? DateTime.UtcNow
            };

            workerHistory.EndDate = DateTime.UtcNow;
            workerHistory.IsFinished = true;
            if (lastWorkerHistory == null) await _context.WorkerHistories.AddAsync(workerHistory);
            await _context.SaveChangesAsync();
        }

        public async Task<(IEnumerable<WorkerHistory> pagedList, int count)> GetAllByPaginationParameter(PaginationParameter paginationParameter, Guid workerId)
        {
            var query = _context.WorkerHistories
                .Where(x => x.WorkerId == workerId)
                .OrderByDescending(x => x.StartDate)
                .AsQueryable();
            if (!string.IsNullOrEmpty(paginationParameter.SearchValue))
                query = query.Where(x => x.StartDate.Year.ToString().Contains(paginationParameter.SearchValue)
                        || x.StartDate.Month.ToString().Contains(paginationParameter.SearchValue)
                        || x.StartDate.Day.ToString().Contains(paginationParameter.SearchValue)
                        || (x.EndDate.HasValue && (x.EndDate.Value.Year.ToString().Contains(paginationParameter.SearchValue)
                        || x.EndDate.Value.Month.ToString().Contains(paginationParameter.SearchValue)
                        || x.EndDate.Value.Day.ToString().Contains(paginationParameter.SearchValue))));
            var count = await query.CountAsync();
            var pagedList = await query
                .Skip(paginationParameter.CurrentNumber)
                .Take(paginationParameter.RecordsPerPage)
                .ToListAsync();
            return (pagedList, count);
        }

        public async Task RestoreWorkerById(Guid workerId)
        {
            var workerHistory = new WorkerHistory
            {
                WorkerId = workerId,
                StartDate = DateTime.UtcNow,
                IsFinished = false
            };
            await _context.WorkerHistories.AddAsync(workerHistory);
            await _context.SaveChangesAsync();
        }
    }
}

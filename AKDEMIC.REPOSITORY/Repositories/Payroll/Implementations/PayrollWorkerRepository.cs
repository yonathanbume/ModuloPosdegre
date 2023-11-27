using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Payroll.Interfaces;
using System;
using System.Threading.Tasks;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.ENTITIES.Models.Payroll;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using AKDEMIC.ENTITIES.Base;

namespace AKDEMIC.REPOSITORY.Repositories.Payroll.Implementations
{
    public class PayrollWorkerRepository : Repository<PayrollWorker>, IPayrollWorkerRepository
    {
        public PayrollWorkerRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task DeleteByPayrollId(Guid payrollId)
        {
            var toRemove = await _context.PayrollWorkers
                .Where(x => x.PayrollId == payrollId).ToListAsync();
            _context.PayrollWorkers.RemoveRange(toRemove);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<PayrollWorker>> GetAllByPayrollId(Guid payrollId)
            => await _context.PayrollWorkers
                .Include(x => x.Worker.User)
                .Where(x => x.PayrollId == payrollId)
                .ToListAsync();
        
        public async Task<IEnumerable<Guid>> GetIdsByPayrollId(Guid payrollId)
            => await _context.PayrollWorkers
                    .Where(x => x.PayrollId == payrollId)
                    .Select(x => x.Id).ToListAsync();

        public async Task<(IList<PayrollWorker> pagedList, int count)> GetPayrollWorkersByPayrollIdAndPaginationParameter(Guid payrollId, PaginationParameter paginationParameter)
        {
            var query = _context.PayrollWorkers
                .Where(x => x.PayrollId == payrollId)
                .AsQueryable();

            if (!string.IsNullOrEmpty(paginationParameter.SearchValue))
                query = query.Where(x => x.Worker.User.WorkerLaborInformation != null ?
                                    x.Worker.User.WorkerLaborInformation.WorkerCode.Contains(paginationParameter.SearchValue) : false ||
                                    x.Worker.User.Name.Contains(paginationParameter.SearchValue) ||
                                    x.Worker.User.MaternalSurname.Contains(paginationParameter.SearchValue) ||
                                    x.Worker.User.PaternalSurname.Contains(paginationParameter.SearchValue) ||
                                    x.Worker.User.Dni.Contains(paginationParameter.SearchValue) ||
                                    x.PayrollWorkerWageItemsCount.ToString().Contains(paginationParameter.SearchValue));

            var count = await query.CountAsync();

            switch (paginationParameter.SortField)
            {
                case "0":
                    query = paginationParameter.SortOrder == paginationParameter.BaseOrder
                        ? query.OrderByDescending(x => x.Worker.User.WorkerLaborInformation != null).ThenByDescending(x => x.Worker.User.WorkerLaborInformation.WorkerCode)
                        : query.OrderBy(x => x.Worker.User.WorkerLaborInformation != null).ThenBy(x => x.Worker.User.WorkerLaborInformation.WorkerCode);
                    break;
                case "1":
                    query = paginationParameter.SortOrder == paginationParameter.BaseOrder
                        ? query.OrderByDescending(x => x.Worker.User.FullName)
                        : query.OrderBy(x => x.Worker.User.FullName);
                    break;
                case "2":
                    query = paginationParameter.SortOrder == paginationParameter.BaseOrder
                        ? query.OrderByDescending(x => x.Worker.User.Dni)
                        : query.OrderBy(x => x.Worker.User.Dni);
                    break;
                case "3":
                    query = paginationParameter.SortOrder == paginationParameter.BaseOrder
                        ? query.OrderByDescending(x => x.PayrollWorkerWageItemsCount)
                        : query.OrderBy(x => x.PayrollWorkerWageItemsCount);
                    break;
            }

            var result = await query
                .Skip(paginationParameter.CurrentNumber)
                .Take(paginationParameter.RecordsPerPage)
                .Select(x => new PayrollWorker
                {
                    Id = x.Id,
                    PayrollWorkerWageItemsCount = x.PayrollWorkerWageItems.Count(),
                    Worker = new Worker
                    {
                        Id = x.Id,
                        User = new ENTITIES.Models.Generals.ApplicationUser
                        {
                            Name = x.Worker.User.Name,
                            PaternalSurname = x.Worker.User.PaternalSurname,
                            MaternalSurname = x.Worker.User.MaternalSurname,
                            Dni = x.Worker.User.Dni,
                            WorkerLaborInformation = new ENTITIES.Models.Scale.Entities.WorkerLaborInformation
                            {
                                WorkerCode = x.Worker.User.WorkerLaborInformation.WorkerCode
                            }
                        }
                    }
                }).ToListAsync();

            return (result, count);
        }
    }
}

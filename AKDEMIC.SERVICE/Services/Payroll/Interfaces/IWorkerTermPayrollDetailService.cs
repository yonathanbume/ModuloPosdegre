using AKDEMIC.ENTITIES.Models.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Payroll.Interfaces
{
    public interface IWorkerTermPayrollDetailService
    {
        Task<bool> AnyByTerm(Guid workingTermId);
        Task<WorkerTermPayrollDetail> Add(WorkerTermPayrollDetail workerTermPayrollDetails);
        Task AddRange(List<WorkerTermPayrollDetail> workerTermPayrollDetails);
    }
}

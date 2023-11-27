using AKDEMIC.ENTITIES.Models.Payroll;
using AKDEMIC.REPOSITORY.Repositories.Payroll.Interfaces;
using AKDEMIC.SERVICE.Services.Payroll.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Payroll.Implementations
{
    public class WorkerTermPayrollDetailService : IWorkerTermPayrollDetailService
    {
        private readonly IWorkerTermPayrollDetailRepository _workerTermPayrollDetailRepository;
        public WorkerTermPayrollDetailService(IWorkerTermPayrollDetailRepository workerTermPayrollDetailRepository)
        {
            _workerTermPayrollDetailRepository = workerTermPayrollDetailRepository;
        }

        public Task<WorkerTermPayrollDetail> Add(WorkerTermPayrollDetail workerTermPayrollDetail)
            => _workerTermPayrollDetailRepository.Add(workerTermPayrollDetail);

        public Task AddRange(List<WorkerTermPayrollDetail> workerTermPayrollDetails)
            => _workerTermPayrollDetailRepository.AddRange(workerTermPayrollDetails);

        public Task<bool> AnyByTerm(Guid workingTermId)
            => _workerTermPayrollDetailRepository.AnyByTerm(workingTermId);
    }
}

using AKDEMIC.ENTITIES.Models.Portal;
using AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Interfaces;
using AKDEMIC.SERVICE.Services.TransparencyPortal.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TransparencyPortal.Implementations
{
    public class FinancialExecutionService : IFinancialExecutionService
    {
        private readonly IFinancialExecutionRepository _financialexecutionRepository;
        public FinancialExecutionService(IFinancialExecutionRepository financialexecutionRepository)
        {
            _financialexecutionRepository = financialexecutionRepository;
        }

        public async Task DeleteById(Guid id)
        {
            await _financialexecutionRepository.DeleteById(id);
        }

        public async  Task<FinancialExecution> Get(Guid id)
        {
            return await _financialexecutionRepository.Get(id);
        }

        public async Task<List<FinancialExecution>> GetByType(int type)
        {
            return await _financialexecutionRepository.GetByType(type);
        }

        public async Task Insert(FinancialExecution regulation)
        {
            await _financialexecutionRepository.Insert(regulation);
        }

        public async Task Update(FinancialExecution regulation)
        {
            await _financialexecutionRepository.Update(regulation);
        }
    }
}

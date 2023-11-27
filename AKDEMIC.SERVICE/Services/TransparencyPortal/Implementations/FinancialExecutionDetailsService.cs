using AKDEMIC.ENTITIES.Models.Portal;
using AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Interfaces;
using AKDEMIC.SERVICE.Services.TransparencyPortal.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TransparencyPortal.Implementations
{
    public class FinancialExecutionDetailsService : IFinancialExecutionDetailsService
    {
        private readonly IFinancialExecutionDetailsRepository _financialexecutionRepository;
        public FinancialExecutionDetailsService(IFinancialExecutionDetailsRepository financialexecutionRepository)
        {
            _financialexecutionRepository = financialexecutionRepository;
        }

        public async Task DeleteById(Guid id)
        {
            await _financialexecutionRepository.DeleteById(id);
        }

        public async  Task<FinancialExecutionDetail> Get(Guid id)
        {
            return await _financialexecutionRepository.Get(id);
        }

        public async Task<List<FinancialExecutionDetail>> GetByFinancialExecutionId(Guid id)
        {
            return await _financialexecutionRepository.GetByFinancialExecutionId(id);
        }

        public async Task Insert(FinancialExecutionDetail regulation)
        {
            await _financialexecutionRepository.Insert(regulation);
        }

        public async Task Update(FinancialExecutionDetail regulation)
        {
            await _financialexecutionRepository.Update(regulation);
        }
    }
}

using AKDEMIC.ENTITIES.Models.Portal;
using AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Interfaces;
using AKDEMIC.SERVICE.Services.TransparencyPortal.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TransparencyPortal.Implementations
{
    public class FinancialStatementService : IFinancialStatementService
    {
        private readonly IFinancialStatementRepository _financialStatementRepository;
        public FinancialStatementService(IFinancialStatementRepository financialStatementRepository)
        {
            _financialStatementRepository = financialStatementRepository;
        }

        public async Task DeleteById(Guid id)
        {
            await _financialStatementRepository.DeleteById(id);
        }

        public async Task<bool> ExistAnyWithName(Guid id, string name)
        {
            return await _financialStatementRepository.ExistAnyWithName(id, name);
        }

        public async  Task<FinancialStatement> Get(Guid id)
        {
            return await _financialStatementRepository.Get(id);
        }

        public async Task<IEnumerable<FinancialStatement>> GetAll()
        {
            return await _financialStatementRepository.GetAll();
        }

        public async Task<IEnumerable<FinancialStatement>> GetBySlug(string slug)
        {
            return await _financialStatementRepository.GetBySlug(slug);
        }

        public async Task Insert(FinancialStatement regulation)
        {
            await _financialStatementRepository.Insert(regulation);
        }

        public async Task Update(FinancialStatement regulation)
        {
            await _financialStatementRepository.Update(regulation);
        }
    }
}

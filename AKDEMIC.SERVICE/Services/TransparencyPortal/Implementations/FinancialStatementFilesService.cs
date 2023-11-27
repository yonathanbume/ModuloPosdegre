using AKDEMIC.ENTITIES.Models.Portal;
using AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Interfaces;
using AKDEMIC.SERVICE.Services.TransparencyPortal.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TransparencyPortal.Implementations
{
    public class FinancialStatementFilesService : IFinancialStatementFilesService
    {
        private readonly IFinancialStatementFilesRepository _FinancialStatementFilesRepository;
        public FinancialStatementFilesService(IFinancialStatementFilesRepository FinancialStatementFilesRepository)
        {
            _FinancialStatementFilesRepository = FinancialStatementFilesRepository;
        }

        public async Task DeleteById(Guid id)
        {
            await _FinancialStatementFilesRepository.DeleteById(id);
        }

        public async  Task<FinancialStatementFile> Get(Guid id)
        {
            return await _FinancialStatementFilesRepository.Get(id);
        }

        public async Task<List<FinancialStatementFile>> GetByFinancialStatementId(Guid id)
        {
            return await _FinancialStatementFilesRepository.GetByFinancialStatementId(id);
        }

        public async Task Insert(FinancialStatementFile regulation)
        {
            await _FinancialStatementFilesRepository.Insert(regulation);
        }

        public async Task Update(FinancialStatementFile regulation)
        {
            await _FinancialStatementFilesRepository.Update(regulation);
        }
    }
}

using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Implementations
{
    public class CreditNoteService : ICreditNoteService
    {
        private readonly ICreditNoteRepository _creditNoteRepository;
        public CreditNoteService(ICreditNoteRepository creditNoteRepository)
        {
            _creditNoteRepository = creditNoteRepository;
        }
        public async Task AddAsync(CreditNote credit)
            => await _creditNoteRepository.Add(credit);
        public async Task<List<CreditNote>> GetCreditNotesByIdList(Guid id)
            => await _creditNoteRepository.GetCreditNotesByIdList(id);
        public async Task<CreditNote> Get(Guid id)
            => await _creditNoteRepository.Get(id);
        public async Task<int> GetCreditNoteNextNumber()
            => await _creditNoteRepository.GetCreditNoteNextNumber();

        public async Task Update(CreditNote creditNote)
        {
            await _creditNoteRepository.Update(creditNote);
        }
    }
}

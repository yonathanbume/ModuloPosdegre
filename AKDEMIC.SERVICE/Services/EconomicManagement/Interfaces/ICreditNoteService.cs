using AKDEMIC.ENTITIES.Models.EconomicManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces
{
    public interface ICreditNoteService
    {
        Task AddAsync(CreditNote credit);
        Task<List<CreditNote>> GetCreditNotesByIdList(Guid id);
        Task<CreditNote> Get(Guid id);
        Task<int> GetCreditNoteNextNumber();
        Task Update(CreditNote creditNote);
    }
}

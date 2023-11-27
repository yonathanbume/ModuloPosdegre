using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Implementations
{
    public class PettyCashBookService : IPettyCashBookService
    {
        private readonly IPettyCashBookRepository _pettyCashBookRepository;
        public PettyCashBookService(IPettyCashBookRepository pettyCashBookRepository)
        {
            _pettyCashBookRepository = pettyCashBookRepository;
        }

        public async Task<PettyCashBook> Get(Guid id)
        {
            return await _pettyCashBookRepository.Get(id);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetPettyCashBookDataTable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user = null)
            => await _pettyCashBookRepository.GetPettyCashBookDataTable(sentParameters, user);

        public async Task Insert(PettyCashBook book)
        {
            await _pettyCashBookRepository.Insert(book);
        }
    }
}

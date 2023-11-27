using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Implementations
{
    public class DebtService : IDebtService
    {
        private readonly IDebtRepository _debtRepository;

        public DebtService(IDebtRepository debtRepository)
        {
            _debtRepository = debtRepository;
        }

        public async Task InsertDebt(Debt debt) =>
            await _debtRepository.Insert(debt);

        public async Task UpdateDebt(Debt debt) =>
            await _debtRepository.Update(debt);

        public async Task DeleteDebt(Debt debt) =>
            await _debtRepository.Delete(debt);

        public async Task<Debt> GetDebtById(Guid id) =>
            await _debtRepository.Get(id);

        public async Task<IEnumerable<Debt>> GetAllDebts() =>
            await _debtRepository.GetAll();

        public async Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string search = null)
            => await _debtRepository.GetDataDatatable(sentParameters, search);
    }
}

using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Implementations
{
    public class ExpenseOutputService : IExpenseOutputService
    {
        private readonly IExpenseOutputRepository _expenseOutputRepository;

        public ExpenseOutputService(IExpenseOutputRepository expenseOutputRepository)
        {
            _expenseOutputRepository = expenseOutputRepository;
        }

        public async Task InsertExpenseOutput(ExpenseOutput expenseOutput) =>
            await _expenseOutputRepository.Insert(expenseOutput);

        public async Task UpdateExpenseOutput(ExpenseOutput expenseOutput) =>
            await _expenseOutputRepository.Update(expenseOutput);

        public async Task DeleteExpenseOutput(ExpenseOutput expenseOutput) =>
            await _expenseOutputRepository.Delete(expenseOutput);
        public async Task<List<ExpenseOutput>> GetExpenseOutputReportList(Guid id)
            => await _expenseOutputRepository.GetExpenseOutputReportList(id);
        public async Task<ExpenseOutput> GetExpenseOutputById(Guid id) =>
            await _expenseOutputRepository.Get(id);

        public async Task<IEnumerable<ExpenseOutput>> GetAllExpenseOutputs() =>
            await _expenseOutputRepository.GetAll();

        public async Task<DataTablesStructs.ReturnedData<object>> GetExpenseOutPutsDatatables(DataTablesStructs.SentParameters sentParameters, string userId, string search)
            => await _expenseOutputRepository.GetExpenseOutPutsDatatables(sentParameters, userId, search);
        public async Task<DataTablesStructs.ReturnedData<object>> GetExpenseOutPutsTesoDatatables(DataTablesStructs.SentParameters sentParameters, string search)
    => await _expenseOutputRepository.GetExpenseOutPutsTesoDatatables(sentParameters, search);
    }
}

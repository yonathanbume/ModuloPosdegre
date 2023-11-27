using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Implementations
{
    public class MonthlyBalanceService : IMonthlyBalanceService
    {
        private readonly IMonthlyBalanceRepository _monthlyBalanceRepository;
        public MonthlyBalanceService(IMonthlyBalanceRepository monthlyBalanceRepository)
        {
            _monthlyBalanceRepository = monthlyBalanceRepository;
        }

        public async Task<MonthlyBalance> GetLastClosedBalance()
            => await _monthlyBalanceRepository.GetLastClosedBalance();
    }
}

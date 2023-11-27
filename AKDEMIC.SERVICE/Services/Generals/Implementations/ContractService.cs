using AKDEMIC.ENTITIES.Models;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Generals.Implementations
{
    public sealed class ContractService : IContractService
    {
        private readonly IContractRepository _contractRepository;

        public ContractService(IContractRepository contractRepository)
        {
            _contractRepository = contractRepository;
        }

        Task IContractService.DeleteAsync(Contract contract)
            => _contractRepository.Delete(contract);

        Task<object> IContractService.GetAllAsModelA()
            => _contractRepository.GetAllAsModelA();

        Task<Contract> IContractService.GetAsync(Guid id)
            => _contractRepository.Get(id);

        Task IContractService.InsertAsync(Contract contract)
            => _contractRepository.Insert(contract);

        Task IContractService.UpdateAsync(Contract contract)
            => _contractRepository.Update(contract);
    }
}
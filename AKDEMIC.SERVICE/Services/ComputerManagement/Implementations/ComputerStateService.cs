using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.ComputersManagement;
using AKDEMIC.REPOSITORY.Repositories.ComputerManagement.Interfaces;
using AKDEMIC.SERVICE.Services.ComputerManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.ComputerManagement.Implementations
{
    public class ComputerStateService : IComputerStateService
    {
        private readonly IComputerStateRepository _computerStateRepository;
        public ComputerStateService(IComputerStateRepository computerStateRepository)
        {
            _computerStateRepository = computerStateRepository;
        }

        public async Task Delete(ComputerState computerState)
            => await _computerStateRepository.Delete(computerState);

        public async Task<ComputerState> Get(Guid id)
            => await _computerStateRepository.Get(id);

        public async Task<IEnumerable<ComputerState>> GetAll()
            => await _computerStateRepository.GetAll();
        public async Task<DataTablesStructs.ReturnedData<object>> GetComputerStateDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
                   => await _computerStateRepository.GetComputerStateDatatable(sentParameters, searchValue);

        public async Task Insert(ComputerState computerState)
            => await _computerStateRepository.Insert(computerState);

        public async Task Update(ComputerState computerState)
            => await _computerStateRepository.Update(computerState);
    }
}

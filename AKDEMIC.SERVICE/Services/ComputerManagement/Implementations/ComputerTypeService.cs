using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.ComputersManagement;
using AKDEMIC.REPOSITORY.Repositories.ComputerManagement.Interfaces;
using AKDEMIC.SERVICE.Services.ComputerManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.ComputerManagement.Implementations
{
    public class ComputerTypeService : IComputerTypeService
    {
        private readonly IComputerTypeRepository _computerTypeRepository;
        public ComputerTypeService(IComputerTypeRepository computerTypeRepository)
        {
            _computerTypeRepository = computerTypeRepository;
        }

        public async Task Delete(ComputerType computerType)
            => await _computerTypeRepository.Delete(computerType);

        public async Task<ComputerType> Get(Guid id)
            => await _computerTypeRepository.Get(id);

        public async Task<IEnumerable<ComputerType>> GetAll()
            => await _computerTypeRepository.GetAll();
        public async Task<DataTablesStructs.ReturnedData<object>> GetComputerTypeDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
                   => await _computerTypeRepository.GetComputerTypeDatatable(sentParameters, searchValue);

        public async Task Insert(ComputerType computerType)
            => await _computerTypeRepository.Insert(computerType);

        public async Task Update(ComputerType computerType)
            => await _computerTypeRepository.Update(computerType);
    }
}

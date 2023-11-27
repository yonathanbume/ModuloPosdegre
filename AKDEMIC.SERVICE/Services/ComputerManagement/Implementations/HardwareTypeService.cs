using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.ComputersManagement;
using AKDEMIC.REPOSITORY.Repositories.ComputerManagement.Interfaces;
using AKDEMIC.SERVICE.Services.ComputerManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.ComputerManagement.Implementations
{
    public class HardwareTypeService :IHardwareTypeService
    {
        private readonly IHardwareTypeRepository _hardwareTypeRepository;
        public HardwareTypeService(IHardwareTypeRepository hardwareTypeRepository)
        {
            _hardwareTypeRepository = hardwareTypeRepository;
        }

        public async Task Delete(HardwareType hardwareType)
            => await _hardwareTypeRepository.Delete(hardwareType);

        public async Task DeleteById(Guid id)
            => await _hardwareTypeRepository.DeleteById(id);

        public async Task<HardwareType> Get(Guid id)
            => await _hardwareTypeRepository.Get(id);

        public async Task<IEnumerable<HardwareType>> GetAll()
            => await _hardwareTypeRepository.GetAll();

        public async Task<DataTablesStructs.ReturnedData<object>> GetHardwareTypeDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
            => await _hardwareTypeRepository.GetHardwareTypeDatatable(sentParameters,searchValue);

        public async Task Insert(HardwareType hardwareType)
            => await _hardwareTypeRepository.Insert(hardwareType);

        public async Task Update(HardwareType hardwareType)
            => await _hardwareTypeRepository.Update(hardwareType);
    }
}

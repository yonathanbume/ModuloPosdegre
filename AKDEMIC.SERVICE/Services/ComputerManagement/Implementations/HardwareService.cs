using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.ComputersManagement;
using AKDEMIC.REPOSITORY.Repositories.ComputerManagement.Interfaces;
using AKDEMIC.SERVICE.Services.ComputerManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.ComputerManagement.Implementations
{
    public class HardwareService: IHardwareService
    {
        private readonly IHardwareRepository _hardwareRepository;
        public HardwareService(IHardwareRepository hardwareRepository)
        {
            _hardwareRepository = hardwareRepository;
        }

        public async Task Delete(Hardware hardware)
            => await _hardwareRepository.Delete(hardware);

        public async Task DeleteById(Guid id)
            => await _hardwareRepository.DeleteById(id);

        public async Task<Hardware> Get(Guid id)
            => await _hardwareRepository.Get(id);

        public async Task<IEnumerable<Hardware>> GetAll()
            => await _hardwareRepository.GetAll();

        public async Task<IEnumerable<Hardware>> GetHardwaresByComputer(Guid computerId)
            => await _hardwareRepository.GetHardwaresByComputer(computerId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetHardwaresByComputerDatatable(DataTablesStructs.SentParameters sentParameters, Guid computerId, string searchValue = null)
            => await _hardwareRepository.GetHardwaresByComputerDatatable(sentParameters,computerId,searchValue);

        public async Task Insert(Hardware hardware)
            => await _hardwareRepository.Insert(hardware);

        public async Task Update(Hardware hardware)
            => await _hardwareRepository.Update(hardware);
    }
}

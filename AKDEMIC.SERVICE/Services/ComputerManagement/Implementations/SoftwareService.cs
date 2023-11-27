using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.ComputersManagement;
using AKDEMIC.REPOSITORY.Repositories.ComputerManagement.Interfaces;
using AKDEMIC.SERVICE.Services.ComputerManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.ComputerManagement.Implementations
{
    public class SoftwareService: ISoftwareService
    {
        private readonly ISoftwareRepository _softwareRepository;
        public SoftwareService(ISoftwareRepository softwareRepository)
        {
            _softwareRepository = softwareRepository;
        }

        public async Task Delete(Software software)
            => await _softwareRepository.Delete(software);

        public async Task DeleteById(Guid id)
            => await _softwareRepository.DeleteById(id);

        public async Task<Software> Get(Guid id)
            => await _softwareRepository.Get(id);

        public async Task<IEnumerable<Software>> GetAll()
            => await _softwareRepository.GetAll();

        public async Task<IEnumerable<Software>> GetSoftwaresByComputer(Guid computerId)
            => await _softwareRepository.GetSoftwaresByComputer(computerId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetSoftwaresByComputerDatatable(DataTablesStructs.SentParameters sentParameters, Guid computerId, string searchValue = null)
            => await _softwareRepository.GetSoftwaresByComputerDatatable(sentParameters,computerId,searchValue);

        public async Task Insert(Software software)
            => await _softwareRepository.Insert(software);

        public async Task Update(Software software)
            => await _softwareRepository.Update(software);
    }
}

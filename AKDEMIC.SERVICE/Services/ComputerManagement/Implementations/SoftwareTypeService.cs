using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.ComputersManagement;
using AKDEMIC.REPOSITORY.Repositories.ComputerManagement.Interfaces;
using AKDEMIC.SERVICE.Services.ComputerManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.ComputerManagement.Implementations
{
    public class SoftwareTypeService : ISoftwareTypeService
    {
        private readonly ISoftwareTypeRepository _softwareTypeRepository;
        public SoftwareTypeService(ISoftwareTypeRepository softwareTypeRepository)
        {
            _softwareTypeRepository = softwareTypeRepository;
        }
        public async Task Delete(SoftwareType softwareType)
            => await _softwareTypeRepository.Delete(softwareType);

        public async Task DeleteById(Guid id)
            => await _softwareTypeRepository.DeleteById(id);

        public async Task<SoftwareType> Get(Guid id)
            => await _softwareTypeRepository.Get(id);

        public async Task<IEnumerable<SoftwareType>> GetAll()
            => await _softwareTypeRepository.GetAll();

        public async Task<DataTablesStructs.ReturnedData<object>> GetSoftwareTypeDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
            => await _softwareTypeRepository.GetSoftwareTypeDatatable(sentParameters, searchValue);

        public async Task Insert(SoftwareType softwareType)
            => await _softwareTypeRepository.Insert(softwareType);

        public async Task Update(SoftwareType softwareType)
            => await _softwareTypeRepository.Update(softwareType);
    }
}

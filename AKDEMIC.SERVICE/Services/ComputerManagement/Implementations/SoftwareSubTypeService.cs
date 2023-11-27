using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.ComputersManagement;
using AKDEMIC.REPOSITORY.Repositories.ComputerManagement.Interfaces;
using AKDEMIC.SERVICE.Services.ComputerManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.ComputerManagement.Implementations
{
    public class SoftwareSubTypeService : ISoftwareSubTypeService
    {
        private readonly ISoftwareSubTypeRepository _softwareSubTypeRepository;
        public SoftwareSubTypeService(ISoftwareSubTypeRepository softwareSubTypeRepository)
        {
            _softwareSubTypeRepository = softwareSubTypeRepository;
        }
        public async Task Delete(SoftwareSubType softwareSubType)
            => await _softwareSubTypeRepository.Delete(softwareSubType);

        public async Task DeleteById(Guid id)
            => await _softwareSubTypeRepository.DeleteById(id);

        public async Task<SoftwareSubType> Get(Guid id)
            => await _softwareSubTypeRepository.Get(id);

        public async Task<IEnumerable<SoftwareSubType>> GetAll()
            => await _softwareSubTypeRepository.GetAll();

        public async Task<DataTablesStructs.ReturnedData<object>> GetSoftwareSubTypeDatatable(DataTablesStructs.SentParameters sentParameters, Guid TypeId, string searchValue = null)
            => await _softwareSubTypeRepository.GetSoftwareSubTypeDatatable(sentParameters, TypeId, searchValue);

        public async Task Insert(SoftwareSubType softwareSubType)
            => await _softwareSubTypeRepository.Insert(softwareSubType);

        public async Task Update(SoftwareSubType softwareSubType)
            => await _softwareSubTypeRepository.Update(softwareSubType);
    }
}

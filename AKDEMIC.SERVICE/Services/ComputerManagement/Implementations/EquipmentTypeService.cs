using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.ComputersManagement;
using AKDEMIC.REPOSITORY.Repositories.ComputerManagement.Interfaces;
using AKDEMIC.SERVICE.Services.ComputerManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.ComputerManagement.Implementations
{
    public class EquipmentTypeService: IEquipmentTypeService
    {
        private readonly IEquipmentTypeRepository _equipmentTypeRepository;
        public EquipmentTypeService(IEquipmentTypeRepository equipmentTypeRepository)
        {
            _equipmentTypeRepository = equipmentTypeRepository;
        }

        public async Task Delete(EquipmentType equipmentType)
            => await _equipmentTypeRepository.Delete(equipmentType);

        public async Task<EquipmentType> Get(Guid id)
            => await _equipmentTypeRepository.Get(id);

        public async Task<IEnumerable<EquipmentType>> GetAll()
            => await _equipmentTypeRepository.GetAll();

        public async Task Insert(EquipmentType equipmentType)
            => await _equipmentTypeRepository.Insert(equipmentType);

        public async Task Update(EquipmentType equipmentType)
            => await _equipmentTypeRepository.Update(equipmentType);

        public async Task<DataTablesStructs.ReturnedData<object>> GetEquipmentTypeDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
            => await _equipmentTypeRepository.GetEquipmentTypeDatatable(sentParameters,searchValue);
    }
}

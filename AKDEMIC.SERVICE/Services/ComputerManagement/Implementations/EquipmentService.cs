using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.ComputersManagement;
using AKDEMIC.REPOSITORY.Repositories.ComputerManagement.Interfaces;
using AKDEMIC.SERVICE.Services.ComputerManagement.Interfaces;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.ComputerManagement.Implementations
{
    public class EquipmentService: IEquipmentService
    {
        private readonly IEquipmentRepository _equipmentRepository;
        public EquipmentService(IEquipmentRepository equipmentRepository )
        {
            _equipmentRepository = equipmentRepository;
        }

        public async Task Delete(Equipment equipment)
            => await _equipmentRepository.Delete(equipment);

        public async Task<Equipment> Get(Guid id)
            => await _equipmentRepository.Get(id);

        public async Task Insert(Equipment equipment)
            => await _equipmentRepository.Insert(equipment);

        public async Task Update(Equipment equipment)
            => await _equipmentRepository.Update(equipment);

        public async Task<DataTablesStructs.ReturnedData<object>> GetEquipmentDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null,Guid? dependencyId = null)
            => await _equipmentRepository.GetEquipmentDatatable(sentParameters,searchValue,dependencyId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetEquipmentReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid? dependencyId = null, Guid? equipmentTypeId = null, Guid? state = null, string searchValue = null)
            => await _equipmentRepository.GetEquipmentReportDatatable(sentParameters, dependencyId,equipmentTypeId,state,searchValue);

        public async Task<object> GetEquipmentReportChart(Guid? dependencyId = null, Guid? equipmentTypeId = null, Guid? state = null)
            => await _equipmentRepository.GetEquipmentReportChart(dependencyId, equipmentTypeId, state);
    }
}

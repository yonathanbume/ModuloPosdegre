using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.ComputersManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.ComputerManagement.Interfaces
{
    public interface IEquipmentTypeService
    {
        //Generals
        Task<EquipmentType> Get(Guid id);
        Task<IEnumerable<EquipmentType>> GetAll();
        Task Insert(EquipmentType equipmentType);
        Task Delete(EquipmentType equipmentType);
        Task Update(EquipmentType equipmentType);

        Task<DataTablesStructs.ReturnedData<object>> GetEquipmentTypeDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
    }
}

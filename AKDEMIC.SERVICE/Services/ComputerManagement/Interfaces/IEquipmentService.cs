using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.ComputersManagement;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.ComputerManagement.Interfaces
{
    public interface IEquipmentService
    {
        //Generales
        Task<Equipment> Get(Guid id);
        Task Insert(Equipment equipment);
        Task Update(Equipment equipment);
        Task Delete(Equipment equipment);

        Task<DataTablesStructs.ReturnedData<object>> GetEquipmentDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null,Guid? dependencyId = null);
        Task<DataTablesStructs.ReturnedData<object>> GetEquipmentReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid? dependencyId = null, Guid? equipmentTypeId = null, Guid? state = null, string searchValue = null);
        Task<object> GetEquipmentReportChart(Guid? dependencyId = null, Guid? equipmentTypeId = null, Guid? state = null);
    }
}

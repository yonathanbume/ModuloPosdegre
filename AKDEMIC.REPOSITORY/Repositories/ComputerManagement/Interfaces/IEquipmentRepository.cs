using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.ComputersManagement;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.ComputerManagement.Interfaces
{
    public interface IEquipmentRepository:IRepository<Equipment>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetEquipmentDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null,Guid? dependencyId = null);
        Task<DataTablesStructs.ReturnedData<object>> GetEquipmentReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid? dependencyId = null, Guid? equipmentTypeId = null, Guid? state = null, string searchValue = null);
        Task<object> GetEquipmentReportChart(Guid? dependencyId = null, Guid? equipmentTypeId = null, Guid? state = null);
    }
}

using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Admission.Interfaces
{
    public interface IEntityLoadFormatService
    {
        Task<DataTablesStructs.ReturnedData<object>> EntityLoadFormatDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task Insert(EntityLoadFormat entityLoadFormat);
        Task Update(EntityLoadFormat entityLoadFormat);
        Task Delete(EntityLoadFormat entityLoadFormat);
        Task DeleteById(Guid entityLoadFormatId);
        Task Activate(Guid entityLoadFormatId);

        Task<EntityLoadFormat> Get(Guid entityLoadFormatId);
        Task<EntityLoadFormat> GetActive();
        Task<object> EntityLoadSelect2(bool? onlyActive = false);
    }
}

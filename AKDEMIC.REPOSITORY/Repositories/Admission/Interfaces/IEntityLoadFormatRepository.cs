using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces
{
    public interface IEntityLoadFormatRepository: IRepository<EntityLoadFormat>
    {
        Task<DataTablesStructs.ReturnedData<object>> EntityLoadFormatDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<object> EntityLoadSelect2(bool? onlyActive = false);
        Task Activate(Guid entityLoadFormatId);
        Task<EntityLoadFormat> GetActive();
    }
}

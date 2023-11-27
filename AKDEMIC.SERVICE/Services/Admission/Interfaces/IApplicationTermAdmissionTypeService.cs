using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Admission.Interfaces
{
    public interface IApplicationTermAdmissionTypeService
    {
        Task<object> GetByApplicationTermId(DataTablesStructs.SentParameters sentParameters, Guid id,int type, bool isAllChecked, string searchValue);
        Task<object> GetByApplicationTermId(Guid id);
        Task SaveApplicationTermAdmissionTypes(Guid id, bool isCheckAll, List<Guid> lstToAdd, List<Guid> lstToAvoid);
        Task<object> GetByApplicationTermIdSelect2(Guid id);
        Task AddRemoveApplicationTermAdmissionType(Guid applicationTermId, Guid admissionTypeId);
        Task<bool> AnyPostulantByApplicationTermIdAndAdmissionTypeId(Guid applicationTermId, Guid admissionTypeId);
        Task<ApplicationTermAdmissionType> Get(Guid Id);
        Task<IEnumerable<ApplicationTermAdmissionType>> GetAllByApplicationTermId(Guid applicationTermId);
        Task<DataTablesStructs.ReturnedData<object>> GeApplicationTermAdmissionTypesDatatable(DataTablesStructs.SentParameters sentParameters, Guid applicationTermId, string searchValue);
    }
}

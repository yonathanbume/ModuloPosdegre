using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;

namespace AKDEMIC.SERVICE.Services.Enrollment.Interfaces
{
    public interface ICampusService
    {
        Task<IEnumerable<Campus>> GetAll();
        Task<object> GetAllAsSelect2ClientSide();
        Task<Campus> Get(Guid id);
        Task DeleteById(Guid id);
        Task Insert(Campus campus);
        Task Update(Campus campus);
        Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue);
        Task<Select2Structs.ResponseParameters> GetAllSelect2(Select2Structs.RequestParameters requestParameters);
        Task ClearPrincipal();
        Task<object> GetCampus(Guid id);
        Task<object> GetCampusJson();
        Task<object> GetCampusCareerJson(Guid cid);
        Task<Campus> GetFirstCampus();
        Task<Campus> GetCampusPrincipal();
        Task<bool> AnyClassroom(Guid id);
    }
}
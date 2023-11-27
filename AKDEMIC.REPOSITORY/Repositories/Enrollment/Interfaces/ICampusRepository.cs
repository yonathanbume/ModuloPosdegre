using System;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces
{
    public interface ICampusRepository : IRepository<Campus>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue);
        Task<object> GetAllAsSelect2ClientSide();
        Task<object> GetCampus(Guid id);
        Task ClearPrincipal();
        Task<Select2Structs.ResponseParameters> GetAllSelect2(Select2Structs.RequestParameters requestParameters);
        Task<object> GetCampusJson();
        Task<object> GetCampusCareerJson(Guid cid);
        Task<Campus> GetFirstCampus();
        Task<Campus> GetCampusPrincipal();
        Task<bool> AnyClassroom(Guid id);
    }
}
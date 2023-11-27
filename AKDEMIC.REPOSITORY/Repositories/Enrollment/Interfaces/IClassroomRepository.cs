using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces
{
    public interface IClassroomRepository : IRepository<Classroom>
    {
        Task<List<Classroom>> GetAllWithData();
        Task<List<Classroom>> GetAllByBuildingId(Guid buildingId);
        Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue);
        Task<IEnumerable<Select2Structs.Result>> GetClassroomsSelect2ClientSide(Guid? buildingId = null, Guid? campusId = null);
        Task<object> GetClassroomsJson(string q);
        Task<object> GetClassroomsWithCampusJson(string q);
        Task<Select2Structs.ResponseParameters> GetClassroomByBuilding(Select2Structs.RequestParameters requestParameters, Guid? buildingId = null, Expression<Func<Classroom, Select2Structs.Result>> selectPredicate = null, Func<Classroom, string[]> searchValuePredicate = null, string search = null);
        Task<Select2Structs.ResponseParameters> GetClassroomSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null, Guid? buildingId = null, Guid? campusId = null);
        Task<object> GetClassroomsWithDataSelect2ClientSide(Guid? buildingId = null, Guid? campusId = null, string search = null);
        Task<Classroom> GetWithData(Guid id);
        Task<bool> AnyByDescription(string description, Guid buildingId, Guid? ignoredId);
        Task<bool> AnyClassSchedule(Guid id, Guid termId);
        Task<bool> AnyClassSchedule(Guid id);
    }
}

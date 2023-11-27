using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Interfaces
{
    public interface IClassroomService
    {
        Task<IEnumerable<Classroom>> GetAll();
        Task<List<Classroom>> GetAllByBuildingId(Guid buildingId);
        Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue);
        Task Delete(Classroom classroom);
        Task DeleteById(Guid id);
        Task Insert(Classroom classroom);
        Task InsertRange(List<Classroom> classrooms);
        Task Update(Classroom classroom);
        Task<Classroom> Get(Guid id);
        Task<IEnumerable<Select2Structs.Result>> GetClassroomsSelect2ClientSide(Guid? buildingId = null, Guid? campusId = null);
        Task<object> GetClassroomsJson(string q);
        Task<object> GetClassroomsWithCampusJson(string q);
        Task<Select2Structs.ResponseParameters> GetClassroomByBuilding(Select2Structs.RequestParameters requestParameters, Guid? buildingId = null, Expression<Func<Classroom, Select2Structs.Result>> selectPredicate = null, Func<Classroom, string[]> searchValuePredicate = null, string search = null);
        Task<Select2Structs.ResponseParameters> GetClassroomSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null,Guid? buildingId = null, Guid? campusId = null);
        Task<object> GetClassroomsWithDataSelect2ClientSide(Guid? buildingId = null, Guid? campusId = null, string search = null);
        Task<Classroom> GetWithData(Guid id);
        Task<bool> AnyByDescription(string description, Guid buildingId, Guid? ignoredId);
        Task<bool> AnyClassSchedule(Guid id, Guid termId);
        Task<bool> AnyClassSchedule(Guid id);
    }
}

using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Implementations
{
    public class ClassroomService : IClassroomService
    {
        private readonly IClassroomRepository _classroomRepository;

        public ClassroomService(IClassroomRepository classroomRepository)
        {
            _classroomRepository = classroomRepository;
        }

        public async Task Delete(Classroom classroom) => await _classroomRepository.Delete(classroom);

        public async Task DeleteById(Guid id) => await _classroomRepository.DeleteById(id);

        public async Task<Classroom> Get(Guid id) => await _classroomRepository.Get(id);

        public async Task<IEnumerable<Classroom>> GetAll()
            => await _classroomRepository.GetAllWithData();

        public async Task<List<Classroom>> GetAllByBuildingId(Guid buildingId)
            => await _classroomRepository.GetAllByBuildingId(buildingId);

        public async Task<IEnumerable<Select2Structs.Result>> GetClassroomsSelect2ClientSide(Guid? buildingId = null, Guid? campusId = null)
            => await _classroomRepository.GetClassroomsSelect2ClientSide(buildingId, campusId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue) 
            => await _classroomRepository.GetDataDatatable(sentParameters, searchValue);

        public async Task Insert(Classroom classroom) => await _classroomRepository.Insert(classroom);

        public async Task Update(Classroom classroom) => await _classroomRepository.Update(classroom);

        public async Task<object> GetClassroomsJson(string q)
            => await _classroomRepository.GetClassroomsJson(q);
        public async Task<object> GetClassroomsWithCampusJson(string q)
            => await _classroomRepository.GetClassroomsWithCampusJson(q);
        public async Task<Select2Structs.ResponseParameters> GetClassroomByBuilding(Select2Structs.RequestParameters requestParameters, Guid? buildingId = null, Expression<Func<Classroom, Select2Structs.Result>> selectPredicate = null, Func<Classroom, string[]> searchValuePredicate = null, string search = null)
            => await _classroomRepository.GetClassroomByBuilding(requestParameters, buildingId, selectPredicate, searchValuePredicate, search);

        public async Task InsertRange(List<Classroom> classrooms) => await _classroomRepository.InsertRange(classrooms);

        public async Task<Select2Structs.ResponseParameters> GetClassroomSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null, Guid? buildingId = null, Guid? campusId = null)
            => await _classroomRepository.GetClassroomSelect2(requestParameters, searchValue, buildingId, campusId);

        public async Task<object> GetClassroomsWithDataSelect2ClientSide(Guid? buildingId = null, Guid? campusId = null, string search = null)
            => await _classroomRepository.GetClassroomsWithDataSelect2ClientSide(buildingId, campusId, search);
        public async Task<Classroom> GetWithData(Guid id)
            => await _classroomRepository.GetWithData(id);

        public async Task<bool> AnyByDescription(string description, Guid buildingId, Guid? ignoredId)
            => await _classroomRepository.AnyByDescription(description, buildingId, ignoredId);

        public async Task<bool> AnyClassSchedule(Guid id, Guid termId)
            => await _classroomRepository.AnyClassSchedule(id, termId);

        public async Task<bool> AnyClassSchedule(Guid id)
            => await _classroomRepository.AnyClassSchedule(id);
    }
}

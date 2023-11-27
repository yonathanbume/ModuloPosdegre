using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Implementations
{
    public class ClassroomTypeService : IClassroomTypeService
    {
        private readonly IClassroomTypeRepository _classroomTypeRepository;

        public ClassroomTypeService(IClassroomTypeRepository classroomTypeRepository)
        {
            _classroomTypeRepository = classroomTypeRepository;
        }

        public async Task Delete(ClassroomType classroomType) => await _classroomTypeRepository.Delete(classroomType);

        public async Task DeleteById(Guid id) => await _classroomTypeRepository.DeleteById(id);

        public async Task<ClassroomType> Get(Guid id) => await _classroomTypeRepository.Get(id);

        public async Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue)
             => await _classroomTypeRepository.GetDataDatatable(sentParameters, searchValue);

        public async Task Insert(ClassroomType classroomType) => await _classroomTypeRepository.Insert(classroomType);

        public async Task Update(ClassroomType classroomType) => await _classroomTypeRepository.Update(classroomType);

        public async Task<object> GetClassroomTypes()
            => await _classroomTypeRepository.GetClassroomTypes();

        public async Task<IEnumerable<ClassroomType>> GetAll()
        {
            return await _classroomTypeRepository.GetAll();
        }

        public async Task InsertRange(List<ClassroomType> classroomTypes)
            => await _classroomTypeRepository.InsertRange(classroomTypes);
    }
}

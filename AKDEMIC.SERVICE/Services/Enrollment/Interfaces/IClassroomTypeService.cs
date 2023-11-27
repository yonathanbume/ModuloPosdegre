using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Interfaces
{
    public interface IClassroomTypeService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue);
        Task Delete(ClassroomType classroomType);
        Task DeleteById(Guid id);
        Task Insert(ClassroomType classroomType);
        Task InsertRange(List<ClassroomType> classroomTypes);
        Task Update(ClassroomType classroomType);
        Task<ClassroomType> Get(Guid id);
        Task<object> GetClassroomTypes();
        Task<IEnumerable<ClassroomType>> GetAll();
    }
}

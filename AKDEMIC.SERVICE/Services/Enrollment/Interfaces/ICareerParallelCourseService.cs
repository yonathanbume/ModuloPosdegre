using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Interfaces
{
    public interface ICareerParallelCourseService
    {
        Task Insert(CareerParallelCourse careerParallelCourse);
        Task Update(CareerParallelCourse careerParallelCourse);
        Task DeleteById(Guid id);
        Task<CareerParallelCourse> Get(Guid id);
        Task<bool> CheckParallelCourseExist(CareerParallelCourse careerParallelCourse);
        Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, Guid? facultyId = null, Guid? careerId = null, string searchValue = null);
        Task<CareerParallelCourse> GetByCareeerAndAcademicYear(Guid careerId, byte academicYear);
    }
}

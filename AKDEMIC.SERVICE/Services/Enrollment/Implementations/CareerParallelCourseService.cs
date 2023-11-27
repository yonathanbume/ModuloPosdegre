using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Implementations
{
    public class CareerParallelCourseService : ICareerParallelCourseService
    {
        private readonly ICareerParallelCourseRepository _careerParallelCourseRepository;

        public CareerParallelCourseService(ICareerParallelCourseRepository careerParallelCourseRepository)
        {
            _careerParallelCourseRepository = careerParallelCourseRepository;
        }

        public async Task<bool> CheckParallelCourseExist(CareerParallelCourse careerParallelCourse) => await _careerParallelCourseRepository.CheckParallelCourseExist(careerParallelCourse);

        public async Task DeleteById(Guid id) => await _careerParallelCourseRepository.DeleteById(id);

        public async Task<CareerParallelCourse> Get(Guid id) => await _careerParallelCourseRepository.Get(id);

        public async Task<CareerParallelCourse> GetByCareeerAndAcademicYear(Guid careerId, byte academicYear)
        {
            return await _careerParallelCourseRepository.GetByCareeerAndAcademicYear(careerId, academicYear);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, Guid? facultyId = null, Guid? careerId = null, string searchValue = null)
            => await _careerParallelCourseRepository.GetDataDatatable(sentParameters, facultyId, careerId, searchValue);

        public async Task Insert(CareerParallelCourse careerParallelCourse) => await _careerParallelCourseRepository.Insert(careerParallelCourse);

        public async Task Update(CareerParallelCourse careerParallelCourse) => await _careerParallelCourseRepository.Update(careerParallelCourse);
    }
}

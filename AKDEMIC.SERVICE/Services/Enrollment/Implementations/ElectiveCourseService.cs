using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Implementations
{
    public class ElectiveCourseService : IElectiveCourseService
    {
        private readonly IElectiveCourseRepository _electiveCourseRepository;

        public ElectiveCourseService(IElectiveCourseRepository electiveCourseRepository)
        {
            _electiveCourseRepository = electiveCourseRepository;
        }

        public async Task InsertElectiveCourse(ElectiveCourse electiveCourse) =>
            await _electiveCourseRepository.Insert(electiveCourse);

        public async Task UpdateElectiveCourse(ElectiveCourse electiveCourse) =>
            await _electiveCourseRepository.Update(electiveCourse);

        public async Task DeleteElectiveCourse(ElectiveCourse electiveCourse) =>
            await _electiveCourseRepository.Delete(electiveCourse);

        public async Task<ElectiveCourse> GetElectiveCourseById(Guid id) =>
            await _electiveCourseRepository.Get(id);

        public async Task<IEnumerable<ElectiveCourse>> GetAllElectiveCourses() =>
            await _electiveCourseRepository.GetAll();

        public async Task<IEnumerable<ElectiveCourse>> GetAllElectiveCoursesWithData()
            => await _electiveCourseRepository.GetAllElectiveCoursesWithData();

        public async Task<List<ElectiveCourse>> GetWithDataByCareerIdAndAcadmic(Guid careerId, int academicYear, int academicYearDispersion)
            => await _electiveCourseRepository.GetWithDataByCareerIdAndAcadmic(careerId, academicYear, academicYearDispersion);
    }
}

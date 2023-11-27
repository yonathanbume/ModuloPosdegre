using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Implementations
{
    public sealed class AcademicYearCoursePreRequisiteService : IAcademicYearCoursePreRequisiteService
    {
        private readonly IAcademicYearCoursePreRequisiteRepository _academicYearCoursePreRequisiteRepository;

        public AcademicYearCoursePreRequisiteService(IAcademicYearCoursePreRequisiteRepository academicYearCoursePreRequisiteRepository)
        {
            _academicYearCoursePreRequisiteRepository = academicYearCoursePreRequisiteRepository;
        }

        Task<bool> IAcademicYearCoursePreRequisiteService.AnyByAcademicYearCourseAndCourseId(Guid academicYearCourseId, Guid courseId)
            => _academicYearCoursePreRequisiteRepository.AnyByAcademicYearCourseAndCourseId(academicYearCourseId, courseId);

        Task IAcademicYearCoursePreRequisiteService.DeleteAsync(AcademicYearCoursePreRequisite academicYearCoursePreRequisite)
            => _academicYearCoursePreRequisiteRepository.Delete(academicYearCoursePreRequisite);

        Task IAcademicYearCoursePreRequisiteService.DeleteRangeAsync(IEnumerable<AcademicYearCoursePreRequisite> academicYearCoursePreRequisites)
            => _academicYearCoursePreRequisiteRepository.DeleteRange(academicYearCoursePreRequisites);

        Task<IEnumerable<AcademicYearCoursePreRequisite>> IAcademicYearCoursePreRequisiteService.GetAllByFilter(byte? academicYear, Guid? academicYearCourseId, Guid? curriculumId)
            => _academicYearCoursePreRequisiteRepository.GetAllByFilter(academicYear, academicYearCourseId, curriculumId);

        Task<AcademicYearCoursePreRequisite> IAcademicYearCoursePreRequisiteService.GetAsync(Guid id)
            => _academicYearCoursePreRequisiteRepository.Get(id);

        Task IAcademicYearCoursePreRequisiteService.InsertAsync(AcademicYearCoursePreRequisite academicYearCoursePreRequisite)
            => _academicYearCoursePreRequisiteRepository.Insert(academicYearCoursePreRequisite);

        Task IAcademicYearCoursePreRequisiteService.InsertRangeAsync(IEnumerable<AcademicYearCoursePreRequisite> academicYearCoursePreRequisites)
            => _academicYearCoursePreRequisiteRepository.InsertRange(academicYearCoursePreRequisites);

        Task IAcademicYearCoursePreRequisiteService.UpdateAsync(AcademicYearCoursePreRequisite academicYearCoursePreRequisite)
            => _academicYearCoursePreRequisiteRepository.Update(academicYearCoursePreRequisite);

        Task IAcademicYearCoursePreRequisiteService.UpdateRangeAsync(IEnumerable<AcademicYearCoursePreRequisite> academicYearCoursePreRequisites)
            => _academicYearCoursePreRequisiteRepository.UpdateRange(academicYearCoursePreRequisites);
    }
}
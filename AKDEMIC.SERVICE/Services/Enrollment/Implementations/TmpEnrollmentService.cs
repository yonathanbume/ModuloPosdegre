using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Implementations
{
    public class TmpEnrollmentService : ITmpEnrollmentService
    {
        private readonly ITmpEnrollmentRepository _tmpEnrollmentRepository;
        public TmpEnrollmentService(ITmpEnrollmentRepository tmpEnrollmentRepository)
        {
            _tmpEnrollmentRepository = tmpEnrollmentRepository;
        }

        public async Task<TmpEnrollment> Get(Guid id)
            => await _tmpEnrollmentRepository.Get(id);

        public async Task<IEnumerable<TmpEnrollment>> GetAll()
            => await _tmpEnrollmentRepository.GetAll();

        public Task DeleteById(Guid id) => _tmpEnrollmentRepository.DeleteById(id);

        public Task<object> GetStudentDataDatatableClientSide(Guid studentId, Guid termId)
            => _tmpEnrollmentRepository.GetStudentDataDatatableClientSide(studentId, termId);

        public void RemoveRange(List<TmpEnrollment> tmpEnrollments)
            => _tmpEnrollmentRepository.RemoveRange(tmpEnrollments);

        public Task<object> GetStudentDataFullCalendar(Guid studentId, Guid termId)
            => _tmpEnrollmentRepository.GetStudentDataFullCalendar(studentId, termId);

        public Task<Tuple<bool, string>> Insert(TmpEnrollment tmpEnrollment)
            => _tmpEnrollmentRepository.InsertWithValidations(tmpEnrollment);

        public Task RemoveActiveSections(Guid courseTermId, Guid studentId)
            => _tmpEnrollmentRepository.RemoveActiveSections(courseTermId, studentId);

        public Task<bool> ValidateIntersection(Guid studentId, Guid termId, Guid courseTermId, List<ClassSchedule> classSchedules)
         => _tmpEnrollmentRepository.ValidateIntersection(studentId, termId, courseTermId, classSchedules);

        public async Task<object> GetAllWithData(string userId, Guid termId)
            => await _tmpEnrollmentRepository.GetAllWithData(userId, termId);

        public async Task<decimal> GetUserCredits(string userId, int status, Guid courseId)
            => await _tmpEnrollmentRepository.GetUserCredits(userId, status, courseId);

        public async Task<bool> GetIntersection(ICollection<ClassSchedule> classSchedules, Guid studentId, int status, Guid courseId)
            => await _tmpEnrollmentRepository.GetIntersection(classSchedules, studentId, status, courseId);

        public async Task<List<TmpEnrollment>> GetActiveSections(Guid courseTermId)
            => await _tmpEnrollmentRepository.GetActiveSections(courseTermId);

        public async Task<TmpEnrollment> GetStudentSection(Guid id, string userId)
            => await _tmpEnrollmentRepository.GetStudentSection(id, userId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetPendingRectificactionsDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user = null)
            => await _tmpEnrollmentRepository.GetPendingRectificactionsDatatable(sentParameters, user);

        public async Task<bool> HasPendingRectificationChanges(Guid studentId)
            => await _tmpEnrollmentRepository.HasPendingRectificationChanges(studentId);

        public async Task Update(TmpEnrollment tmpEnrollment) => await _tmpEnrollmentRepository.Update(tmpEnrollment);
        public async Task Delete(TmpEnrollment tmpEnrollment) => await _tmpEnrollmentRepository.Delete(tmpEnrollment);

        public async Task<List<Guid>> GetCoursesIdByStudentAndTermId(Guid studentId, Guid termId)
        {
            return await _tmpEnrollmentRepository.GetCoursesIdByStudentAndTermId(studentId, termId);
        }

        public async Task<List<TmpEnrollment>> GetAllByStudentAndTermId(Guid studentId, Guid termId)
        {
            return await _tmpEnrollmentRepository.GetAllByStudentAndTermId(studentId,termId);
        }

        public async Task<int> GetUserCredits(Guid studentId, Guid curriculumId, Guid courseTermId, bool electives)
        {
            return await _tmpEnrollmentRepository.GetUserCredits(studentId, curriculumId, courseTermId, electives);
        }

        public async Task<int> ParallelCoursesCount(Guid studentId, Guid termId, Guid courseTermId)
        {
            return await _tmpEnrollmentRepository.ParallelCoursesCount(studentId, termId, courseTermId);
        }

        public async Task InsertRange(List<TmpEnrollment> tmpList)
        {
            await _tmpEnrollmentRepository.InsertRange(tmpList);
        }

        public async Task<TmpEnrollment> GetByStudentAndSection(Guid studentId, Guid sectionId)
            => await _tmpEnrollmentRepository.GetByStudentAndSection(studentId, sectionId);

        public async Task<Tuple<bool, string>> DeleteWithValidations(Guid tmpEnrollmentId, string userId = null)
            => await _tmpEnrollmentRepository.DeleteWithValidations(tmpEnrollmentId, userId);
    }
}

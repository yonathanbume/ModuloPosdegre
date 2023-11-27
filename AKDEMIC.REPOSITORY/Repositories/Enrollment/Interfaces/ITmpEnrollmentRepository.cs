using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces
{
    public interface ITmpEnrollmentRepository : IRepository<TmpEnrollment>
    {
        Task<object> GetStudentDataFullCalendar(Guid studentId, Guid termId);
        Task<object> GetStudentDataDatatableClientSide(Guid studentId, Guid termId);
        Task<bool> ValidateIntersection(Guid studentId, Guid termId, Guid courseTermId, List<ClassSchedule> classSchedules);
        Task RemoveActiveSections(Guid courseTermId, Guid studentId);
        Task<object> GetAllWithData(string userId, Guid termId);
        Task<decimal> GetUserCredits(string userId, int status, Guid courseId);
        Task<bool> GetIntersection(ICollection<ClassSchedule> classSchedules, Guid studentId, int status, Guid courseId);
        Task<bool> HasPendingRectificationChanges(Guid studentId);
        Task<bool> IsParallelCourse(Guid studentId, Guid courseTermId);
        Task<bool> ValidateParallelCoursesLimit(Guid studentId, Guid courseTermId);
        Task<List<TmpEnrollment>> GetActiveSections(Guid courseTermId);
        Task<TmpEnrollment> GetStudentSection(Guid id, string userId);
        Task<DataTablesStructs.ReturnedData<object>> GetPendingRectificactionsDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user = null);
        Task<Tuple<bool, string>> InsertWithValidations(TmpEnrollment tmpEnrollment);
        Task<List<Guid>> GetCoursesIdByStudentAndTermId(Guid studentId, Guid termId);
        Task<List<TmpEnrollment>> GetAllByStudentAndTermId(Guid studentId, Guid termId);
        Task<int> GetUserCredits(Guid studentId, Guid curriculumId, Guid courseTermId, bool electives);
        Task<int> ParallelCoursesCount(Guid studentId, Guid termId, Guid courseTermId);
        Task<TmpEnrollment> GetByStudentAndSection(Guid studentId, Guid sectionId);
        Task<Tuple<bool, string>> DeleteWithValidations(Guid tmpEnrollmentId, string userId = null);
    }
}

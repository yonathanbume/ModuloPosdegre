using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.CourseTerm;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Implementations
{
    public sealed class CourseTermService : ICourseTermService
    {
        private readonly ICourseTermRepository _courseTermRepository;

        public CourseTermService(ICourseTermRepository courseTermRepository)
        {
            _courseTermRepository = courseTermRepository;
        }

        public async Task<IEnumerable<CourseTerm>> GetAll()
            => await _courseTermRepository.GetAll();
        public Task<bool> AnyByCourseIdAndTermStatus(Guid courseId, int termStatus)
            => _courseTermRepository.AnyByCourseIdAndTermStatus(courseId, termStatus);

        public Task DeleteAsync(CourseTerm courseTerm)
            => _courseTermRepository.Delete(courseTerm);

        public Task<object> GetAsModelA(Guid? termId = null, Guid? courseId = null)
            => _courseTermRepository.GetAsModelA(termId, courseId);

        public async Task<IEnumerable<Select2Structs.Result>> GetCourseTermByTermSelect2ClientSide(Guid termId, string academicCoordinatorId = null, Guid? careerId = null)
            => await _courseTermRepository.GetCourseTermByTermSelect2ClientSide(termId, academicCoordinatorId, careerId);


        public async Task<DataTablesStructs.ReturnedData<CourseTermDataTemplate>> GetCourseTermDataTable(DataTablesStructs.SentParameters parameters,Guid carId, Guid curId, Guid cid, Guid pid,string search)
        {
            return await _courseTermRepository.GetCourseTermDataTable(parameters,carId,curId, cid, pid,search);
        }

        public async Task<Select2Structs.ResponseParameters> GetCourseTermsByTermSelect2(Select2Structs.RequestParameters requestParameters, Guid termId, string userId = null)
        {
            return await _courseTermRepository.GetCourseTermsByTermSelect2(requestParameters, termId, userId);
        }

        public async Task<CourseTerm> GetCourseTermWithCourse(Guid ctid)
        {
            return await _courseTermRepository.GetCourseTermWithCourse(ctid);
        }

        public Task<CourseTerm> GetCourseTermWithTermAndSections(Guid id)
        {
            return _courseTermRepository.GetCourseTermWithTermAndSections(id);
        }

        Task<CourseTerm> ICourseTermService.GetAsync(Guid id)
        {
            return _courseTermRepository.Get(id);
        }

        Task<CourseTerm> ICourseTermService.GetByFilters(Guid? courseId, Guid? termId)
        {
            return _courseTermRepository.GetByFilters(courseId, termId);
        }

        Task<CourseTermATemplate> ICourseTermService.GetCourseTermATemplateById(Guid id)
        {
            return _courseTermRepository.GetCourseTermATemplateById(id);
        }

        Task ICourseTermService.InsertAsync(CourseTerm courseTerm)
        {
            return _courseTermRepository.Insert(courseTerm);
        }

        Task ICourseTermService.UpdateAsync(CourseTerm courseTerm)
        {
            return _courseTermRepository.Update(courseTerm);
        }

        public async Task<CourseTerm> GetcourseTermEnrollemtn(Guid courseId, Guid? termId = null)
            => await _courseTermRepository.GetcourseTermEnrollemtn(courseId, termId);

        public async Task<object> GetCourseTerms(Guid cid, string q)
            => await _courseTermRepository.GetCourseTerms(cid, q);

        public async Task<IEnumerable<CourseTerm>> GetAllByTermAndCareer(Guid? careerId = null, Guid? termId = null)
            => await _courseTermRepository.GetAllByTermAndCareer(careerId,termId);

        public async Task<bool> AnyByIdAndTermStatus(Guid id, int termStatus)
        {
            return await _courseTermRepository.AnyByIdAndTermStatus(id, termStatus);
        }

        public async  Task DeleteById(Guid id)
        {
            await _courseTermRepository.DeleteById(id);
        }

        public async Task AddRangeAsync(CourseTerm[] listCourseTerm)
        {
            await _courseTermRepository.AddRange(listCourseTerm);
        }

        public async Task InsertRange(CourseTerm[] listCourseTerm)
        {
            await _courseTermRepository.InsertRange(listCourseTerm);
        }

        public async Task<CourseTerm> GetFirstByCourseAndeTermId(Guid courseId, Guid termId)
        {
            return await _courseTermRepository.GetFirstByCourseAndeTermId(courseId, termId);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetCourseTermSectionsDataTable(DataTablesStructs.SentParameters parameters, Guid careerId, Guid curriculumId, Guid cid, Guid pid, string search)
            => await _courseTermRepository.GetCourseTermSectionsDataTable(parameters, careerId, curriculumId, cid, pid, search);

        public async Task<DataTablesStructs.ReturnedData<object>> GetCourseGradeStatisticsDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? careerId = null, Guid? curriculumId = null, ClaimsPrincipal user = null, string search = null)
            => await _courseTermRepository.GetCourseGradeStatisticsDatatable(sentParameters, termId, careerId, curriculumId, user, search);

        public async Task<DataTablesStructs.ReturnedData<object>> GetCourseAttendanceStatisticsDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? careerId = null, Guid? curriculumId = null, ClaimsPrincipal user = null, string search = null)
            => await _courseTermRepository.GetCourseAttendanceStatisticsDatatable(sentParameters, termId, careerId, curriculumId, user, search);
        
        public async Task<DataTablesStructs.ReturnedData<object>> GetCourseTermReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid curriculumId, int academicYear)
            => await _courseTermRepository.GetCourseTermReportDatatable(sentParameters, termId, curriculumId, academicYear);

        public async Task<List<CourseStatisticTemplate>> GetCourseGradeStatisticsTemplate(Guid termId, Guid? careerId = null, Guid? curriculumId = null, ClaimsPrincipal user = null)
            => await _courseTermRepository.GetCourseGradeStatisticsTemplate(termId, careerId, curriculumId, user);

        public async Task<List<CourseStatisticTemplate>> GetCourseAttendanceStatisticsTemplate(Guid termId, Guid? careerId = null, Guid? curriculumId = null, ClaimsPrincipal user = null)
            => await _courseTermRepository.GetCourseAttendanceStatisticsTemplate(termId, careerId, curriculumId, user);

        public async Task<object> GetCoursesWithSectionsSelect(Guid termId, Guid? careerId, Guid? curriculumId, int? academicYear)
            => await _courseTermRepository.GetCoursesWithSectionsSelect(termId, careerId, curriculumId, academicYear);
    }
}
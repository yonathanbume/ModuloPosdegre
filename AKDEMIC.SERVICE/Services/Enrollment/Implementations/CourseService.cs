using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.Course;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Implementations
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;

        public CourseService(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        Task<int> ICourseService.Count()
        {
            return _courseRepository.Count();
        }

        Task<IEnumerable<Course>> ICourseService.GetAllByTeacherId(string teacherId, Guid termId)
        {
            return _courseRepository.GetAllByTeacherId(teacherId, termId);
        }

        Task<IEnumerable<CourseATemplate>> ICourseService.GetCoursesATemplate(Guid termId, Guid? careerId, Guid? courseTypeId, ClaimsPrincipal user = null)
        {
            return _courseRepository.GetCoursesATemplate(termId, careerId, courseTypeId,user);
        }

        Task<CourseBTemplate> ICourseService.GetCourseBTemplate(Guid courseTermId)
        {
            return _courseRepository.GetCourseBTemplate(courseTermId);
        }

        Task<DataTablesStructs.ReturnedData<object>> ICourseService.GetAllByTermAndAreaCareerAndAcademicYearAndPaginationParameters(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid areaCareerId, Guid academicProgramId, byte? academicYear, string searchValue, bool? onlyWithSections = null, ClaimsPrincipal user = null, bool? onlyWithOutCoordinatos = null, Guid? curriculumId = null)
        {
            return _courseRepository.GetAllByTermAndAreaCareerAndAcademicYearAndPaginationParameters(sentParameters, termId, areaCareerId, academicProgramId, academicYear, searchValue, onlyWithSections,user, onlyWithOutCoordinatos, curriculumId);
        }

        Task<DataTablesStructs.ReturnedData<object>> ICourseService.GetAllWithSyllabusByTermAndPaginationParameters(DataTablesStructs.SentParameters sentParameters, Guid termId)
        {
            return _courseRepository.GetAllWithSyllabusByTermAndPaginationParameters(sentParameters, termId);
        }

        Task<DataTablesStructs.ReturnedData<object>> ICourseService.GetAllWithSyllabusComplianceDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, int state, Guid careerOrAreaId, ClaimsPrincipal user = null)
        {
            return _courseRepository.GetAllWithSyllabusComplianceDatatable(sentParameters, termId, state, careerOrAreaId, user);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllByTermIdAndPaginationParameters(DataTablesStructs.SentParameters sentParameters, Guid termId, string name = null)
        {
            return await _courseRepository.GetAllByTermIdAndPaginationParameters(sentParameters, termId, name);
        }

        public async Task<IEnumerable<Select2Structs.Result>> GetCoursesSelect2ClientSide(Guid? careerId = null, string search = null, Guid? curriculumId = null, bool onlyWithSections = false)
        {
            return await _courseRepository.GetCoursesSelect2ClientSide(careerId, search, curriculumId, onlyWithSections);
        }

        public async Task<IEnumerable<Select2Structs.Result>> GetCoursesSelect2ClientSide(Guid termId, Guid facultyId, Guid academicProgramId, Guid? curriculumId, string search = null)
        {
            return await _courseRepository.GetCoursesSelect2ClientSide(termId, facultyId, academicProgramId, curriculumId, search);
        }

        public Task<Course> GetAsync(Guid id)
        {
            return _courseRepository.Get(id);
        }

        public Task<Course> GetByCode(string code)
            => _courseRepository.GetByCode(code);

        public Task<bool> AnyByCodeAndName(string code, string name)
        {
            return _courseRepository.AnyByCodeAndName(code, name);
        }

        public Task InsertAsync(Course course)
        {
            return _courseRepository.Insert(course);
        }

        public Task UpdateAsync(Course course)
        {
            return _courseRepository.Update(course);
        }

        public Task DeleteAsync(Course course)
        {
            return _courseRepository.Delete(course);
        }

        public Task<object> GetAllAsSelectClientSide(string name = null, Guid? careerId = null, Guid? academicProgramId = null)
        {
            return _courseRepository.GetAllAsSelectClientSide(name, careerId, academicProgramId);
        }

        public Task<bool> AnyByCode(string code, Guid? id = null)
        {
            return _courseRepository.AnyByCode(code, id);
        }

        public Task<DataTablesStructs.ReturnedData<object>> GetAllAsModelB(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? careerId = null, string teacherId = null, bool forCareerDirector = false, Guid? areaCareerId = null, Guid? planId = null, string coordinatorId = null, Guid? programId = null, int? cycle = null, string search = null, ClaimsPrincipal user = null, bool? withSections = null)
        {
            return _courseRepository.GetAllAsModelB(sentParameters, termId, careerId, teacherId, forCareerDirector, areaCareerId, planId, coordinatorId, programId, cycle, search, user, withSections);
        }

        public async Task<Select2Structs.ResponseParameters> GetCoursesServerSideSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null, Guid? selectedId = null, ClaimsPrincipal user = null)
        {
            return await _courseRepository.GetCoursesServerSideSelect2(requestParameters, searchValue, selectedId, user);
        }

        public async Task<bool> AnyInArea(Guid areaId)
        {
            return await _courseRepository.AnyInArea(areaId);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllByParameters(DataTablesStructs.SentParameters paginationParameter, Guid pid, Guid aid, Guid cid, byte? ayid, Guid apid, string search)
        {
            return await _courseRepository.GetAllByParameters(paginationParameter, pid, aid, cid, ayid, apid, search);
        }

        public async Task DeleteById(Guid id)
        {
            await _courseRepository.DeleteById(id);
        }

        public async Task<object> GetWithAcademicHistory(Guid id, Guid studentId, Guid curriculumId)
        {
            return await _courseRepository.GetWithAcademicHistory(id, studentId, curriculumId);
        }

        public async Task<IEnumerable<Course>> GetAll()
        {
            return await _courseRepository.GetAll();
        }

        public async Task InsertRange(Course[] listCourse)
        {
            await _courseRepository.InsertRange(listCourse);
        }

        public async Task UpdateCourseCodeJob()
        {
            await _courseRepository.UpdateCourseCodeJob();
        }

        public async Task<Course> GetCourseBySectionId(Guid sectionId)
            => await _courseRepository.GetCourseBySectionId(sectionId);

        public async Task<Select2Structs.ResponseParameters> GetCoursesSelect2(Select2Structs.RequestParameters requestParameters, ClaimsPrincipal claimsPrincipal = null, string searchValue = null, Guid? academicProgramId = null, bool? generalCourses = null)
            => await _courseRepository.GetCoursesSelect2(requestParameters, claimsPrincipal, searchValue, academicProgramId, generalCourses);
        public async Task<DataTablesStructs.ReturnedData<object>> GetCoursesDatatble(DataTablesStructs.SentParameters parameters, Guid? termId, Guid? areaCareerId, Guid? academicProgramId, Guid? curriculumId, int? cycle, ClaimsPrincipal user, string searchValue)
            => await _courseRepository.GetCoursesDatatble(parameters, termId, areaCareerId, academicProgramId, curriculumId, cycle, user, searchValue);

        public async Task<DataTablesStructs.ReturnedData<object>> GetCoursesDatatable(DataTablesStructs.SentParameters sentParameters, Guid? careerId = null, Guid? programId = null, Guid? curriculumId = null, int? cycle = null, string search = null, ClaimsPrincipal user = null,Guid? termId = null)
        {
            return await _courseRepository.GetCoursesDatatable(sentParameters, careerId, programId, curriculumId, cycle, search, user, termId);
        }
       public async Task<EditTemplate> GetCourseEditTemplate(Guid id)
        {
            return await _courseRepository.GetCourseEditTemplate(id);
        }

        public async Task<object> GetTeacherCoursesSelect2ClientSide(string teacherId, Guid? termId = null)
            => await _courseRepository.GetTeacherCoursesSelect2ClientSide(teacherId, termId);

        public async Task<int> GetQuantityCoursesAssigned(Guid termId, string teacherId)
            => await _courseRepository.GetQuantityCoursesAssigned(termId, teacherId);

        public async Task<List<CourseTeacherReportExcel>> GetCourseTeacherExcel(ClaimsPrincipal user, Guid termId, Guid? careerId, Guid? academicProgramId, Guid? curriculumId, byte? academicYear, bool? onlyWithSections, bool? onlyWithOutCoordinators)
            => await _courseRepository.GetCourseTeacherExcel(user, termId, careerId, academicProgramId, curriculumId, academicYear, onlyWithSections, onlyWithOutCoordinators);

        public async Task<List<CourseScheduleLoad>> GetCourseSectionsScheduleLoad(Guid termId, Guid? careerId, Guid? academicProgramId, Guid? curriculumId, int? academicYear)
            => await _courseRepository.GetCourseSectionsScheduleLoad(termId, careerId, academicProgramId, curriculumId, academicYear);

        public async Task<DataTablesStructs.ReturnedData<object>> GetCurriculumsByCourseDatatable(DataTablesStructs.SentParameters parameters, Guid courseId)
            => await _courseRepository.GetCurriculumsByCourseDatatable(parameters, courseId);

        public async Task<bool> AnyCourseTerm(Guid courseId)
            => await _courseRepository.AnyCourseTerm(courseId);
    }
}
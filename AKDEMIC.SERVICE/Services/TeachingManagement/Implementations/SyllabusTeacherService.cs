using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Templates.SyllabusTeacher;
using AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces;

namespace AKDEMIC.SERVICE.Services.TeachingManagement.Implementations
{
    public sealed class SyllabusTeacherService : ISyllabusTeacherService
    {
        private readonly ISyllabusTeacherRepository _syllabusTeacherRepository;
        public SyllabusTeacherService(ISyllabusTeacherRepository syllabusTeacherRepository)
        {
            _syllabusTeacherRepository = syllabusTeacherRepository;
        }

        public async Task Delete(SyllabusTeacher syllabusTeacher)
            => await _syllabusTeacherRepository.Delete(syllabusTeacher);

        public async Task<SyllabusTeacher> Get(Guid id)
            => await _syllabusTeacherRepository.Get(id);

        public async Task<SyllabusTeacher> GetByCourseTermId(Guid courseTermId)
            => await _syllabusTeacherRepository.GetByCourseTermId(courseTermId);

        public async Task<object> GetChartJsReport(Guid termId, Guid? facultyId)
            => await _syllabusTeacherRepository.GetChartJsReport(termId, facultyId);

        public async Task<IEnumerable<SyllabusTeacherTemplate>> GetSyllabusTeacher(Guid termId, string teacherId)
            => await _syllabusTeacherRepository.GetSyllabusTeacher(termId, teacherId);

        public async Task<List<SyllabusTeacher>> GetSyllabusTeacherCourses(Guid termId, Guid careerId, Guid curriculumId)
            => await _syllabusTeacherRepository.GetSyllabusTeacherCourses(termId, careerId, curriculumId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetSyllabusTeacherDatatable(DataTablesStructs.SentParameters parameters, Guid termId, Guid academicDepartmentId, string teacherSearch, ClaimsPrincipal user = null)
            => await _syllabusTeacherRepository.GetSyllabusTeacherDatatable(parameters, termId, academicDepartmentId, teacherSearch, user);

        public async Task<DataTablesStructs.ReturnedData<object>> GetSyllabusTeacherOutOfDateDatatable(DataTablesStructs.SentParameters parameters, Guid termId, Guid? careerId, Guid? curriculumId)
            => await _syllabusTeacherRepository.GetSyllabusTeacherOutOfDateDatatable(parameters, termId, careerId, curriculumId);

        public async Task<List<SyllabusTeacherReportTemplate>> GetSyllabusTeacherReport(Guid termId, Guid academicDepartmentId, ClaimsPrincipal user = null)
            => await _syllabusTeacherRepository.GetSyllabusTeacherReport(termId, academicDepartmentId, user);

        public async Task Update(SyllabusTeacher entity)
            => await _syllabusTeacherRepository.Update(entity);

        Task ISyllabusTeacherService.InsertAsync(SyllabusTeacher syllabusTeacher)
            => _syllabusTeacherRepository.Insert(syllabusTeacher);
    }
}
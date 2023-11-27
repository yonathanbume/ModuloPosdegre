using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.Curriculum;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Interfaces
{
    public interface ICurriculumService
    {
        Task<DataTablesStructs.ReturnedData<object>> CurriculumsDatatable(DataTablesStructs.SentParameters sentParameters, Guid? facultyId = null, Guid? careerId = null, ClaimsPrincipal user = null, Guid? termId = null);
        Task<Curriculum> GetCareerLastCurriculum(Guid id);
        Task<Curriculum> GetCareerPreviousCurriculum(Guid id);
        Task<Curriculum> GetCurriculumPrevCurriculum(Guid curriculumId);

        Task<Curriculum> GetActiveByCareer(Guid careerId);
        Task ActivateCurriculum(Guid careerId, Guid curriculumId);
        Task ProcessStudents(Guid newCurriculumId, ClaimsPrincipal user = null);
        Task ProcessStudent(Guid studentId, Guid newCurriculumId);
        Task<DataTablesStructs.ReturnedData<object>> GetCurriculumDatatable(DataTablesStructs.SentParameters sentParameters, Guid faculty, Guid career,Guid academicProgram, string searchValue, ClaimsPrincipal user = null);
        Task<Curriculum> GetCurriculumByCareerId(Guid careerId);
        Task<Curriculum> GetAsync(Guid id);
        Task AddAsync(Curriculum curriculum);
        Task InsertAsync(Curriculum curriculum);
        Task UpdateAsync(Curriculum curriculum);
        Task DeleteAsync(Curriculum curriculum);
        Task<int> CountAsync(Guid? careerId = null);
        Task<CurriculumTemplateA> GetAsModelA(Guid? id = null);
        Task<object> GetAllAsModelB(Guid? facultyId = null, Guid? careerId = null, string coordinatorId = null);
        Task<CurriculumTemplateC> GetAsModelC(Guid? id = null);
        Task<CurriculumTemplateD> GetAsModelD(Guid? id = null);
        Task<bool> AnyNewByCareerId(Guid? careerId = null);
        Task<object> GetCareerCurriculumJson(Guid id, bool? onlyActive = null);
        Task<List<Curriculum>> GetAllCurriculumsByCareer(Guid? careerId = null, Guid? academicProgramId = null);
        Task<object> GetAllNumberPlusCodeByCareerId(Guid careerId, bool onlyActive = false);
        Task<List<byte>> GetAllAcademicYears(Guid? curriculumId = null);
        Task<object> GetAcademicProgramsCurriculumJson(Guid id, bool? onlyActive = false);
        Task<object> GetAllByAcademicProgramOrCareerJson(Guid careerId ,Guid? academicProgramId = null, ClaimsPrincipal user = null, bool? onlyActive = false);
        Task<Curriculum> Get(Guid id);
        Task<Curriculum> GetFirstByCourse(Guid courseId);
        Task UpdateCurriculumsAcademicProgramJob();
        Task<Curriculum> GetFirstByCareerAndCurriculumCode(string v1, string v2);
        Task<List<Guid>> GetCoursesWithSyllabusId(Guid termId, Guid careerId, Guid curriculumId);
        Task<Tuple<string, bool>> UpdateCurriculumCompetence(Guid curriculumId, List<Guid> competences);
        Task<List<Competencie>> GetCurriculumCompetencies(Guid curriculumId, byte? type);
        Task<List<Curriculum>> GetCurriculumsWithCareer();
        Task<object> GetCurriculumsLastYearActiveByCareerSelect2(Guid careerId);
    }
}
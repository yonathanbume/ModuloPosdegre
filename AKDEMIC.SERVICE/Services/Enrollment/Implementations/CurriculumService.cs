using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.Curriculum;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Implementations
{
    public class CurriculumService : ICurriculumService
    {
        private readonly ICurriculumRepository _curriculumRepository;

        public CurriculumService(ICurriculumRepository curriculumRepository)
        {
            _curriculumRepository = curriculumRepository;
        }

        public Task ActivateCurriculum(Guid careerId, Guid curriculumId) => _curriculumRepository.ActivateCurriculum(careerId, curriculumId);

        public Task<Curriculum> GetCareerLastCurriculum(Guid id) => _curriculumRepository.GetCareerLastCurriculum(id);

        public Task<Curriculum> GetCareerPreviousCurriculum(Guid id) => _curriculumRepository.GetCareerPreviousCurriculum(id);
        public Task<Curriculum> GetCurriculumPrevCurriculum(Guid curriculumId)
            => _curriculumRepository.GetCurriculumPrevCurriculum(curriculumId);

        public Task ProcessStudents(Guid newCurriculumId, ClaimsPrincipal user = null) => _curriculumRepository.ProcessStudents(newCurriculumId, user);

        public Task ProcessStudent(Guid studentId, Guid newCurriculumId) => _curriculumRepository.ProcessStudent(studentId, newCurriculumId);

        public Task<DataTablesStructs.ReturnedData<object>> CurriculumsDatatable(DataTablesStructs.SentParameters sentParameters, Guid? facultyId = null, Guid? careerId = null, ClaimsPrincipal user = null, Guid? termId = null)
            => _curriculumRepository.CurriculumsDatatable(sentParameters, facultyId, careerId,user, termId);

        public Task<Curriculum> GetActiveByCareer(Guid careerId)
            => _curriculumRepository.GetActiveByCareer(careerId);
        public async Task<DataTablesStructs.ReturnedData<object>> GetCurriculumDatatable(DataTablesStructs.SentParameters sentParameters, Guid faculty, Guid career, Guid academicProgram, string searchValue, ClaimsPrincipal user = null)
            => await _curriculumRepository.GetCurriculumDatatable(sentParameters, faculty, career, academicProgram, searchValue, user);

        public async Task<Curriculum> GetCurriculumByCareerId(Guid careerId)
            => await _curriculumRepository.GetCurriculumByCareerId(careerId);

        public Task<Curriculum> GetAsync(Guid id)
            => _curriculumRepository.Get(id);

        public Task AddAsync(Curriculum curriculum)
            => _curriculumRepository.Add(curriculum);

        public Task UpdateAsync(Curriculum curriculum)
            => _curriculumRepository.Update(curriculum);

        public Task DeleteAsync(Curriculum curriculum)
            => _curriculumRepository.Delete(curriculum);

        public Task<int> CountAsync(Guid? careerId = null)
            => _curriculumRepository.CountAsync(careerId);

        public Task<CurriculumTemplateA> GetAsModelA(Guid? id = null)
            => _curriculumRepository.GetAsModelA(id);

        public Task<object> GetAllAsModelB(Guid? facultyId = null, Guid? careerId = null, string coordinatorId = null)
            => _curriculumRepository.GetAllAsModelB(facultyId, careerId, coordinatorId);

        public Task<CurriculumTemplateC> GetAsModelC(Guid? id = null)
            => _curriculumRepository.GetAsModelC(id);

        public Task<CurriculumTemplateD> GetAsModelD(Guid? id = null)
            => _curriculumRepository.GetAsModelD(id);

        public Task<bool> AnyNewByCareerId(Guid? careerId = null)
            => _curriculumRepository.AnyNewByCareerId(careerId);

        public Task InsertAsync(Curriculum curriculum)
            => _curriculumRepository.Insert(curriculum);

        public Task<object> GetAllNumberPlusCodeByCareerId(Guid careerId, bool onlyActive = false)
            => _curriculumRepository.GetAllNumberPlusCodeByCareerId(careerId,onlyActive);

        public async Task<object> GetCareerCurriculumJson(Guid id, bool? onlyActive = null)
            => await _curriculumRepository.GetCareerCurriculumJson(id,onlyActive);

        public async Task<List<Curriculum>> GetAllCurriculumsByCareer(Guid? careerId = null, Guid? academicProgramId = null)
            => await _curriculumRepository.GetAllCurriculumsByCareer(careerId, academicProgramId);

        public async Task<object> GetAcademicProgramsCurriculumJson(Guid id, bool? onlyActive = false)
                    => await _curriculumRepository.GetAcademicProgramsCurriculumJson(id, onlyActive);

        public async Task<List<byte>> GetAllAcademicYears(Guid? curriculumId = null) => await _curriculumRepository.GetAllAcademicYears(curriculumId);

        public async Task<Curriculum> Get(Guid id)
            => await _curriculumRepository.Get(id);

        public async Task UpdateCurriculumsAcademicProgramJob()
        {
            await _curriculumRepository.UpdateCurriculumsAcademicProgramJob();
        }

        public async Task<Curriculum> GetFirstByCareerAndCurriculumCode(string careerCode, string curriculumCode)
        {
            return await _curriculumRepository.GetFirstByCareerAndCurriculumCode(careerCode, curriculumCode);
        }

        public async Task<object> GetAllByAcademicProgramOrCareerJson(Guid careerId, Guid? academicProgramId = null, ClaimsPrincipal user = null, bool? onlyActive = false)
        {
            return await _curriculumRepository.GetAllByAcademicProgramOrCareerJson(careerId, academicProgramId, user, onlyActive);
        }

        public async Task<List<Guid>> GetCoursesWithSyllabusId(Guid termId, Guid careerId, Guid curriculumId)
            => await _curriculumRepository.GetCoursesWithSyllabusId(termId, careerId, curriculumId);

        public async Task<Tuple<string, bool>> UpdateCurriculumCompetence(Guid curriculumId, List<Guid> competences)
            => await _curriculumRepository.UpdateCurriculumCompetence(curriculumId, competences);

        public async Task<List<Competencie>> GetCurriculumCompetencies(Guid curriculumId, byte? type)
            => await _curriculumRepository.GetCurriculumCompetencies(curriculumId, type);

        public Task<Curriculum> GetFirstByCourse(Guid courseId)
            => _curriculumRepository.GetFirstByCourse(courseId);

        public async Task<List<Curriculum>> GetCurriculumsWithCareer()
            => await _curriculumRepository.GetCurriculumsWithCareer();

        public Task<object> GetCurriculumsLastYearActiveByCareerSelect2(Guid careerId)
            => _curriculumRepository.GetCurriculumsLastYearActiveByCareerSelect2(careerId);
    }
}
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Generals.Templates.AcademicProgram;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Generals.Implementations
{
    public class AcademicProgramService : IAcademicProgramService
    {
        private readonly IAcademicProgramRepository _academicProgramRepository;

        public AcademicProgramService(IAcademicProgramRepository academicProgramRepository)
        {
            _academicProgramRepository = academicProgramRepository;
        }

        public Task<DataTablesStructs.ReturnedData<object>> AcademicProgramsDatatable(DataTablesStructs.SentParameters sentParameters, Guid? curriculumId = null)
            => _academicProgramRepository.AcademicProgramsDatatable(sentParameters, curriculumId);

        public Task<bool> AnyByName(string name, Guid? id = null)
            => _academicProgramRepository.AnyByName(name, id);

        public Task DeleteAsync(AcademicProgram academicProgram)
            => _academicProgramRepository.Delete(academicProgram);

        public async Task<AcademicProgram> Get(Guid id)
            => await _academicProgramRepository.Get(id);

        public async Task<IEnumerable<AcademicProgram>> GetAll()
            => await _academicProgramRepository.GetAll();

        public async Task<Select2Structs.ResponseParameters> GetAcademicProgramByCareerSelect2(Select2Structs.RequestParameters requestParameters, Guid? selectedId, Guid? careerId, string searchValue = null)
            => await _academicProgramRepository.GetAcademicProgramByCareerSelect2(requestParameters, selectedId, careerId, searchValue);

        public async Task<Select2Structs.ResponseParameters> GetAcademicProgramsSelect2(Select2Structs.RequestParameters requestParameters, Guid? idSelected, string v)
            => await _academicProgramRepository.GetAcademicProgramSelect2(requestParameters, idSelected, v);

        public Task<object> GetAllAsModelA(Guid? facultyId = null, Guid? careerId = null, string coordinatorId = null)
            => _academicProgramRepository.GetAllAsModelA(facultyId, careerId, coordinatorId);

        public Task<object> GetAsModelB(Guid? id = null)
            => _academicProgramRepository.GetAsModelB(id);

        public Task InsertAsync(AcademicProgram academicProgram)
            => _academicProgramRepository.Insert(academicProgram);

        public Task UpdateAsync(AcademicProgram academicProgram)
            => _academicProgramRepository.Update(academicProgram);

        public async Task<object> GetAcademicProgramsSelect(Guid cid, bool HasAll = false, List<Guid> CareerIds = null)
            => await _academicProgramRepository.GetAcademicProgramsSelect(cid, HasAll, CareerIds);

        public async Task<IEnumerable<Select2Structs.Result>> GetAcademicProgramByCareerSelect2ClientSide(Guid? careerId = null, Guid? selectedId = null)
            => await _academicProgramRepository.GetAcademicProgramByCareerSelect2ClientSide(careerId,selectedId);

        public async Task<object> GetCareerAcademicProgramsJson(Guid id, bool onlyWithCourses = false)
            => await _academicProgramRepository.GetCareerAcademicProgramsJson(id, onlyWithCourses);

        public async Task<IEnumerable<AcademicProgram>> GetAllByCareer(Guid careerId)
            => await _academicProgramRepository.GetAllByCareer(careerId);

        public async Task<AcademicProgram> GetByCode(string code, Guid? careerId = null)
            => await _academicProgramRepository.GetByCode(code, careerId);

        public async Task<object> GetCurriculumAcademicProgramsJson(Guid id)
            => await _academicProgramRepository.GetCurriculumAcademicProgramsJson(id);

        public Task<object> GetCareerAcademicProgramsByPlan(Guid id)
            => _academicProgramRepository.GetCareerAcademicProgramsByPlan(id);

        public async Task<AcademicProgramReportTemplate> GetAcademicProgramsReportData(Guid termId, Guid facultyId, Guid careerId)
            => await _academicProgramRepository.GetAcademicProgramsReportData(termId, facultyId, careerId);

        public async Task<DataTablesStructs.ReturnedData<AcademicProgramModelATemplate>> GetAllAsModelADataTable(DataTablesStructs.SentParameters sentParameters, Guid? facultyId = null, Guid? careerId = null, string coordinatorId = null,string search=null, ClaimsPrincipal user = null)
        {
            return await _academicProgramRepository.GetAllAsModelADataTable(sentParameters, facultyId, careerId, coordinatorId,search,user);
        }

        public async Task LoadCurriculumAcademicProgramJob()
        {
             await _academicProgramRepository.LoadCurriculumAcademicProgramJob();
        }

        public async Task LoadStudentsAcademicProgramJob()
        {
             await _academicProgramRepository.LoadStudentsAcademicProgramJob();
        }

        public async Task<object> GetAcademicProgramByCoursesSelect2CliendSide(Guid careerId)
        {
            return await _academicProgramRepository.GetAcademicProgramByCoursesSelect2CliendSide(careerId);
        }

        public async Task<object> GetAcademicProgramByCampusIdSelect2ClientSide(Guid campusId, Guid? selectedId = null)
            => await _academicProgramRepository.GetAcademicProgramByCampusIdSelect2ClientSide(campusId, selectedId);
        public async Task<object> GetCareersToForum()
            => await _academicProgramRepository.GetCareersToForum();

        public async Task<DataTablesStructs.ReturnedData<object>> GetEnrolledStudentsByAcademicProgram(DataTablesStructs.SentParameters sentParameters, Guid termId, bool onlyWithStudents = false)
            => await _academicProgramRepository.GetEnrolledStudentsByAcademicProgram(sentParameters, termId, onlyWithStudents);

        public async Task<List<AcademicProgram>> GetAcademicProgramsByCurriculumId(Guid curriculumId)
            => await _academicProgramRepository.GetAcademicProgramsByCurriculumId(curriculumId);
    }
}
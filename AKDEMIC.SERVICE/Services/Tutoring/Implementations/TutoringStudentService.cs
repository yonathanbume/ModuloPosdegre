using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Tutoring;
using AKDEMIC.REPOSITORY.Repositories.Tutoring.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Tutoring.Templates;
using AKDEMIC.SERVICE.Services.Tutoring.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Tutoring.Implementations
{
    public class TutoringStudentService : ITutoringStudentService
    {
        private readonly ITutoringStudentRepository _tutoringStudentRepository;

        public TutoringStudentService(ITutoringStudentRepository tutoringStudentRepository)
        {
            _tutoringStudentRepository = tutoringStudentRepository;
        }

        public async Task<bool> Any(Guid tutoringStudentId)
            => await _tutoringStudentRepository.Any(tutoringStudentId);
        public async Task<bool> AnyByTermId(Guid tutoringStudentId, Guid termId)
            => await _tutoringStudentRepository.AnyByTermId(tutoringStudentId, termId);
        public async Task<int> CountByCareerIdAndTutorId(Guid? careerId = null, string tutorId = null, Guid? termId = null)
            => await _tutoringStudentRepository.CountByCareerIdAndTutorId(careerId, tutorId, termId);

        public async Task Delete(TutoringStudent tutoringStudent)
            => await _tutoringStudentRepository.Delete(tutoringStudent);

        public async Task DeleteById(Guid tutoringStudentId)
            => await _tutoringStudentRepository.DeleteById(tutoringStudentId);

        public async Task<TutoringStudent> Get(Guid tutoringStudentId)
            => await _tutoringStudentRepository.Get(tutoringStudentId);
        public async Task<TutoringStudent> GetWithData(Guid tutoringStudentId, Guid termId)
            => await _tutoringStudentRepository.GetWithData(tutoringStudentId, termId);

        public async Task<IEnumerable<TutoringStudent>> GetAll()
            => await _tutoringStudentRepository.GetAll();

        public async Task<IEnumerable<TutoringStudent>> GetAllAssignedToTutor(string tutorId, string search = null, Guid? termId = null)
            => await _tutoringStudentRepository.GetAllAssignedToTutor(tutorId, search, termId);

        public async Task<IEnumerable<TutoringStudent>> GetAllByCareerId(Guid careerId)
            => await _tutoringStudentRepository.GetAllByCareerId(careerId);
        
        public async Task<TutoringStudent> GetByUserId(string userId)
            => await _tutoringStudentRepository.GetByUserId(userId);

        public async Task<TutoringStudent> GetByStudentId(Guid studentId)
            => await _tutoringStudentRepository.GetByStudentId(studentId);

        public async Task<IEnumerable<TutoringStudent>> GetNewTutoringStudentsForTutor(string tutorId, string query, Guid? careerId = null)
            => await _tutoringStudentRepository.GetNewTutoringStudentsForTutor(tutorId, query, careerId);
        public async Task<object> GetNewTutoringStudentsAllForTutor(Select2Structs.RequestParameters requestParameters, Guid termId, string query, Guid? careerId = null, int? academicYear = null, int? condition = null)
    => await _tutoringStudentRepository.GetNewTutoringStudentsAllForTutor(requestParameters, termId, query, careerId, academicYear, condition);

        public async Task<DataTablesStructs.ReturnedData<object>> GetTutoringStudentsAdminViewDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId = null, string searchValue = null, Guid? careerId = null, string tutorId = null)
            => await _tutoringStudentRepository.GetTutoringStudentsAdminViewDatatable(sentParameters, termId, searchValue, careerId, tutorId);
        
        public async Task<DataTablesStructs.ReturnedData<object>> GetTutoringStudentsDatatable(DataTablesStructs.SentParameters sentParameters, string tutorId = null, Guid? termId = null, string searchValue = null, Guid? careerId = null)
            => await _tutoringStudentRepository.GetTutoringStudentsDatatable(sentParameters, tutorId, termId, searchValue, careerId);

        public async Task<DataTablesStructs.ReturnedData<Student>> GetTutoringStudentsByTermDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, string searchValue = null, Guid? careerId = null, string tutorId = null, int? color = null)
            => await _tutoringStudentRepository.GetTutoringStudentsByTermDatatable(sentParameters, termId, searchValue, careerId, tutorId, color);

        public async Task Insert(TutoringStudent tutoringStudent)
            => await _tutoringStudentRepository.Insert(tutoringStudent);

        public async Task<int> SumTimesUpdatedByCareerIdAndTutorId(Guid? careerId = null, string tutorId = null)
            => await _tutoringStudentRepository.SumTimesUpdatedByCareerIdAndTutorId(careerId, tutorId);

        public async Task Update(TutoringStudent tutoringStudent)
            => await _tutoringStudentRepository.Update(tutoringStudent);

        public async Task<List<TutoringStudent>> GetTutoringStudentsPdf(Guid? termId = null, string searchValue = null, Guid? careerId = null, string tutorId = null)
            => await _tutoringStudentRepository.GetTutoringStudentsPdf(termId, searchValue, careerId, tutorId);

        public async Task<IEnumerable<TutoringStudent>> GetAllWithInclude()
            => await _tutoringStudentRepository.GetAllWithInclude();
        public async Task<bool> AnyByUser(string userId)
            => await _tutoringStudentRepository.AnyByUser(userId);

        public async Task<object> GetTutoringStudentsProgressReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? careerId = null, string search = null, bool? showOnlyDissaproved = null)
            => await _tutoringStudentRepository.GetTutoringStudentsProgressReportDatatable(sentParameters, termId, careerId, search, showOnlyDissaproved);

        public Task<object> GetAllSelect2(Guid termId, Guid? careerId = null)
            => _tutoringStudentRepository.GetAllSelect2(termId, careerId);

        public Task<object> GetAllSearchStudent(string searchValue, Guid termId, Guid? careerId = null)
            => _tutoringStudentRepository.GetAllSearchStudent(searchValue, termId, careerId);

        public Task<List<TutoringStudentTemplate>> GetTutoringStudentsReportExcel(Guid? termId = null, Guid? careerId = null, string tutorId = null)
            => _tutoringStudentRepository.GetTutoringStudentsReportExcel(termId, careerId, tutorId);
    }
}

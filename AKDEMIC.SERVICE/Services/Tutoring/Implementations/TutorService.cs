using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Tutoring;
using AKDEMIC.REPOSITORY.Repositories.Tutoring.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Tutoring.Templates;
using AKDEMIC.SERVICE.Services.Tutoring.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Tutoring.Implementations
{
    public class TutorService : ITutorService
    {
        private readonly ITutorRepository _tutorRepository;

        public TutorService(ITutorRepository tutorRepository)
        {
            _tutorRepository = tutorRepository;
        }

        public async Task<bool> Any(string tutorId)
            => await _tutorRepository.Any(tutorId);

        public async Task<int> CountByCareerId(Guid? careerId = null)
            => await _tutorRepository.CountByCareerId(careerId);
        public async Task<int> CountByTutorsectionId(Guid careerId, Guid termId)
            => await _tutorRepository.CountByTutorsectionId(careerId, termId);

        public async Task<int> CountDictatedTutoringSession(int times, Guid? termId = null, Guid? careerId = null)
            => await _tutorRepository.CountDictatedTutoringSession(times, termId, careerId);

        public async Task Delete(Tutor tutor)
            => await _tutorRepository.Delete(tutor);

        public async Task DeleteById(string tutorId)
            => await _tutorRepository.DeleteById(tutorId);

        public async Task<Tutor> Get(string tutorId)
            => await _tutorRepository.Get(tutorId);

        public async Task<IEnumerable<Tutor>> GetAll()
            => await _tutorRepository.GetAll();

        public async Task<IEnumerable<Tutor>> GetAllByCareerIdAndTermId(Guid careerId, Guid? termId = null, string search = null)
            => await _tutorRepository.GetAllByCareerIdAndTermId(careerId, termId, search);
        
        public async Task<IEnumerable<Tutor>> GetAllByTutoringStudentId(Guid tutoringStudentId)
            => await _tutorRepository.GetAllByTutoringStudentId(tutoringStudentId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetTutorsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? careerId = null, int? type = null)
            => await _tutorRepository.GetTutorsDatatable(sentParameters, searchValue, careerId, type);
        public async Task<DataTablesStructs.ReturnedData<Teacher>> GetTutorsByTermDatatable(DataTablesStructs.SentParameters sentParameters, Guid termid, string searchValue = null, Guid? careerId = null)
            => await _tutorRepository.GetTutorsByTermDatatable(sentParameters,termid, searchValue, careerId);

        public async Task Insert(Tutor tutor)
            => await _tutorRepository.Insert(tutor);
        public async Task Add(Tutor tutor)
            => await _tutorRepository.Add(tutor);
        public async Task<int> SumTimesUpdatedByCareerId(Guid? careerId = null)
            => await _tutorRepository.SumTimesUpdatedByCareerId(careerId);

        public async Task Update(Tutor tutor)
            => await _tutorRepository.Update(tutor);
        public async Task<SupportOfficeUser> GetTutorSupportOffice(string tutorId)
            => await _tutorRepository.GetTutorSupportOffice(tutorId);
        public async Task<Tutor> GetWithData(string tutorId)
            => await _tutorRepository.GetWithData(tutorId);
        public async Task<Tutor> GetByUser(string userId)
            => await _tutorRepository.GetByUser(userId);

        public async Task<IEnumerable<Tutor>> GetAllByCareerIdAndHasTutoringStudent(Guid careerId, Guid termId, string search = null)
            => await _tutorRepository.GetAllByCareerIdAndHasTutoringStudent(careerId, termId, search);
        public async Task<object> GetReportCountByCareer(Guid termId, string searchValue)
            => await _tutorRepository.GetReportCountByCareer(termId, searchValue);
        public async Task<List<ReportCountByCareerTemplate>> GetReportCountByCareerTemplate(Guid termId)
            => await _tutorRepository.GetReportCountByCareerTemplate(termId);
        public async Task<DataTablesStructs.ReturnedData<object>> GetAllTutoringsMadeByTutorByFacultyIdAsDataTable(DataTablesStructs.SentParameters sentParameters, Guid? careerId = null, ClaimsPrincipal user = null)
            => await _tutorRepository.GetAllTutoringsMadeByTutorByFacultyIdAsDataTable(sentParameters, careerId, user);

        public Task<List<TutorTemplate>> GetTutorsReportExcel(Guid? careerId = null, int? type = null)
            => _tutorRepository.GetTutorsReportExcel(careerId, type);
    }
}

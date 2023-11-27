using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Tutoring;
using AKDEMIC.REPOSITORY.Repositories.Tutoring.Templates;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Tutoring.Interfaces
{
    public interface ITutorService
    {
        Task<Tutor> Get(string tutorId);
        Task<bool> Any(string tutorId);
        Task DeleteById(string tutorId);
        Task Delete(Tutor tutor);
        Task Update(Tutor tutor);
        Task Insert(Tutor tutor);
        Task Add(Tutor tutor);
        Task<IEnumerable<Tutor>> GetAll();
        Task<IEnumerable<Tutor>> GetAllByCareerIdAndTermId(Guid careerId, Guid? termId = null, string search = null);
        Task<DataTablesStructs.ReturnedData<object>> GetTutorsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? careerId = null, int? type = null);
        Task<List<TutorTemplate>> GetTutorsReportExcel(Guid? careerId = null, int? type = null);
        Task<DataTablesStructs.ReturnedData<Teacher>> GetTutorsByTermDatatable(DataTablesStructs.SentParameters sentParameters, Guid termid, string searchValue = null, Guid? careerId = null);
        Task<int> CountByCareerId(Guid? careerId = null);
        Task<int> CountByTutorsectionId(Guid careerId, Guid termId);
        Task<int> SumTimesUpdatedByCareerId(Guid? careerId = null);
        Task<IEnumerable<Tutor>> GetAllByTutoringStudentId(Guid tutoringStudentId);
        Task<int> CountDictatedTutoringSession(int times, Guid? termId = null, Guid? careerId = null);
        Task<SupportOfficeUser> GetTutorSupportOffice(string tutorId);
        Task<Tutor> GetWithData(string tutorId);
        Task<Tutor> GetByUser(string userId);
        Task<IEnumerable<Tutor>> GetAllByCareerIdAndHasTutoringStudent(Guid careerId, Guid termId, string search = null);
        Task<object> GetReportCountByCareer(Guid termId, string searchValue);
        Task<List<ReportCountByCareerTemplate>> GetReportCountByCareerTemplate(Guid termId);
        Task<DataTablesStructs.ReturnedData<object>> GetAllTutoringsMadeByTutorByFacultyIdAsDataTable(DataTablesStructs.SentParameters sentParameters, Guid? careerId = null, ClaimsPrincipal user = null);
    }
}

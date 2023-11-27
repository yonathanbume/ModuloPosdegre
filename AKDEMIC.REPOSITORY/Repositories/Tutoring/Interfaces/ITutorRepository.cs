using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Tutoring;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Tutoring.Templates;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Tutoring.Interfaces
{
    public interface ITutorRepository : IRepository<Tutor>
    {
        Task<IEnumerable<Tutor>> GetAllByCareerIdAndTermId(Guid careerId, Guid? termId = null, string search = null);
        Task<DataTablesStructs.ReturnedData<object>> GetTutorsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? careerId = null, int? type = null);
        Task<List<TutorTemplate>> GetTutorsReportExcel(Guid? careerId = null, int? type = null);
        Task<DataTablesStructs.ReturnedData<Teacher>> GetTutorsByTermDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, string searchValue = null, Guid? careerId = null);
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
        Task<DataTablesStructs.ReturnedData<object>> GetAllTutoringsMadeByTutorByFacultyIdAsDataTable(DataTablesStructs.SentParameters sentParameters, Guid? careerId = null , ClaimsPrincipal user = null);
    }
}

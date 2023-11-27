using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Tutoring;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Tutoring.Templates;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Tutoring.Interfaces
{
    public interface ITutoringStudentRepository : IRepository<TutoringStudent>
    {
        Task<TutoringStudent> GetByUserId(string userId);
        Task<TutoringStudent> GetByStudentId(Guid studentId);
        Task<TutoringStudent> GetWithData(Guid tutoringStudentId, Guid termId);
        Task<bool> AnyByTermId(Guid tutoringStudentId, Guid termId);
        Task<IEnumerable<TutoringStudent>> GetAllByCareerId(Guid careerId);
        Task<object> GetAllSelect2(Guid termId, Guid? careerId = null);
        Task<object> GetAllSearchStudent(string searchValue, Guid termId, Guid? careerId = null);
        Task<IEnumerable<TutoringStudent>> GetAllAssignedToTutor(string tutorId, string search = null, Guid? termId = null);
        Task<IEnumerable<TutoringStudent>> GetNewTutoringStudentsForTutor(string tutorId, string query, Guid? careerId = null);
        Task<object> GetNewTutoringStudentsAllForTutor(Select2Structs.RequestParameters requestParameters, Guid termId, string query, Guid? careerId = null, int? academicYear = null, int? condition = null);
        Task<DataTablesStructs.ReturnedData<object>> GetTutoringStudentsAdminViewDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId = null, string searchValue = null, Guid? careerId = null, string tutorId = null);
        Task<DataTablesStructs.ReturnedData<object>> GetTutoringStudentsDatatable(DataTablesStructs.SentParameters sentParameters, string tutorId = null, Guid? termId = null, string searchValue = null, Guid? careerId = null);
        Task<List<TutoringStudentTemplate>> GetTutoringStudentsReportExcel(Guid? termId = null, Guid? careerId = null, string tutorId = null);
        Task<DataTablesStructs.ReturnedData<Student>> GetTutoringStudentsByTermDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, string searchValue = null, Guid? careerId = null, string tutorId = null, int? color = null);
        Task<int> CountByCareerIdAndTutorId(Guid? careerId = null, string tutorId = null, Guid? termId = null);
        Task<int> SumTimesUpdatedByCareerIdAndTutorId(Guid? careerId = null, string tutorId = null);
        Task<List<TutoringStudent>> GetTutoringStudentsPdf(Guid? termId = null, string searchValue = null, Guid? careerId = null, string tutorId = null);
        Task<IEnumerable<TutoringStudent>> GetAllWithInclude();
        Task<bool> AnyByUser(string userId);
        Task<object> GetTutoringStudentsProgressReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? careerId = null, string search = null, bool? showOnlyDissaproved = null);
    }
}

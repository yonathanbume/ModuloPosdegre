using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Tutoring;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Tutoring.Interfaces
{
    public interface ITutorTutoringStudentRepository : IRepository<TutorTutoringStudent>
    {
        Task<bool> AnyByTutorIdAndTutoringStudentId(string tutorId, Guid tutoringStudentId, Guid term);
        Task<TutorTutoringStudent> GetByTutorIdAndTutoringStudentId(string tutorId, Guid tutoringStudentId);
        Task<IEnumerable<TutorTutoringStudent>> GetByTutorId(string tutorId);
        Task<IEnumerable<TutorTutoringStudent>> GetByTutoringStudentId(Guid tutoringStudentId);
        Task DeleteByTutorIdAndTutoringStudentId(string tutorId, Guid tutoringStudentId, Guid term);
        Task<bool> AnyByStudentId(Guid studentId);
        Task<TutorTutoringStudent> GetByStudentId(Guid studentId);
        Task<bool> AnyByCoordinator(string userId);
        Task<bool> AnyByTutor(string tutorId);
        Task<object> GetTutorsByTutoringStudent(Guid studentId, Guid termId);
        Task<DataTablesStructs.ReturnedData<object>> GetTutorByTermDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId = null, ClaimsPrincipal user = null);
        Task<object> GetTutorByTermChart(Guid? termId = null, ClaimsPrincipal user = null);
        Task<object> GetAllTutoringsChart(Guid? termId = null, ClaimsPrincipal user = null);
    }
}

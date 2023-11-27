using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Tutoring;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Tutoring.Interfaces
{
    public interface ITutorTutoringStudentService
    {
        Task<bool> AnyByTutorIdAndTutoringStudentId(string tutorId, Guid tutoringStudentId, Guid term);
        Task Insert(TutorTutoringStudent tutorTutoringStudent);
        Task InsertRange(IEnumerable<TutorTutoringStudent> tutorTutoringStudents);
        Task DeleteRange(IEnumerable<TutorTutoringStudent> tutorTutoringStudents);
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

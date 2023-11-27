using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Tutoring;
using AKDEMIC.REPOSITORY.Repositories.Tutoring.Templates;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Tutoring.Interfaces
{
    public interface ITutoringSessionStudentService
    {
        Task<TutoringSessionStudent> Get(Guid tutoringSessionStudentId);
        Task<TutoringSessionStudent> AddAsync(TutoringSessionStudent tutoringSessionStudent);
        Task Insert(TutoringSessionStudent tutoringSessionStudent);
        Task InsertRange(IEnumerable<TutoringSessionStudent> tutoringSessionStudents);
        Task Update(TutoringSessionStudent tutoringSessionStudent);
        Task UpdateRange(IEnumerable<TutoringSessionStudent> tutoringSessionStudents);
        Task DeleteById(Guid tutoringSessionStudentId);
        Task DeleteRange(IEnumerable<TutoringSessionStudent> tutoringSessionStudents);
        Task<DataTablesStructs.ReturnedData<TutoringSessionStudent>> GetTutoringSessionStudentsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? supportOfficeId = null, Guid? careerId = null, Guid? termId = null, string tutorId = null, bool? attended = null);
        Task<List<TutoringSessionStudent>> GetTutoringSessionStudents(Guid supportOfficeId, string search = null, Guid? careerId = null, Guid? termId = null);
        Task<TutoringSessionStudent> GetByTutoringSessionIdAndTutoringStudentId(Guid tutoringSessionId, Guid tutoringStudentId);
        Task<IEnumerable<TutoringSessionStudent>> GetAllByTutoringSessionId(Guid tutoringSessionId);
        Task<IEnumerable<TutoringSessionStudent>> GetAllByTutoringStudentId(Guid tutoringStudentId);
        Task<bool> AnyByTutorIdAndTutoringStudentId(Guid tutoringStudentId, string tutorId, bool? absent = null, Guid? termId = null);
        Task<List<TutoringSessionStudent>> GetWithData(Guid tutoringSessionId, Guid tutoringStudentId);
        Task<DataTablesStructs.ReturnedData<object>> GetTutoringSessionStudentsDatatableByStudent(DataTablesStructs.SentParameters sentParameters, Guid studentId, Guid? termId = null, string search = null);
        Task<IEnumerable<TutoringSessionStudent>> GetAllWithInclude();
        Task<DataTablesStructs.ReturnedData<object>> GetStudentSessionsDatatable(DataTablesStructs.SentParameters sentParameters, Guid studentId, Guid termId);
    }
}

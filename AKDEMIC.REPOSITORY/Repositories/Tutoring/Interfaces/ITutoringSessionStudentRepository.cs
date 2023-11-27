using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Tutoring;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Tutoring.Templates;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Tutoring.Interfaces
{
    public interface ITutoringSessionStudentRepository : IRepository<TutoringSessionStudent>
    {
        Task<DataTablesStructs.ReturnedData<TutoringSessionStudent>> GetTutoringSessionStudentsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? supportOfficeId = null, Guid? careerId = null, Guid? termId = null, string tutorId = null, bool? attended = null);
        Task<List<TutoringSessionStudent>> GetTutoringSessionStudents(Guid supportOfficeId, string search = null, Guid? careerId = null, Guid? termId = null);
        Task<TutoringSessionStudent> GetByTutoringSessionIdAndTutoringStudentId(Guid tutoringSessionId, Guid tutoringStudentId);
        Task<IEnumerable<TutoringSessionStudent>> GetAllByTutoringSessionId(Guid tutoringSessionId);
        Task<IEnumerable<TutoringSessionStudent>> GetAllByTutoringStudentId(Guid tutoringStudentId);
        Task<bool> AnyByTutorIdAndTutoringStudentId(Guid tutoringStudentId, string tutorId, bool? absent = null, Guid? termId = null);
        Task<List<TutoringSessionStudent>> GetWithData(Guid tutoringSessionId, Guid tutoringStudentId);
        Task<DataTablesStructs.ReturnedData<object>> GetTutoringSessionStudentsDatatableByStudent(DataTablesStructs.SentParameters sentParameters, Guid studentId, Guid? termId = null, string search = null);
        Task<DataTablesStructs.ReturnedData<TutoringSessionStudent>> GetHistoryTutoringSessionStudentsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? supportOfficeId = null, Guid? careerId = null, Guid? termId = null, string tutorId = null, bool? attended = null);
        Task<IEnumerable<TutoringSessionStudent>> GetAllWithInclude();
        Task<DataTablesStructs.ReturnedData<object>> GetStudentSessionsDatatable(DataTablesStructs.SentParameters sentParameters, Guid studentId, Guid termId);
    }
}

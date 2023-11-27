using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Tutoring;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Tutoring.Interfaces
{
    public interface ITutoringAttendanceService
    {
        Task<DataTablesStructs.ReturnedData<TutoringAttendance>> GetTutoringAttendancesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? supportOfficeId = null, string tutorId = null, Guid? tutoringStudentId = null, Guid? termId = null, Guid? careerId = null);
        Task<List<TutoringAttendance>> GetTutoringAttendances(Guid supportOfficeId, Guid? termId = null, Guid? careerId = null, string searchValue = null);
        Task<IEnumerable<TutoringAttendance>> GetAll();
        Task<TutoringAttendance> Get(Guid id);
        Task<TutoringAttendance> GetWithData(Guid id);
        Task<object> GetInformation(Guid id);
        Task Insert(TutoringAttendance tutoringAttendance);
        Task Update(TutoringAttendance tutoringAttendance);
        Task DeleteById(Guid id);
        Task<IEnumerable<TutoringAttendance>> GetAllByTutorId(string tutorId, Guid? tutoringStudentId = null);
        Task<TutoringAttendance> GetAllByStudenIdAndSupportOfficeId(Guid tutoringStudentId, Guid supportOfficeId);
    }
}

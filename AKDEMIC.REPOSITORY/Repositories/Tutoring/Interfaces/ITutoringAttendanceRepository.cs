using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Tutoring;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Tutoring.Interfaces
{
    public interface ITutoringAttendanceRepository : IRepository<TutoringAttendance>
    {
        Task<DataTablesStructs.ReturnedData<TutoringAttendance>> GetTutoringAttendancesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? supportOfficeId = null, string tutorId = null, Guid? tutoringStudentId = null, Guid? termId = null, Guid? careerId = null);
        Task<List<TutoringAttendance>> GetTutoringAttendances(Guid supportOfficeId, Guid? termId = null, Guid? careerId = null, string searchValue = null);
        Task<IEnumerable<TutoringAttendance>> GetAllByTutorId(string tutorId, Guid? tutoringStudentId = null);
        Task<TutoringAttendance> GetAllByStudenIdAndSupportOfficeId(Guid tutoringStudentId, Guid supportOfficeId);
        Task<TutoringAttendance> GetWithData(Guid id);
        Task<object> GetInformation(Guid id);
    }
}

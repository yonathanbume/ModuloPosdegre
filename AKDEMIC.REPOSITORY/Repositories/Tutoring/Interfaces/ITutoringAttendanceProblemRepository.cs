using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Tutoring;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Tutoring.Interfaces
{
    public interface ITutoringAttendanceProblemRepository : IRepository<TutoringAttendanceProblem>
    {
        Task<DataTablesStructs.ReturnedData<TutoringAttendanceProblem>> GetTutoringAttendanceProblemsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? tutoringAttendanceId = null);
        Task<IEnumerable<TutoringAttendanceProblem>> GetAllByTutoringAttendanceId(Guid tutoringAttendanceId);
        Task<bool> AnyByTutoringAttendanceIdAndProblemId(Guid tutoringAttendanceId, Guid tutoringProblemId);
        Task<int> CountByTutoringProblemId(Guid? tutoringProblemId = null, byte? category = null, Guid? termId = null, Guid? careerId = null, string tutorId = null);
    }
}

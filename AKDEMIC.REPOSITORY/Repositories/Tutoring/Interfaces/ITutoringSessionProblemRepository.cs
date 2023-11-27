using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Tutoring;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Tutoring.Implementations
{
    public interface ITutoringSessionProblemRepository : IRepository<TutoringSessionProblem>
    {
        Task<DataTablesStructs.ReturnedData<TutoringSessionProblem>> GetTutoringSessionProblemsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? tutoringSessionId = null);
        Task<IEnumerable<TutoringSessionProblem>> GetAllByTutoringSessionId(Guid tutoringSessionId);
        Task<bool> AnyByTutoringSessionIdAndProblemId(Guid tutoringSessionId, Guid tutoringProblemId);
        Task<int> CountByTutoringProblemId(Guid? tutoringProblemId = null, byte? category = null, Guid? termId = null, Guid? careerId = null, string tutorId = null, bool? isMultiple = null);
        Task<IEnumerable<TutoringSessionProblem>> GetAllWithInclude();
        Task<DataTablesStructs.ReturnedData<object>> GetStudentSessionProblemsDatatable(DataTablesStructs.SentParameters sentParameters, Guid studentId, Guid termId);
    }
}

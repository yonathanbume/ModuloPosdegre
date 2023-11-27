using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces
{
    public interface ITutorialStudentRepository : IRepository<TutorialStudent>
    {
        Task<object> GetStudentTutorialDone(Guid eid);
        Task<object> GetChartReport(Guid eid);
        Task<IEnumerable<Select2Structs.Result>> GetStudentsByTutorialIdSelect2ClientSide(Guid tutorialId);
        Task<object> GetStudents(Guid id, string userId);
        Task<object> GetTutorialStudentByTutorialId(Guid eid);
        Task<TutorialStudent> GetByTutorialIdAndStudentId(Guid tutorialId, Guid studentId);
    }
}

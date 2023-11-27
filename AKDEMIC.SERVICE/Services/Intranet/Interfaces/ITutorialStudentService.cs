using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface ITutorialStudentService
    {
        Task<object> GetStudentTutorialDone(Guid eid);
        Task<object> GetChartReport(Guid eid);
        Task<IEnumerable<Select2Structs.Result>> GetStudentsByTutorialIdSelect2ClientSide(Guid tutorialId);
        Task<object> GetStudents(Guid id, string userId);
        void RemoveRange(ICollection<TutorialStudent> tutorialStudents);
        Task<object> GetTutorialStudentByTutorialId(Guid eid);
        Task<IEnumerable<TutorialStudent>> GetAll();
        Task<TutorialStudent> GetByTutorialIdAndStudentId(Guid tutorialId, Guid studentId);
    }
}

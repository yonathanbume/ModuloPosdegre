using AKDEMIC.ENTITIES.Models.Intranet;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface ITutorialService
    {
        Task<object> GetProgramTutorials();
        Task<object> GetDoneTutorialsStartAndEndDate(DateTime startDate, DateTime endDate);
        Task<object> GetTutorials(DateTime startDate, DateTime endDate, string userId);
        Task<object> GetTutorialByIdAndUserId(Guid id, string userId);
        Task<Tutorial> AddAsync();
        Task<Tutorial> GetTutorialByIdAndUserIdEdit(Guid id, string userId);
        Task<bool> GetExistClassRoom(Guid id, Guid classroomId, DateTime startTime, DateTime endTime);
        Task<Tutorial> GetConflictedTutorial(Guid id, string teacherId, DateTime startTime, DateTime endTime);
        Task<Tutorial> GetWithDataById(Guid id);
        void Remove(Tutorial tutorial);
        Task<object> GetTutorialsByDatesAndTeacherId(DateTime startDate, DateTime endDate, string teacherId);
        Task<Tutorial> Get(Guid id);
    }
}

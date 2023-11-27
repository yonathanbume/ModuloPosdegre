using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class TutorialService : ITutorialService
    {
        private readonly ITutorialRepository _tutorialRepository;

        public TutorialService(ITutorialRepository tutorialRepository)
        {
            _tutorialRepository = tutorialRepository;
        }
        public async Task<object> GetProgramTutorials()
            => await _tutorialRepository.GetProgramTutorials();

        public async Task<object> GetDoneTutorialsStartAndEndDate(DateTime startDate, DateTime endDate)
            => await _tutorialRepository.GetDoneTutorialsStartAndEndDate(startDate, endDate);
        public async Task<object> GetTutorials(DateTime startDate, DateTime endDate, string userId)
            => await _tutorialRepository.GetTutorials(startDate, endDate, userId);
        public async Task<object> GetTutorialByIdAndUserId(Guid id, string userId)
            => await _tutorialRepository.GetTutorialByIdAndUserId(id, userId);
        public async Task<Tutorial> AddAsync()
            => await _tutorialRepository.AddAsync();
        public async Task<Tutorial> GetTutorialByIdAndUserIdEdit(Guid id, string userId)
            => await _tutorialRepository.GetTutorialByIdAndUserIdEdit(id, userId);
        public async Task<bool> GetExistClassRoom(Guid id, Guid classroomId, DateTime starTime, DateTime endTime)
            => await _tutorialRepository.GetExistClassRoom(id, classroomId, starTime, endTime);
        public async Task<Tutorial> GetConflictedTutorial(Guid id, string teacherId, DateTime startTime, DateTime endTime)
            => await _tutorialRepository.GetConflictedTutorial(id, teacherId, startTime, endTime);
        public async Task<Tutorial> GetWithDataById(Guid id)
            => await _tutorialRepository.GetWithDataById(id);
        public void RemoveRange(List<Tutorial> tutorials)
            => _tutorialRepository.RemoveRange(tutorials);
        public void Remove(Tutorial tutorial)
            => _tutorialRepository.Remove(tutorial);
        public async Task<object> GetTutorialsByDatesAndTeacherId(DateTime startDate, DateTime endDate, string teacherId)
            => await _tutorialRepository.GetTutorialsByDatesAndTeacherId(startDate, endDate, teacherId);
        public async Task<Tutorial> Get(Guid id)
            => await _tutorialRepository.Get(id);
    }
}

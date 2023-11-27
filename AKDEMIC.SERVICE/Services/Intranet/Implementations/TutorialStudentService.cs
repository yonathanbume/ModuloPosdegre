using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class TutorialStudentService : ITutorialStudentService
    {
        private readonly ITutorialStudentRepository _tutorialStudentRepository;

        public TutorialStudentService(ITutorialStudentRepository tutorialStudentRepository)
        {
            _tutorialStudentRepository = tutorialStudentRepository;
        }

        public async Task<object> GetStudentTutorialDone(Guid eid)
            => await _tutorialStudentRepository.GetStudentTutorialDone(eid);
        public async Task<object> GetChartReport(Guid eid)
            => await _tutorialStudentRepository.GetChartReport(eid);

        public async Task<IEnumerable<Select2Structs.Result>> GetStudentsByTutorialIdSelect2ClientSide(Guid tutorialId)
            => await _tutorialStudentRepository.GetStudentsByTutorialIdSelect2ClientSide(tutorialId);
        public async Task<object> GetStudents(Guid id, string userId)
            => await _tutorialStudentRepository.GetStudents(id, userId);
        public void RemoveRange(ICollection<TutorialStudent> tutorialStudents)
            => _tutorialStudentRepository.RemoveRange(tutorialStudents);
        public async Task<object> GetTutorialStudentByTutorialId(Guid eid)
            => await _tutorialStudentRepository.GetTutorialStudentByTutorialId(eid);
        public async Task<IEnumerable<TutorialStudent>> GetAll()
            => await _tutorialStudentRepository.GetAll();
        public async Task<TutorialStudent> GetByTutorialIdAndStudentId(Guid tutorialId, Guid studentId)
            => await _tutorialStudentRepository.GetByTutorialIdAndStudentId(tutorialId, studentId);
    }
}

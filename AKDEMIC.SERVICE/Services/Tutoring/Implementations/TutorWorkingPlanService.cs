using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Tutoring;
using AKDEMIC.REPOSITORY.Repositories.Tutoring.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Tutoring.Templates;
using AKDEMIC.SERVICE.Services.Tutoring.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Tutoring.Implementations
{
    public class TutorWorkingPlanService : ITutorWorkingPlanService
    {
        private readonly ITutorWorkingPlanRepository _tutorWorkingPlanRepository;

        public TutorWorkingPlanService(ITutorWorkingPlanRepository tutorWorkingPlanRepository)
        {
            _tutorWorkingPlanRepository = tutorWorkingPlanRepository;
        }

        public Task<bool> AnyByTermTutor(string tutorId, Guid termId)
            => _tutorWorkingPlanRepository.AnyByTermTutor(tutorId, termId);

        public Task Delete(TutorWorkingPlan tutorWorkingPlan)
            => _tutorWorkingPlanRepository.Delete(tutorWorkingPlan);

        public Task<TutorWorkingPlan> Get(Guid id)
            => _tutorWorkingPlanRepository.Get(id);

        public Task<IEnumerable<TutorWorkingPlan>> GetAll()
            => _tutorWorkingPlanRepository.GetAll();

        public Task<DataTablesStructs.ReturnedData<object>> GetAllByTutorDatatable(DataTablesStructs.SentParameters sentParameters, string tutorId)
            => _tutorWorkingPlanRepository.GetAllByTutorDatatable(sentParameters, tutorId);

        public Task<TutorWorkingPlanTemplate> GetInfo(Guid id)
            => _tutorWorkingPlanRepository.GetInfo(id);

        public Task Insert(TutorWorkingPlan tutorWorkingPlan)
            => _tutorWorkingPlanRepository.Insert(tutorWorkingPlan);

        public Task Update(TutorWorkingPlan tutorWorkingPlan)
            => _tutorWorkingPlanRepository.Update(tutorWorkingPlan);
    }
}

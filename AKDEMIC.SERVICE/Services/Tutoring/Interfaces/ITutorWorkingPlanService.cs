using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Tutoring;
using AKDEMIC.REPOSITORY.Repositories.Tutoring.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Tutoring.Interfaces
{
    public interface ITutorWorkingPlanService
    {
        Task<TutorWorkingPlan> Get(Guid id);
        Task<TutorWorkingPlanTemplate> GetInfo(Guid id);
        Task<bool> AnyByTermTutor(string tutorId, Guid termId);
        Task<IEnumerable<TutorWorkingPlan>> GetAll();
        Task Insert(TutorWorkingPlan tutorWorkingPlan);
        Task Update(TutorWorkingPlan tutorWorkingPlan);
        Task Delete(TutorWorkingPlan tutorWorkingPlan);
        Task<DataTablesStructs.ReturnedData<object>> GetAllByTutorDatatable(DataTablesStructs.SentParameters sentParameters, string tutorId);
    }
}

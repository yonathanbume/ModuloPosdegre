using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Tutoring;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Tutoring.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Tutoring.Interfaces
{
    public interface ITutorWorkingPlanRepository:IRepository<TutorWorkingPlan>
    {
        Task<bool> AnyByTermTutor(string tutorId, Guid termId);
        Task<TutorWorkingPlanTemplate> GetInfo(Guid id);
        Task<DataTablesStructs.ReturnedData<object>> GetAllByTutorDatatable(DataTablesStructs.SentParameters sentParameters, string tutorId);
    }
}

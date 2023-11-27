using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Evaluation;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Evaluation.Interfaces
{
    public interface IProjectScheduleHistoricRepository : IRepository<ProjectScheduleHistoric>
    {
        Task<DataTablesStructs.ReturnedData<ProjectScheduleHistoric>> GetProjectScheduleHistoricDatatable(DataTablesStructs.SentParameters sentParameters, Guid projectId);
    }
}

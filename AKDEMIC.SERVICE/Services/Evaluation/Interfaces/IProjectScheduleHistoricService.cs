using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Evaluation;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Evaluation.Interfaces
{
    public interface IProjectScheduleHistoricService
    {
        Task<DataTablesStructs.ReturnedData<ProjectScheduleHistoric>> GetProjectScheduleHistoricDatatable(DataTablesStructs.SentParameters sentParameters, Guid projectId);
        Task<ProjectScheduleHistoric> Get(Guid id);
        Task Insert(ProjectScheduleHistoric entity);
    }
}

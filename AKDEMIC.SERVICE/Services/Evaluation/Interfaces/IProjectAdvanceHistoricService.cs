using System;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Evaluation;

namespace AKDEMIC.SERVICE.Services.Evaluation.Interfaces
{
    public interface IProjectAdvanceHistoricService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetAdvanceHistoricDataTable(DataTablesStructs.SentParameters parameters, Guid id,string search = null);
        Task Insert(ProjectAdvanceHistoric projectAdvanceHistoric);
        Task Update(ProjectAdvanceHistoric update);
        Task<ProjectAdvanceHistoric> GetWithProjectAdvance(Guid id);
        Task<ProjectAdvanceHistoric> Get(Guid id);
    }
}

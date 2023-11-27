using System;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Evaluation;
using AKDEMIC.REPOSITORY.Base;

namespace AKDEMIC.REPOSITORY.Repositories.Evaluation.Interfaces
{
    public interface IProjectAdvanceHistoricRepository : IRepository<ProjectAdvanceHistoric>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetAdvanceHistoricDataTable(DataTablesStructs.SentParameters parameters, Guid id, string search);
        Task<ProjectAdvanceHistoric> GetWithProjectAdvance(Guid id);
    }
}

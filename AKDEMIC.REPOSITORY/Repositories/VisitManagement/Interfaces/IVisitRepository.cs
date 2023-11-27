using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.VisitManagement;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.VisitManagement.Interfaces
{
    public interface IVisitRepository : IRepository<Visit>
    {
        Task<DataTablesStructs.ReturnedData<Visit>> GetVisitDatatable(DataTablesStructs.SentParameters sentParameters, string startDate = null, string endDate = null, Guid? dependencyId = null, string search = null);
        Task<object> GetVisitByDependenciesChart();
        Task<DataTablesStructs.ReturnedData<Visit>> GetVisitDatatableToPublic(DataTablesStructs.SentParameters sentParameters, string dateTime);
    }
}

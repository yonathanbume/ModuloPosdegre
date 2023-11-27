using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.VisitManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.VisitManagement.Interfaces
{
    public interface IVisitService
    {
        Task<DataTablesStructs.ReturnedData<Visit>> GetVisitDatatable(DataTablesStructs.SentParameters sentParameters, string startDate = null, string endDate = null, Guid? dependencyId = null, string search = null);
        Task Insert(Visit visit);
        Task<Visit> Get(Guid id);
        Task Delete(Visit visit);
        Task<object> GetVisitByDependenciesChart();
        Task<IEnumerable<Visit>> GetAll();
        Task<DataTablesStructs.ReturnedData<Visit>> GetVisitDatatableToPublic(DataTablesStructs.SentParameters sentParameters, string dateTime);
    }
}

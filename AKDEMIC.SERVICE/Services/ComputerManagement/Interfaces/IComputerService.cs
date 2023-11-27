using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.ComputersManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.ComputerManagement.Interfaces
{
    public interface IComputerService
    {
        //Generales
        Task Insert(Computer computer);
        Task Update(Computer computer);
        Task Delete(Computer computer);
        Task DeleteById(Guid id);
        Task<Computer> Get(Guid id);
        Task<IEnumerable<Computer>> GetAll();

        Task<DataTablesStructs.ReturnedData<object>> GetComputerDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? dependencyId = null,
           string brand = null, int? start_year = null, int? end_year = null, Guid? status = null, string start_purchase = null,
           string end_purchase = null, string start_createdat = null, string end_createdat = null);
        Task<DataTablesStructs.ReturnedData<object>> GetComputerReportDatatable(DataTablesStructs.SentParameters sentParameters,Guid? dependencyId , Guid? type = null, Guid? state = null, string searchValue = null);
        Task<object> GetComputerReportChart(Guid? dependencyId, Guid? type = null, Guid? state = null);
        Task<object> GetReportByDependencyChart();
    }
}

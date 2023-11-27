using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Generals.Interfaces
{
    public interface ICareerAccreditationService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetCareerAcreditationsHistoryDatatable(DataTablesStructs.SentParameters parameters, Guid careerId, string searchValue);
        Task<DataTablesStructs.ReturnedData<object>> GetCareerAccreditationDatatable(DataTablesStructs.SentParameters sentParameters, Guid? careerId = null, DateTime? EndDate = null);
        Task<object> GetCareerAccreditationChart(Guid? careerId = null, DateTime? EndDate = null);
        Task Insert(CareerAccreditation entity);
        Task<List<CareerAccreditation>> GetDataList();
        Task<CareerAccreditation> Get(Guid id);
        Task Delete(CareerAccreditation entity);
    }
}

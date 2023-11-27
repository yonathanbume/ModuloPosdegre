using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces
{
    public interface ICareerAccreditationRepository : IRepository<CareerAccreditation>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetCareerAcreditationsHistoryDatatable(DataTablesStructs.SentParameters parameters, Guid careerId, string searchValue);
        Task<DataTablesStructs.ReturnedData<object>> GetCareerAccreditationDatatable(DataTablesStructs.SentParameters sentParameters, Guid? careerId = null, DateTime? EndDate = null);
        Task<object> GetCareerAccreditationChart(Guid? careerId = null, DateTime? EndDate = null);
        Task<List<CareerAccreditation>> GetDataList();
    }
}

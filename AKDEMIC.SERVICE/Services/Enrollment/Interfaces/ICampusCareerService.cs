using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Interfaces
{
    public interface ICampusCareerService
    {
        Task<IEnumerable<CampusCareer>> GetAll();
        Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue);
        Task<DataTablesStructs.ReturnedData<object>> GetDataByCampusDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue, Guid campus);
        Task Delete(CampusCareer area);
        Task DeleteById(Guid campus, Guid career);
        Task Insert(CampusCareer area);
        Task Update(CampusCareer area);
        Task<CampusCareer> Get(Guid campus, Guid career);
        Task<CampusCareer> GetCampusByIdAndCareer(Guid careerId, Guid campusId);
        Task<object> GetCareerSelect2ByCampusId(Guid campusId);
    }
}

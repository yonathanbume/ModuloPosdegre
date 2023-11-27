using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces
{
    public interface ICampusCareerRepository : IRepository<CampusCareer>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue, Guid? campus);
        Task<CampusCareer> GetCampusByIdAndCareer(Guid careerId, Guid campusId);
        Task<object> GetCareerSelect2ByCampusId(Guid campusId);
    }
}

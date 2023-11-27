using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Base;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces
{
    public interface ICoordinatorCareerRepository: IRepository<CoordinatorCareer>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetCoordinatorsCareerDatatable(DataTablesStructs.SentParameters sentParameters, Guid? careerId, string searchValue = null);
        Task<List<CoordinatorCareer>> GetCoordinatorCareer(string userId);
        Task<bool> AnyByUserId(string userId);
        Task DownloadExcel(IXLWorksheet worksheet);
        Task<object> GetCareersByFacultySelect2ClientSide(Guid? facultyId, ClaimsPrincipal user = null);
        Task<object> GetFacultieSelect2ClientSide(ClaimsPrincipal user = null);
    }
}

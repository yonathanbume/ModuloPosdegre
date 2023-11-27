using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.JobExchange.Interfaces
{
    public interface ICoordinatorCareerService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetCoordinatorsCareerDatatable(DataTablesStructs.SentParameters sentParameters, Guid? careerId, string searchValue = null);
        Task<bool> AnyByUserId(string userId);
        Task<List<CoordinatorCareer>> GetCoordinatorCareer(string userId);
        Task<CoordinatorCareer> Get(Guid id);
        Task Insert(CoordinatorCareer coordinatorCareer);
        Task Update(CoordinatorCareer coordinatorCareer);
        Task Delete(CoordinatorCareer coordinatorCareer);
        Task DeleteById(Guid id);
        Task DownloadExcel(IXLWorksheet worksheet);
        Task<object> GetCareersByFacultySelect2ClientSide(Guid? facultyId, ClaimsPrincipal user = null);
        Task<object> GetFacultieSelect2ClientSide(ClaimsPrincipal user = null);
    }
}

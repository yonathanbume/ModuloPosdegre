using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.AcademicExchange;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.AcademicExchange.Interfaces
{
    public interface IScholarshipRepository : IRepository<Scholarship>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetScholarshipDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal User, byte? program = null, string startDate = null, byte? status = null, string search = null);
        Task<object> GetScholarshipSelect2ClientSide(Guid? selectedId = null);
        Task<bool> HasPostulants(Guid scholarshipId);
        Task<bool> HasPostulations(Guid scholarshipId);
    }
}

using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.AcademicExchange;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.AcademicExchange.Interfaces
{
    public interface IScholarshipService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetScholarshipDatatable(DataTablesStructs.SentParameters sentParameters,ClaimsPrincipal User, byte? program = null, string startDate = null, byte? status = null, string search = null);
        Task<Scholarship> Get(Guid id);
        Task<IEnumerable<Scholarship>> GetAll();
        Task Insert(Scholarship entity);
        Task Delete(Scholarship entity);
        Task Update(Scholarship entity);
        Task<object> GetScholarshipSelect2ClientSide(Guid? selectedId = null);
        Task<bool> HasPostulants(Guid scholarshipId);
        Task<bool> HasPostulations(Guid scholarshipId);
    }
}

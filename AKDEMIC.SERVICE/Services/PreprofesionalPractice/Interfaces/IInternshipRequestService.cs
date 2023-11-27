using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.PreprofesionalPractice;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.PreprofesionalPractice.Interfaces
{
    public interface IInternshipRequestService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, ClaimsPrincipal user = null);
        Task<DataTablesStructs.ReturnedData<object>> GetDatatableBySupervisor(DataTablesStructs.SentParameters sentParameters, byte? status, string supervisorId, string searchValue = null, ClaimsPrincipal user = null);
        Task Delete(InternshipRequest internshipValidationRequest);
        Task DeleteById(Guid id);
        Task Insert(InternshipRequest internshipValidationRequest);
        Task Update(InternshipRequest internshipValidationRequest);
        Task<InternshipRequest> Get(Guid id);
        Task<IEnumerable<InternshipRequest>> GetAll();
    }
}

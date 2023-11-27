using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.PreprofesionalPractice;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.PreprofesionalPractice.Interfaces
{
    public interface IInternshipRequestRepository : IRepository<InternshipRequest>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, ClaimsPrincipal user = null);
        Task<DataTablesStructs.ReturnedData<object>> GetDatatableBySupervisor(DataTablesStructs.SentParameters sentParameters, byte? status, string supervisorId, string searchValue = null, ClaimsPrincipal user = null);
    }
}

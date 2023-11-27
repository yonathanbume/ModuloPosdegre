using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Templates;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.JobExchange.Interfaces
{
    public interface IInternshipRequestService
    {
        Task Insert(InternshipRequest model);
        Task<InternshipRequest> Get(Guid id);
        Task Update(InternshipRequest model);
        Task<bool> AnyByStudentExperience(Guid studentExperienceId);
        Task<DataTablesStructs.ReturnedData<object>> GetInternshipEmployeeDatatable(DataTablesStructs.SentParameters sentParameters, string userId, Guid? careerId = null, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetInternshipRequestDatatable(DataTablesStructs.SentParameters sentParameters, string userId, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetStudentExperienceDatatable(DataTablesStructs.SentParameters sentParameters, string userId, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetInternshipRequestDatatableTotal(DataTablesStructs.SentParameters sentParameters, List<Guid> careers, int ConvalidationType = 0 , string searchValue = null);
        Task<List<InternshipRequestTemplate>> GetInternshipRequestData(List<Guid> careers);
        Task<List<InternshipRequestTemplate>> GetInternshipRequestStudentData(Guid studentId);
    }
}

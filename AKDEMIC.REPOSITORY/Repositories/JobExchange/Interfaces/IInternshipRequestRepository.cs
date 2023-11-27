using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Templates;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces
{
    public interface IInternshipRequestRepository : IRepository<InternshipRequest>
    {
        Task<bool> AnyByStudentExperience(Guid studentExperienceId);
        Task<DataTablesStructs.ReturnedData<object>> GetInternshipEmployeeDatatable(DataTablesStructs.SentParameters sentParameters, string userId, Guid? careerId = null, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetInternshipRequestDatatable(DataTablesStructs.SentParameters sentParameters, string userId, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetStudentExperienceDatatable(DataTablesStructs.SentParameters sentParameters, string userId, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetInternshipRequestDatatableTotal(DataTablesStructs.SentParameters sentParameters, List<Guid> Careers, int ConvalidationType = 0, string searchValue = null);
        Task<List<InternshipRequestTemplate>> GetInternshipRequestData(List<Guid> careers);
        Task<List<InternshipRequestTemplate>> GetInternshipRequestStudentData(Guid studentId);
    }
}

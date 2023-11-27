using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces
{
    public interface IStudentObservationRepository : IRepository<StudentObservation>
    {
        Task<DataTablesStructs.ReturnedData<StudentObservation>> GetStudentObservationDatatable(DataTablesStructs.SentParameters sentParameters, Guid? studentId = null, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetObservationsDatatable(DataTablesStructs.SentParameters sentParameters, Guid studentId);
        Task<IEnumerable<StudentObservation>> GetAllByType(byte type);
        Task<StudentObservation> GetByStudentId(Guid studentId);
        Task SaveChanges();
        Task<List<StudentObservation>> GetStudentObservations(Guid termId, Guid studentId, byte type);
        Task<DataTablesStructs.ReturnedData<object>> GetResignatedStudentsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? termId = null, Guid? facultyId = null, Guid? careerId = null, ClaimsPrincipal user = null);
    }
}

using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface IStudentObservationService
    {
        Task<DataTablesStructs.ReturnedData<StudentObservation>> GetStudentObservationDatatable(DataTablesStructs.SentParameters sentParameters, Guid? studentId = null, string searchValue = null);
        Task Insert(StudentObservation entity);
        Task<StudentObservation> Add(StudentObservation entity);
        Task InsertRange(List<StudentObservation> entities);
        Task<DataTablesStructs.ReturnedData<object>> GetObservationsDatatable(DataTablesStructs.SentParameters sentParameters, Guid studentId);
        Task<StudentObservation> GetByStudentId(Guid studentId);
        Task<IEnumerable<StudentObservation>> GetAllByType(byte type);
        Task Update(StudentObservation entity);
        Task<StudentObservation> Get(Guid id);
        Task SaveChanges();
        Task<List<StudentObservation>> GetStudentObservations(Guid termId, Guid studentId, byte type);
        Task<DataTablesStructs.ReturnedData<object>> GetResignatedStudentsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? termId = null, Guid? facultyId = null, Guid? careerId = null, ClaimsPrincipal user = null);
    }
}

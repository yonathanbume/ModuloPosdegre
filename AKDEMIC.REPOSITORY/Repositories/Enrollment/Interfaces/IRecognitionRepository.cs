using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces
{
    public interface IRecognitionRepository : IRepository<Recognition>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, ClaimsPrincipal user = null);
        Task<DataTablesStructs.ReturnedData<object>> GetStudentsDataDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? facultyId = null, Guid? careerId = null, Guid? admissionType = null, ClaimsPrincipal user = null);
        Task<DataTablesStructs.ReturnedData<object>> GetCoursesDataDatatable(DataTablesStructs.SentParameters sentParameters, Guid recognitionId, string searchValue = null);
        Task<Recognition> GetByStudentId(Guid studentId);
    }
}

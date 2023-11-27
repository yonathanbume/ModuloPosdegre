using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Interfaces
{
    public interface IRecognitionService
    {
        Task<Recognition> Get(Guid id);
        Task Add(Recognition recognition);
        Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, ClaimsPrincipal user = null);
        Task<DataTablesStructs.ReturnedData<object>> GetStudentsDataDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? facultyId = null, Guid? careerId = null, Guid? admissionType = null, ClaimsPrincipal user = null);
        Task<DataTablesStructs.ReturnedData<object>> GetCoursesDataDatatable(DataTablesStructs.SentParameters sentParameters, Guid recognitionId, string searchValue = null);
        Task Insert(Recognition recognition);
        Task Update(Recognition recognition);
        Task<Recognition> GetByStudentId(Guid studentId);
    }
}

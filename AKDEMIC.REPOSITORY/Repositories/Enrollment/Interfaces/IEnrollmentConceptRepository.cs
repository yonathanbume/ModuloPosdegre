using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces
{
    public interface IEnrollmentConceptRepository : IRepository<EnrollmentConcept>
    {
        Task<bool> Any(byte type, Guid conceptId, byte? condition = null);
        Task<bool> AnyConceptToReplace(byte type, Guid conceptToReplaceId, Guid careerId);
        Task<bool> AnyByCareerAndAdmissionType(byte type, Guid? careerId, Guid? admissionTypeId, byte? condition = null);
        Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, byte? type = null, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetEnrolmentDataDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<List<EnrollmentConcept>> GetByTypeIncludeConcept(byte type);
        Task<List<EnrollmentConcept>> GetAllIncludeConcept();
    }
}

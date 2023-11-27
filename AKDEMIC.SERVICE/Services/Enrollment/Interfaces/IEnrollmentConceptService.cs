using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Interfaces
{
    public interface IEnrollmentConceptService
    {
        Task<bool> Any(byte type, Guid conceptId, byte? condition = null);
        Task<bool> AnyConceptToReplace(byte type, Guid conceptToReplaceId, Guid careerId);
        Task<bool> AnyByCareerAndAdmissionType(byte type, Guid? careerId, Guid? admissionTypeId, byte? condition = null);
        Task<EnrollmentConcept> Get(Guid id);
        Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, byte? type, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetEnrolmentDataDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task DeleteById(Guid id);
        Task Insert(EnrollmentConcept enrollmentConcept);
        Task<List<EnrollmentConcept>> GetByTypeIncludeConcept(byte type);
        Task<List<EnrollmentConcept>> GetAllIncludeConcept();
    }
}

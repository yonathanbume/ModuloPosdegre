using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Implementations
{
    public class EnrollmentConceptService : IEnrollmentConceptService
    {
        private readonly IEnrollmentConceptRepository _enrollmentConceptRepository;
        public EnrollmentConceptService(IEnrollmentConceptRepository enrollmentConceptRepository)
        {
            _enrollmentConceptRepository = enrollmentConceptRepository;
        }

        public async Task<bool> Any(byte type, Guid conceptId, byte? condition = null)
            => await _enrollmentConceptRepository.Any(type, conceptId);

        public async Task<bool> AnyByCareerAndAdmissionType(byte type, Guid? careerId, Guid? admissionTypeId, byte? condition = null)
             => await _enrollmentConceptRepository.AnyByCareerAndAdmissionType(type, careerId, admissionTypeId);

        public async Task DeleteById(Guid id) => await _enrollmentConceptRepository.DeleteById(id);

        public async Task<EnrollmentConcept> Get(Guid id) => await _enrollmentConceptRepository.Get(id);

        public async Task<List<EnrollmentConcept>> GetByTypeIncludeConcept(byte type)
            => await _enrollmentConceptRepository.GetByTypeIncludeConcept(type);

        public async Task<List<EnrollmentConcept>> GetAllIncludeConcept()
            => await _enrollmentConceptRepository.GetAllIncludeConcept();

        public Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, byte? type, string searchValue = null)
            => _enrollmentConceptRepository.GetDataDatatable(sentParameters, type, searchValue);

        public Task<DataTablesStructs.ReturnedData<object>> GetEnrolmentDataDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
            => _enrollmentConceptRepository.GetEnrolmentDataDatatable(sentParameters, searchValue);

        public async Task Insert(EnrollmentConcept enrollmentConcept) => await _enrollmentConceptRepository.Insert(enrollmentConcept);

        public async Task<bool> AnyConceptToReplace(byte type, Guid conceptToReplaceId, Guid careerId)
            => await _enrollmentConceptRepository.AnyConceptToReplace(type, conceptToReplaceId, careerId);
    }
}

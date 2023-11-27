using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Implementations
{
    public class EntrantEnrollmentService : IEntrantEnrollmentService
    {
        private readonly IEntrantEnrollmentRepository _entrantEnrollmentRepository;

        public EntrantEnrollmentService(IEntrantEnrollmentRepository entrantEnrollmentRepository)
        {
            _entrantEnrollmentRepository = entrantEnrollmentRepository;
        }

        public async Task InsertEntrantEnrollment(EntrantEnrollment entrantEnrollment) =>
            await _entrantEnrollmentRepository.Insert(entrantEnrollment);

        public async Task UpdateEntrantEnrollment(EntrantEnrollment entrantEnrollment) =>
            await _entrantEnrollmentRepository.Update(entrantEnrollment);

        public async Task DeleteEntrantEnrollment(EntrantEnrollment entrantEnrollment) =>
            await _entrantEnrollmentRepository.Delete(entrantEnrollment);

        public async Task<EntrantEnrollment> GetEntrantEnrollmentById(Guid id) =>
            await _entrantEnrollmentRepository.Get(id);

        public async Task<IEnumerable<EntrantEnrollment>> GetAllEntrantEnrollments() =>
            await _entrantEnrollmentRepository.GetAll();

        public async Task<IEnumerable<EntrantEnrollment>> GetAllWithData(Guid careerId, Guid? termId = null)
            => await _entrantEnrollmentRepository.GetAllWithData(careerId, termId);
        public async Task<EntrantEnrollment> GetEnrollmentByTermidAndCareerId(Guid careerId, Guid? termId = null)
            => await _entrantEnrollmentRepository.GetEnrollmentByTermidAndCareerId(careerId, termId);
    }
}

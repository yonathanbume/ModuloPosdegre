using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class GradeRegistrationService : IGradeRegistrationService
    {
        private readonly IGradeRegistrationRepository _gradeRegistrationRepository;

        public GradeRegistrationService(IGradeRegistrationRepository gradeRegistrationRepository)
        {
            _gradeRegistrationRepository = gradeRegistrationRepository;
        }

        public Task DeleteById(Guid id)
            => _gradeRegistrationRepository.DeleteById(id);

        public Task<GradeRegistration> Get(Guid id)
            => _gradeRegistrationRepository.Get(id);

        public Task<IEnumerable<GradeRegistration>> GetAll()
            => _gradeRegistrationRepository.GetAll();

        public Task<IEnumerable<GradeRegistration>> GetAllByFilter(Guid studentSectionId)
            => _gradeRegistrationRepository.GetAllByFilter(studentSectionId);

        public Task<GradeRegistration> GetByFilters(Guid sectionId, Guid? evaluationId, string userId)
            => _gradeRegistrationRepository.GetByFilters(sectionId, evaluationId, userId);

        public Task Insert(GradeRegistration gradeRegistration)
            => _gradeRegistrationRepository.Insert(gradeRegistration);

        public Task Update(GradeRegistration gradeRegistration)
            => _gradeRegistrationRepository.Update(gradeRegistration);
    }
}

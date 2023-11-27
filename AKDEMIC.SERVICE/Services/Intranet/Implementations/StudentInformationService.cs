using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class StudentInformationService : IStudentInformationService
    {
        private readonly IStudentInformationRepository _studentInformationRepository;

        public StudentInformationService(IStudentInformationRepository studentInformationRepository)
        {
            _studentInformationRepository = studentInformationRepository;
        }

        public async Task Update(StudentInformation studentInformation)
             => await _studentInformationRepository.Update(studentInformation);

        public async Task Insert(StudentInformation studentInformation)
            => await _studentInformationRepository.Insert(studentInformation);
        public async Task<bool> Any(Guid studentInformationId)
            => await _studentInformationRepository.Any(studentInformationId);

        public async Task Delete(StudentInformation studentInformation)
            => await _studentInformationRepository.Delete(studentInformation);

        public async Task DeleteById(Guid studentInformationId)
            => await _studentInformationRepository.DeleteById(studentInformationId);

        public async Task<StudentInformation> Get(Guid studentInformationId)
            => await _studentInformationRepository.Get(studentInformationId);

        public async Task<IEnumerable<StudentInformation>> GetAll()
            => await _studentInformationRepository.GetAll();

        public Task<object> GetOriginSchoolSelect()
            => _studentInformationRepository.GetOriginSchoolSelect();

        public Task<StudentInformation> GetByStudentAndTerm(Guid studentId, Guid termId)
            => _studentInformationRepository.GetByStudentAndTerm(studentId, termId);

        public Task<bool> HasStudentInformation(Guid studentId)
            => _studentInformationRepository.HasStudentInformation(studentId);
    }
}

using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Admission.Templates.AdmissionExamClassroomPostulant;
using AKDEMIC.SERVICE.Services.Admission.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Admission.Implementations
{
    public class AdmissionExamClassroomPostulantService : IAdmissionExamClassroomPostulantService
    {
        private readonly IAdmissionExamClassroomPostulantRepository _admissionExamClassroomPostulantRepository;

        public AdmissionExamClassroomPostulantService(IAdmissionExamClassroomPostulantRepository admissionExamClassroomPostulantRepository)
        {
            _admissionExamClassroomPostulantRepository = admissionExamClassroomPostulantRepository; 
        }

        public async Task Insert(AdmissionExamClassroomPostulant admissionExamClassroomPostulant) =>
            await _admissionExamClassroomPostulantRepository.Insert(admissionExamClassroomPostulant);

        public async Task Update(AdmissionExamClassroomPostulant admissionExamClassroomPostulant) =>
            await _admissionExamClassroomPostulantRepository.Update(admissionExamClassroomPostulant);

        public async Task Delete(AdmissionExamClassroomPostulant admissionExamClassroomPostulant) =>
            await _admissionExamClassroomPostulantRepository.Delete(admissionExamClassroomPostulant);

        public async Task<AdmissionExamClassroomPostulant> Get(Guid id) =>
            await _admissionExamClassroomPostulantRepository.Get(id);

        public async Task<IEnumerable<AdmissionExamClassroomPostulant>> GetAll() =>
            await _admissionExamClassroomPostulantRepository.GetAll();

        public async Task<List<AdmissionExamClassroomPostulant>> GetClassroomPostulants(Guid id)
            => await _admissionExamClassroomPostulantRepository.GetClassroomPostulants(id);
        public void RemoveRange(List<AdmissionExamClassroomPostulant> admissionExamClassroomPostulants)
            => _admissionExamClassroomPostulantRepository.RemoveRange(admissionExamClassroomPostulants);
        public async Task<List<AdmissionExamClassroomPostulant>> GetStudentClassroom(Guid id)
            => await _admissionExamClassroomPostulantRepository.GetStudentClassroom(id);
        public async Task AddRangeAsync(List<AdmissionExamClassroomPostulant> admissionExamClassroomPostulants)
            => await _admissionExamClassroomPostulantRepository.AddRange(admissionExamClassroomPostulants);
        public async Task InsertRange(List<AdmissionExamClassroomPostulant> admissionExamClassroomPostulants)
           => await _admissionExamClassroomPostulantRepository.InsertRange(admissionExamClassroomPostulants);

        public async Task<PostulantInformationTemplate> GetStudentClassroomInformation(Guid examId, string document)
           => await _admissionExamClassroomPostulantRepository.GetStudentClassroomInformation(examId, document);

    }
}

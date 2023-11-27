using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces;
using AKDEMIC.SERVICE.Services.Admission.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Admission.Implementations
{
    public class AdmissionExamClassroomService : IAdmissionExamClassroomService
    {
        private readonly IAdmissionExamClassroomRepository _admissionExamClassroomRepository;

        public AdmissionExamClassroomService(IAdmissionExamClassroomRepository admissionExamClassroomRepository)
        {
            _admissionExamClassroomRepository = admissionExamClassroomRepository;
        }

        public async Task Insert(AdmissionExamClassroom admissionExam) =>
            await _admissionExamClassroomRepository.Insert(admissionExam);

        public async Task Update(AdmissionExamClassroom admissionExam) =>
            await _admissionExamClassroomRepository.Update(admissionExam);

        public async Task Delete(AdmissionExamClassroom admissionExam) =>
            await _admissionExamClassroomRepository.Delete(admissionExam);

        public async Task<AdmissionExamClassroom> Get(Guid id) =>
            await _admissionExamClassroomRepository.Get(id);

        public async Task<IEnumerable<AdmissionExamClassroom>> GetAll() =>
            await _admissionExamClassroomRepository.GetAll();
        public async Task<object> GetClassrooms(Guid id)
            => await _admissionExamClassroomRepository.GetClassrooms(id);

        public async Task<DataTablesStructs.ReturnedData<object>> GetClassroomsDatatable(DataTablesStructs.SentParameters sentParameters, Guid id)
            => await _admissionExamClassroomRepository.GetClassroomsDatatable(sentParameters, id);
        public void Remove(AdmissionExamClassroom admissionExamClassroom)
            => _admissionExamClassroomRepository.Remove(admissionExamClassroom);
        public async Task<List<AdmissionExamClassroom>> GetClassroomsListById(Guid id)
            => await _admissionExamClassroomRepository.GetClassroomsListById(id);

        public async Task<bool> Any(Guid id, Guid classroomId)
        {
            return await _admissionExamClassroomRepository.Any(id,classroomId);
        }

        public async Task InsertRange(IEnumerable<AdmissionExamClassroom> examClassrooms)
        {
            await _admissionExamClassroomRepository.InsertRange(examClassrooms);
        }

        public async Task<AdmissionExamClassroom> GetWithClassroomBuildingAndCampus(Guid id)
        {
           return await _admissionExamClassroomRepository.GetWithClassroomBuildingAndCampus(id);
        }

        public async Task<IEnumerable<AdmissionExamClassroom>> GetListByTeacherId(string userId, Guid? admissionTermId = null)
        {
            return await _admissionExamClassroomRepository.GetListByTeacherId(userId, admissionTermId);
        }

        public async Task<List<AdmissionExamClassroom>> GetAllByExam(Guid examId)
            => await _admissionExamClassroomRepository.GetAllByExam(examId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetAssistanceDatatable(DataTablesStructs.SentParameters sentParameters, Guid admissionExamnId)
            => await _admissionExamClassroomRepository.GetAssistanceDatatable(sentParameters, admissionExamnId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetClassroomAssistanceDatatable(DataTablesStructs.SentParameters sentParameters, Guid classroomId)
            => await _admissionExamClassroomRepository.GetClassroomAssistanceDatatable(sentParameters, classroomId);
    }
}
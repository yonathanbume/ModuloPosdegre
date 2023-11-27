using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Admission.Templates.AdmissionExamClassroomTeacher;
using AKDEMIC.SERVICE.Services.Admission.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Admission.Implementations
{
    public class AdmissionExamClassroomTeacherService : IAdmissionExamClassroomTeacherService
    {
        private readonly IAdmissionExamClassroomTeacherRepository _admissionExamClassroomTeacherRepository;
        public AdmissionExamClassroomTeacherService(IAdmissionExamClassroomTeacherRepository admissionExamClassroomTeacherRepository)
        {
            _admissionExamClassroomTeacherRepository = admissionExamClassroomTeacherRepository;
        }

        public async Task DeleteByAdmissionExamClassroomId(Guid id)
        {
            await _admissionExamClassroomTeacherRepository.DeleteByAdmissionExamClassroomId(id);
        }

        public async Task DeleteById(Guid id) => await _admissionExamClassroomTeacherRepository.DeleteById(id);

        public async Task<List<AdmissionExamClassroomTeacher>> GetByAdmissionExamClassroomId(Guid id)
        {
            return await _admissionExamClassroomTeacherRepository.GetByAdmissionExamClassroomId(id);
        }

        public async Task<List<AdmissionExamClassroomTeacher>> GetByAdmissionExamId(Guid examId)
        {
            return await _admissionExamClassroomTeacherRepository.GetByAdmissionExamId(examId);
        }

        public async Task<List<ClassroomAssistanceTemplate>> GetClassroomsAssistanceData(Guid id)
        {
            return await _admissionExamClassroomTeacherRepository.GetClassroomsAssistanceData(id);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetClassroomsAssistanceDatatable(DataTablesStructs.SentParameters sentParameters, Guid id)
        {
            return await _admissionExamClassroomTeacherRepository.GetClassroomsAssistanceDatatable(sentParameters, id);
        }

        public async Task InsertRange(IEnumerable<AdmissionExamClassroomTeacher> newTeachers)
        {
            await _admissionExamClassroomTeacherRepository.InsertRange(newTeachers);
        }

        public async Task InsertRange(List<AdmissionExamClassroomTeacher> teachers)
            => await _admissionExamClassroomTeacherRepository.InsertRange(teachers);

        public async Task SaveTeacherAssistance(Guid id, List<string> lstToAdd, List<string> lstToAvoid)
        {
            await _admissionExamClassroomTeacherRepository.SaveTeacherAssistance(id, lstToAdd, lstToAvoid);
        }

        public async Task UpdateRange(List<AdmissionExamClassroomTeacher> teachers)
            => await _admissionExamClassroomTeacherRepository.UpdateRange(teachers);
    }
}

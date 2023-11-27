using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Repositories.Admission.Templates.AdmissionExamClassroomPostulant;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Admission.Interfaces
{
    public interface IAdmissionExamClassroomPostulantService
    {
        Task Insert(AdmissionExamClassroomPostulant admissionExamClassroomPostulant);
        Task Update(AdmissionExamClassroomPostulant admissionExamClassroomPostulant);
        Task Delete(AdmissionExamClassroomPostulant admissionExamClassroomPostulant);
        Task<AdmissionExamClassroomPostulant> Get(Guid id);
        Task<IEnumerable<AdmissionExamClassroomPostulant>> GetAll();
        Task<List<AdmissionExamClassroomPostulant>> GetClassroomPostulants(Guid id);
        void RemoveRange(List<AdmissionExamClassroomPostulant> admissionExamClassroomPostulants);
        Task<List<AdmissionExamClassroomPostulant>> GetStudentClassroom(Guid id);
        Task AddRangeAsync(List<AdmissionExamClassroomPostulant> admissionExamClassroomPostulants);
        Task InsertRange(List<AdmissionExamClassroomPostulant> classroomPostulants);
        Task<PostulantInformationTemplate> GetStudentClassroomInformation(Guid examId, string document);
    }
}

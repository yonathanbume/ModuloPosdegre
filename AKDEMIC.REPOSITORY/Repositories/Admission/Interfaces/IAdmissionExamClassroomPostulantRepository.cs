using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Admission.Templates.AdmissionExamClassroomPostulant;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces
{
    public interface IAdmissionExamClassroomPostulantRepository : IRepository<AdmissionExamClassroomPostulant>
    {
        Task<List<AdmissionExamClassroomPostulant>> GetClassroomPostulants(Guid id);
        Task<List<AdmissionExamClassroomPostulant>> GetStudentClassroom(Guid id);
        Task<PostulantInformationTemplate> GetStudentClassroomInformation(Guid examId, string document);
    }
}

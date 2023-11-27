using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Admission.Interfaces
{
    public interface IAdmissionExamClassroomService
    {
        Task Insert(AdmissionExamClassroom admissionExamClassroom);
        Task Update(AdmissionExamClassroom admissionExamClassroom);
        Task Delete(AdmissionExamClassroom admissionExamClassroom);
        Task<AdmissionExamClassroom> Get(Guid id);
        Task<IEnumerable<AdmissionExamClassroom>> GetAll();
        Task<object> GetClassrooms(Guid id);
        Task<DataTablesStructs.ReturnedData<object>> GetClassroomsDatatable(DataTablesStructs.SentParameters sentParameters, Guid id);
        void Remove(AdmissionExamClassroom admissionExamClassroom);
        Task<List<AdmissionExamClassroom>> GetClassroomsListById(Guid id);
        Task<bool> Any(Guid id, Guid classroomId);
        Task InsertRange(IEnumerable<AdmissionExamClassroom> examClassrooms);
        Task<AdmissionExamClassroom> GetWithClassroomBuildingAndCampus(Guid id);
        Task<IEnumerable<AdmissionExamClassroom>> GetListByTeacherId(string userId, Guid? admissionTermId = null);
        Task<List<AdmissionExamClassroom>> GetAllByExam(Guid examId);
        Task<DataTablesStructs.ReturnedData<object>> GetAssistanceDatatable(DataTablesStructs.SentParameters sentParameters, Guid admissionExamnId);
        Task<DataTablesStructs.ReturnedData<object>> GetClassroomAssistanceDatatable(DataTablesStructs.SentParameters sentParameters, Guid classroomId);
    }
}

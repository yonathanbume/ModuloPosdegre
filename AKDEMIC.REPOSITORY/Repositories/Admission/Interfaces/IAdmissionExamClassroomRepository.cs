using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces
{
    public interface IAdmissionExamClassroomRepository : IRepository<AdmissionExamClassroom>
    {
        Task<object> GetClassrooms(Guid id);
        Task<DataTablesStructs.ReturnedData<object>> GetClassroomsDatatable(DataTablesStructs.SentParameters sentParameters, Guid id);
        void Remove(AdmissionExamClassroom admissionExamClassroom);
        Task<List<AdmissionExamClassroom>> GetClassroomsListById(Guid id);
        Task<bool> Any(Guid id, Guid classroomId);
        Task<AdmissionExamClassroom> GetWithClassroomBuildingAndCampus(Guid id);
        Task<IEnumerable<AdmissionExamClassroom>> GetListByTeacherId(string userId, Guid? admissionTermId = null);
        Task<List<AdmissionExamClassroom>> GetAllByExam(Guid examId);
        Task<DataTablesStructs.ReturnedData<object>> GetAssistanceDatatable(DataTablesStructs.SentParameters sentParameters, Guid admissionExamnId);
        Task<DataTablesStructs.ReturnedData<object>> GetClassroomAssistanceDatatable(DataTablesStructs.SentParameters sentParameters, Guid classroomId);
    }
}

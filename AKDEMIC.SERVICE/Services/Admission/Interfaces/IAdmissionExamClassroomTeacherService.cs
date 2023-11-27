using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Repositories.Admission.Templates.AdmissionExamClassroomTeacher;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Admission.Interfaces
{
    public interface IAdmissionExamClassroomTeacherService
    {
        Task<List<AdmissionExamClassroomTeacher>> GetByAdmissionExamClassroomId(Guid id);
        Task DeleteByAdmissionExamClassroomId(Guid id);
        Task UpdateRange(List<AdmissionExamClassroomTeacher> teachers);
        Task DeleteById(Guid id);
        Task InsertRange(List<AdmissionExamClassroomTeacher> teachers);
        Task InsertRange(IEnumerable<AdmissionExamClassroomTeacher> newTeachers);
        Task<DataTablesStructs.ReturnedData<object>> GetClassroomsAssistanceDatatable(DataTablesStructs.SentParameters sentParameters, Guid id);
        Task<List<ClassroomAssistanceTemplate>> GetClassroomsAssistanceData(Guid id);
        Task SaveTeacherAssistance(Guid id, List<string> lstToAdd, List<string> lstToAvoid);
        Task<List<AdmissionExamClassroomTeacher>> GetByAdmissionExamId(Guid examId);
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Admission.Templates.AdmissionExamClassroomTeacher;

namespace AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces
{
    public interface IAdmissionExamClassroomTeacherRepository : IRepository<AdmissionExamClassroomTeacher>
    {
        Task<List<AdmissionExamClassroomTeacher>> GetByAdmissionExamClassroomId(Guid id);
        Task DeleteByAdmissionExamClassroomId(Guid id);
        Task<DataTablesStructs.ReturnedData<object>> GetClassroomsAssistanceDatatable(DataTablesStructs.SentParameters sentParameters, Guid id);
        Task SaveTeacherAssistance(Guid id, List<string> lstToAdd, List<string> lstToAvoid);
        Task<List<AdmissionExamClassroomTeacher>> GetByAdmissionExamId(Guid examId);
        Task<List<ClassroomAssistanceTemplate>> GetClassroomsAssistanceData(Guid id);
    }
}

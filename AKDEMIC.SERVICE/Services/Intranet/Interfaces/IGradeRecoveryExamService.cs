using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface IGradeRecoveryExamService
    {
        Task<GradeRecoveryExam> Get(Guid id);
        Task Insert(GradeRecoveryExam entity);
        Task Update(GradeRecoveryExam entity);
        Task<DataTablesStructs.ReturnedData<object>> GetGradeRecoveryDetailDatatable(DataTablesStructs.SentParameters parameters, Guid? careerId, Guid? curriculumId, int? cycle, Guid? courseId, string searchValue);

        Task<IEnumerable<GradeRecoveryExam>> GetGradeRecoveryByStudent(Guid studentId, Guid termId);
        Task<bool> AnyBySectionId(Guid sectionId);
        Task<DataTablesStructs.ReturnedData<object>> GetGradeRecoveryExamByTeacherDatatable(DataTablesStructs.SentParameters parameters, byte? status, string teacherId);

    }
}

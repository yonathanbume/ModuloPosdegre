using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces
{
    public interface IGradeRecoveryExamRepository : IRepository<GradeRecoveryExam>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetGradeRecoveryDetailDatatable(DataTablesStructs.SentParameters parameters, Guid? careerId, Guid? curriculumId, int? cycle, Guid? courseId, string searchValue);
        Task<IEnumerable<GradeRecoveryExam>> GetGradeRecoveryByStudent(Guid studentId, Guid termId);
        Task<bool> AnyBySection(Guid sectionId);
        Task<DataTablesStructs.ReturnedData<object>> GetGradeRecoveryExamByTeacherDatatable(DataTablesStructs.SentParameters parameters, byte? status, string teacherId);
    }
}

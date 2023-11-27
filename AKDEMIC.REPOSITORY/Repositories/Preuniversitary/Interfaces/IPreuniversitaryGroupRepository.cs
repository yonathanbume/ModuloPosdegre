using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Preuniversitary;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Preuniversitary.Interfaces
{
    public interface IPreuniversitaryGroupRepository : IRepository<PreuniversitaryGroup>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid preuniversitaryTermId, string searchValue);
        Task<object> GetStudentsByGroupId(Guid groupId);
        Task<DataTablesStructs.ReturnedData<object>> GetReportAdvanceDatatable(DataTablesStructs.SentParameters sentParameters, Guid preuniversitaryTermId, string searchValue = null);
        Task<object> GetTemaries(Guid groupId, bool? status = null);
        Task<List<PreuniversitaryGroup>> GetAllByTermIdAndCourseId(Guid preuniversitaryTermId, Guid preuniversitaryCourseId);
        Task<DataTablesStructs.ReturnedData<object>> GetGroupsDatatable(DataTablesStructs.SentParameters sentParameters, Guid courseId, Guid termId, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetCourseGroupByTeacherDatatable(DataTablesStructs.SentParameters sentParameters, Guid currentTermId, string teacherId, string searchValue = null);
    }
}

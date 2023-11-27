using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Preuniversitary;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Preuniversitary.Interfaces
{
    public interface IPreuniversitaryGroupService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid preuniversitaryTermId, string searchValue);
        Task<object> GetStudentsByGroupId(Guid groupId);
        Task<DataTablesStructs.ReturnedData<object>> GetReportAdvanceDatatable(DataTablesStructs.SentParameters sentParameters, Guid preuniversitaryTermId, string searchValue = null);
        Task<object> GetTemaries(Guid groupId, bool? status = null);
        Task<List<PreuniversitaryGroup>> GetAllByTermIdAndCourseId(Guid preuniversitaryTermId, Guid preuniversitaryCourseId);
        Task<PreuniversitaryGroup> Get(Guid id);
        Task<DataTablesStructs.ReturnedData<object>> GetGroupsDatatable(DataTablesStructs.SentParameters sentParameters, Guid courseId, Guid termId, string searchValue = null);
        Task Insert(PreuniversitaryGroup entity);
        Task Update(PreuniversitaryGroup entity);
        Task Delete(PreuniversitaryGroup entity);
        Task<DataTablesStructs.ReturnedData<object>> GetCourseGroupByTeacherDatatable(DataTablesStructs.SentParameters sentParameters, Guid currentTermId, string teacherId, string searchValue = null);
    }
}

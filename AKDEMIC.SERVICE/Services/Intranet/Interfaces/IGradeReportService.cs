using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.GradeReport;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface IGradeReportService
    {
        Task<object> GetStudentByUserName(string username);
        Task<object> GetStudentByUserNameBachelor(string username);
        Task<GradeReport> Get(Guid id);
        Task<GradeReport> Add(GradeReport model);
        Task Insert(GradeReport model);
        Task SaveChanges();
        Task Update(GradeReport model);
        Task<DataTablesStructs.ReturnedData<object>> GetGradeReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid? careerId, int gradeType, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetGradeReportByStudentDatatable(DataTablesStructs.SentParameters sentParameters, int gradeType,string userId, string searchValue = null);
        Task<GradeReport> GetGradeReportWithIncludes(Guid id);
        Task<GradeReport> GetGradeReportBachelor(Guid studentId);
        Task<string> GetPromotionGrade(Guid studentId);
        Task<bool> ExistGradeReport(Guid studentId, int gradeType);
        Task<string> GetEndDateByTermId(Guid termId);
        Task<GradeReport> GetByStudentIdAndGradeType(Guid studentId, int gradeType);
        Task<StudentInfoTemplate> GetStudentByStudentId(Guid studentId);
    }
}

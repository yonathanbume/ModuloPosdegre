using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.ClassStudent;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface IClassStudentService
    {
        Task<DataTablesStructs.ReturnedData<SectionAbsenceDetailDataTableTemplate>> GetSectionAbsenceDetailDataTable(DataTablesStructs.SentParameters parameters, Guid sid, Guid aid, int filter);
        Task<ClassStudent> Get(Guid classStudentId);
        Task<IEnumerable<object>> GetStudentAssistances(Class @class, string teacherId, Guid? classId = null);
        Task<object> GetClassStudentsOldAssistance(Guid classId, string teacherId = null);
        Task<IEnumerable<ClassStudent>> GetAll();
        Task<IEnumerable<ClassStudent>> GetAll(Guid? sectionId = null, Guid? studentId = null, DateTime? startTime = null, DateTime? endTime = null, bool? absent = null, Guid? classId = null, string teacherId = null);
        Task Insert(ClassStudent classStudent);
        Task Update(ClassStudent classStudent);
        Task Delete(ClassStudent classStudent);
        Task<DataTablesStructs.ReturnedData<object>> GetCoursesAssistanceDetailByStudentAndSectionDatatable(DataTablesStructs.SentParameters parameters, Guid studentId, Guid sectionId);
        Task<List<ClassStudent>> GetClassesBySectionId(Guid sectionId);
        Task<IEnumerable<Select2Structs.Result>> GetClassStudentSelect2ClientSide(string userId, Guid sectionId, bool isAbsent = false);
        Task CreateClassStudentsJob(Guid? sectionId, Guid id);
        Task<bool> AnyAssistanceByClassScheduleId(Guid classScheduleId, Guid sectionId);
        Task AssignDPI(Guid sectionId);
        Task<List<ClassStudentReportTemplate>> GetClassStudentReport(Guid sectionId);
        Task<List<ClassStudent>> GetClassStudentsByStudentSection(Guid studentId, Guid sectionId);
    }
}

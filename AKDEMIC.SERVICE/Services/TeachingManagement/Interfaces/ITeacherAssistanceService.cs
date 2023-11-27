using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces
{
    public interface ITeacherAssistanceService
    {
        Task<TeacherAssistance> GetByFilter(string userId = null, DateTime? time = null);
        Task<List<TeacherAssistance>> GetByFilter(DateTime? time);
        Task InsertAsync(TeacherAssistance teacherAssistance);
        Task UpdateAsync(TeacherAssistance teacherAssistance);
        Task<DataTablesStructs.ReturnedData<WorkingDay>> GetReport(DataTablesStructs.SentParameters sentParameters, Guid? facultyId, Guid? careerId, DateTime? starttime, DateTime? endtime);
        Task<List<WorkingDay>> GetReportData(Guid? facultyId, Guid? careerId, DateTime? starttime, DateTime? endtime);
        Task InsertRangeAsync(List<TeacherAssistance> listassistance);
    }
}